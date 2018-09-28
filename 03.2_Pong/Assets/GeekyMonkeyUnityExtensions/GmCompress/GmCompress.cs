using System.IO;
using System.IO.Compression;
using System;
using System.Text;
using UniRx.Async;

namespace GeekyMonkey
{
    public static class GmCompress
    {
        /// <summary>
        /// Converts UTF8 String to gzip-compressed byte array
        /// </summary>
        /// <param name="toCompress">String to be compressed</param>
        /// <returns>ZIP compressed string</returns>
        public static async UniTask<byte[]> ZipCompressStringAsync(string toCompress)
        {
            using (MemoryStream output = new MemoryStream())
            {
                using (GZipStream compression = new GZipStream(output, CompressionMode.Compress))
                {
                    using (StreamWriter writer = new StreamWriter(compression))
                    {
                        await writer.WriteAsync(toCompress);
                    }
                }
                return output.ToArray();
            }
        }

        /// <summary>
        /// Converts UTF8 String to gzip-compressed base64 string
        /// </summary>
        /// <param name="toCompress">String to be compressed</param>
        /// <returns>ZIP compressed string</returns>
        public static async UniTask<string> ZipCompressStringBase64Async(string toCompress)
        {
            byte[] zipBytes = await ZipCompressStringAsync(toCompress);
            return Convert.ToBase64String(zipBytes);
        }

        /// <summary>
        /// Converts Gzip-compressed byte array to UTF8 string
        /// </summary>
        /// <param name="toDecompress">Byte array to decompress</param>
        /// <returns>Decompressed string</returns>
        public static async UniTask<string> ZipDecompressStringAsync(byte[] toDecompress)
        {
            using (MemoryStream input = new MemoryStream(toDecompress))
            {
                using (GZipStream compression = new GZipStream(input, CompressionMode.Decompress))
                {
                    using (MemoryStream output = new MemoryStream())
                    {
                        await compression.CopyToAsync(output);
                        return Encoding.UTF8.GetString(output.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// Converts Gzip-compressed base64 string to UTF8 string
        /// </summary>
        /// <param name="toDecompress">Base64 string to decompress</param>
        /// <returns>Decompressed string</returns>
        public static async UniTask<string> ZipDecompressStringBase64Async(string toDecompress)
        {
            byte[] bytes = Convert.FromBase64String(toDecompress);
            return await ZipDecompressStringAsync(bytes);
        }
    }
}
