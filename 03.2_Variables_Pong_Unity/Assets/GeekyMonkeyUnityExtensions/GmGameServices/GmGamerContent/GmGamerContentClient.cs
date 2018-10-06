using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UniRx;
using Newtonsoft.Json;
using UniRx.Async;

namespace GeekyMonkey
{
    /// <summary>
    /// Network client for the Geeky Monkey Gamer Content Service
    /// </summary>
    public static class GmGamerContentClient
    {
        /// <summary>
        /// Known mime types
        /// </summary>
        public static Dictionary<GmGamerContentFileType, string> MimeTypes = new Dictionary<GmGamerContentFileType, string> {
            { GmGamerContentFileType.jpg, "image/jpeg" },
            { GmGamerContentFileType.png, "image/png" },
            { GmGamerContentFileType.gif, "image/gif" },
            { GmGamerContentFileType.json, "application/xml" },
            { GmGamerContentFileType.xml, "application/jpeg" },
            { GmGamerContentFileType.text, "text/plain" },
            { GmGamerContentFileType.binary, "application/octet-stream" }
        };

        /// <summary>
        /// Get the mime type based on the file extension
        /// </summary>
        /// <param name="fileName">File name</param>
        /// <returns>Mime type</returns>
        private static GmGamerContentFileType GetFileTimeFromFileName(string fileName)
        {
            string fileExt = (Path.GetExtension(fileName) ?? string.Empty).ToLower().Replace(".","");

            GmGamerContentFileType fileType;
            switch (fileExt)
            {
                case "jpg":
                case "jpeg":
                    fileType = GmGamerContentFileType.jpg;
                    break;
                case "gif":
                    fileType = GmGamerContentFileType.gif;
                    break;
                case "png":
                    fileType = GmGamerContentFileType.png;
                    break;
                case "txt":
                    fileType = GmGamerContentFileType.text;
                    break;
                case "json":
                    fileType = GmGamerContentFileType.json;
                    break;
                case "xml":
                    fileType = GmGamerContentFileType.xml;
                    break;
                default:
                    fileType = GmGamerContentFileType.binary;
                    break;
            }
            return fileType;
        }

        /// <summary>
        /// Get the url for a file
        /// </summary>
        /// <param name="gamerId">Gamer ID</param>
        /// <param name="fileName">File Name</param>
        /// <returns></returns>
        public static string GetFileContentUrl(string gamerId, string fileName)
        {
            return $"https://geekymonkeygamestorage.blob.core.windows.net/gamercontent/{GmGameServicesClient.GameId}/{gamerId}/{fileName}";
        }

        /// <summary>
        /// Donwload a file
        /// </summary>
        /// <param name="gamerId">Gamer ID</param>
        /// <param name="fileName">File Name</param>
        /// <param name="progressCallback">Progress callback used to update UI</param>
        /// <returns>File Bytes</returns>
        public static async UniTask<byte[]> DownloadGamerFile(
            string gamerId,
            string fileName,
            Func<float, bool> progressCallback = null)
        {
            byte[] fileBytes = null;
            try
            {
                string url = GetFileContentUrl(gamerId, fileName);
                Debug.Log("Download from " + url);

                UnityWebRequest uwr = UnityWebRequest.Get(url);
                uwr.redirectLimit = 2;
                uwr.useHttpContinue = true;
                uwr.downloadHandler = new DownloadHandlerBuffer();

                var result = await uwr.SendWebRequest()
                    .ConfigureAwait(new Progress<float>((progress) =>
                    {
                        if (progressCallback != null)
                        {
                            // Don't let progress to go 100 if it's not done
                            if (progress > 0.99f && (uwr.downloadProgress < 1 || !uwr.isDone))
                            {
                                progress = 0.99f;
                            }
                            progressCallback.Invoke(progress);
                        }
                    }));

                Debug.Log("Download completed");

                if (result.isNetworkError)
                {
                    Debug.Log("Download network error");
                    Events.Raise(new GmGamerContentDownloadEvent
                    {
                        Success = false,
                        ErrorMessage = uwr.error,
                        ErrorCode = 1,
                        GamerId = gamerId,
                        FileName = fileName,
                        FileBytes = null
                    });
                    return null;
                }

                fileBytes = result.downloadHandler.data;
            }
            catch (Exception ex)
            {
                Debug.Log("Download exception: " + ex.Message);
                Events.Raise(new GmGamerContentDownloadEvent
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    ErrorCode = 2,
                    GamerId = gamerId,
                    FileName = fileName,
                    FileBytes = null
                });
                return null;
            }

            Debug.Log("Download success: " + fileBytes.Length);
            Events.Raise(new GmGamerContentDownloadEvent
            {
                Success = fileBytes != null && fileBytes.Length > 0,
                GamerId = gamerId,
                FileName = fileName,
                FileBytes = fileBytes
            });

            return fileBytes;
        }

        /// <summary>
        /// JSON serialize the metadata
        /// </summary>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public static string FormatMetadataAsJson(Dictionary<string,string> metadata)
        {
            var metadataJson = new StringBuilder("{");
            bool first = true;
            foreach (var keyval in metadata)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    metadataJson.Append(',');
                }
                metadataJson.Append($"\"{keyval.Key}\":\"{keyval.Value}\"");
            }
            metadataJson.Append("}");
            return metadataJson.ToString();
        }

        /// <summary>
        /// Remove forbidden characters from the file name
        /// </summary>
        /// <param name="fileName">Dirty file name</param>
        /// <returns>Clean file name</returns>
        private static string CleanFileName(string fileName)
        {
            string clean = Regex.Replace(fileName, @"[\/\\:]", "_");
            return clean;
        }

        /// <summary>
        /// Upload a file
        /// </summary>
        /// <param name="gamerId">User identifier - game can use deivce id or GamerTag id</param>
        /// <param name="fileName">File Name (slashes not allowed)</param>
        /// <param name="fileBytes">Content of File being uploaded</param>
        /// <param name="metadata">Additional data to store with the file</param>
        /// <param name="fileType">File Type used to set mime type</param>
        /// <param name="progressCallback">Progress callback used to update UI</param>
        /// <returns>Json details of success or failure/</returns>
        public static async UniTask<GmGamerContentFileInfo> SaveFileAsync(
            string gamerId,
            string fileName,
            byte[] fileBytes,
            Dictionary<string, string> metadata,
            GmGamerContentFileType fileType = GmGamerContentFileType.autoFromFileName,
            Func<float, bool> progressCallback = null
            )
        {
            GmGameServicesClient.CheckApiKey();

            // Remove forbidden characters from the file name
            fileName = CleanFileName(fileName);

            // Mime type
            string mimeType;
            if (fileType == GmGamerContentFileType.autoFromFileName)
            {
                fileType = GetFileTimeFromFileName(fileName);
            }
            mimeType = MimeTypes[fileType];

            // Convert the metadata to json
            string metadataJson = FormatMetadataAsJson(metadata);

            // Build the request
            string url = $"{GmGameServicesClient.BaseUrl}/gamercontent/save/{GmGameServicesClient.GameId}/{gamerId}";

            List<IMultipartFormSection> form = new List<IMultipartFormSection>
             {
                new MultipartFormFileSection("file", fileBytes, fileName, mimeType),
                new MultipartFormDataSection("metadata", metadataJson)
            };

            var uwr = UnityWebRequest.Post(url, form);
            uwr.useHttpContinue = true;
            uwr.redirectLimit = 2;
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);

            // Wait for completion
            var result = await uwr.SendWebRequest()
                .ConfigureAwait(new Progress<float>((progress) => { 
                    if (progressCallback != null)
                    {
                        // Don't let progress to go 100 if it's not done
                        if (progress > 0.99f && (uwr.downloadProgress < 1 || !uwr.isDone))
                        {
                            progress = 0.99f;
                        }
                        progressCallback.Invoke(progress);
                    }
                }));

            if (result.isNetworkError)
            {
                Events.Raise(new GmGamerContentSaveEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1,
                    FileInfo = null
                });

                return null;
            }

            string responseText = result.downloadHandler.text;
            var fileInfoResponse = JsonConvert.DeserializeObject<GmGamerContentFileInfoResponse>(responseText);
            var fileInfo = fileInfoResponse.FileInfo;
            if (fileInfo == null)
            {
                Debug.Log("Response didnt deserialize");
            }
            var evnt = new GmGamerContentSaveEvent
                {
                    Success = fileInfoResponse.Success,
                    FileInfo = fileInfo
                };
                Events.Raise(evnt);

            return fileInfo;
        }

        /// <summary>
        /// Get the lsit of files for a gamer
        /// </summary>
        /// <param name="gamerId">Gamer ID</param>
        /// <returns>List of file infos</returns>
        public static async UniTask<List<GmGamerContentFileInfo>> GetGamerFileList(string gamerId)
        {
            GmGameServicesClient.CheckApiKey();

            string url = $"{GmGameServicesClient.BaseUrl}/gamercontent/list/{GmGameServicesClient.GameId}/{gamerId}";

            UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.useHttpContinue = true;
            uwr.redirectLimit = 2;
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);
            await uwr.SendWebRequest().AsObservable();
            if (uwr.isNetworkError)
            {
                Events.Raise(new GmGamerContentListEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1,
                    FileInfos = null
                });
                return null;
            }

            string responseText = uwr.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<GmGamerContentListResponse>(responseText);
            var files = response.Files;

            Events.Raise(new GmGamerContentListEvent
            {
                Success = response.Success,
                FileInfos = files
            });

            return files;
        }

        /// <summary>
        /// Delete a gamer file
        /// </summary>
        /// <param name="gamerId">Gamer ID</param>
        /// <param name="fileName">File Name</param>
        /// <returns>True if deleted</returns>
        public static async UniTask<bool> DeleteGamerFile(string gamerId, string fileName)
        {
            GmGameServicesClient.CheckApiKey();

            string url = $"{GmGameServicesClient.BaseUrl}/gamercontent/delete/{GmGameServicesClient.GameId}/{gamerId}/{fileName}";

            fileName = CleanFileName(fileName);

            UnityWebRequest uwr = UnityWebRequest.Get(url);
            uwr.method = "DELETE";
            uwr.SetRequestHeader("Authentication", GmGameServicesClient.GameApiKey);
            await uwr.SendWebRequest().AsObservable();
            if (uwr.isNetworkError)
            {
                Events.Raise(new GmGamerContentDeleteEvent
                {
                    Success = false,
                    ErrorMessage = uwr.error,
                    ErrorCode = 1,
                    GamerId = gamerId,
                    FileName = fileName
                });
                return false;
            }

            string responseText = uwr.downloadHandler.text;
            var response = JsonConvert.DeserializeObject<GmGamerContentFileDeleteResponse>(responseText);

            Events.Raise(new GmGamerContentDeleteEvent
            {
                Success = response.Success,
                FileName = response.FileName
            });

            return true;
        }
    }
}
