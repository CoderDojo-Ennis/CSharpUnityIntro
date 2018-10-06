using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GeekyMonkey
{
    public class GmWrapCam : MonoBehaviour
    {
        /// <summary>
        /// Singleton Instance
        /// </summary>
        public static GmWrapCam Instance;

        public Camera OriginalCamera;

        [Header("Wrap Options")]
        [Range(0, 0.5f)]
        public float WrapPercent = 0.5f;
        public MeshRenderer WrapMeshRenderer;

        /***********************************/
        /*** Calculated World Dimensions ***/
        [HideInInspector]
        public Vector3 WorldBottomLeft;
        [HideInInspector]
        public Vector3 WorldTopRight;
        [HideInInspector]
        public float WorldWidth;
        [HideInInspector]
        public float WorldHeight;
        /***********************************/

        private float OriginalCameraSize;
        private int RenderTextureWidth;
        private int RenderTextureHeight;
        private Camera WrapCam;
        private int WrapCamLayer;
        private int WrapCamMask;

        private void Awake()
        {
            Instance = this;
            if (OriginalCamera == null)
            {
                throw new Exception("GeekyMonkeyWrapCam must be attached to a camera object.");
            }
            CalculateScreenWorldDimensions();
        }

        private void OnEnable()
        {
            GenerateWrapCam();
            SetupWrapCam();
            SetupMesh();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }

        private void GenerateWrapCam()
        {
            OriginalCameraSize = OriginalCamera.orthographicSize;
            WrapCam = gameObject.GetComponent<Camera>();
            //WrapCam.clearFlags = CameraClearFlags.Nothing;
            WrapCamLayer = LayerMask.NameToLayer("WrapCam");
            if (WrapCamLayer < 1)
            {
                WrapCamLayer = 31;
            }
            WrapCamMask = 1 << WrapCamLayer;
            WrapCam.cullingMask = WrapCamMask;
            WrapCam.orthographicSize = OriginalCameraSize;
            WrapCam.orthographic = true;
            WrapCam.enabled = true;
        }

        private void SetupWrapCam()
        {
            float WrapScale = 1 + (WrapPercent * 2);
            RenderTextureWidth = (int)(Screen.width * WrapScale);
            RenderTextureHeight = (int)(Screen.height * WrapScale);
            OriginalCamera.orthographicSize = OriginalCameraSize * WrapScale;
            OriginalCamera.depthTextureMode = DepthTextureMode.Depth;
            if (OriginalCamera.targetTexture != null)
            {
                OriginalCamera.targetTexture.Release();
            }
            OriginalCamera.targetTexture = new RenderTexture(RenderTextureWidth, RenderTextureHeight, 24, RenderTextureFormat.ARGB32);
            OriginalCamera.targetTexture.Create();
        }

        private void SetupMesh()
        {
            WrapMeshRenderer = GetComponentInChildren<MeshRenderer>();
            WrapMeshRenderer.material.mainTexture = OriginalCamera.targetTexture;

            MeshFilter mf = WrapMeshRenderer.gameObject.GetComponent<MeshFilter>();
            var mesh = new Mesh();
            mf.mesh = mesh;

            int QuadCount = 9;

            Vector3[] vertices = new Vector3[4 * QuadCount];
            int[] tri = new int[6 * QuadCount];
            Vector3[] normals = new Vector3[4 * QuadCount];
            Vector2[] uv = new Vector2[4 * QuadCount];

            void MeshSetQuad(int index,
                float srcX1, float srcX2, float dstX1, float dstX2,
                float srcY1, float srcY2, float dstY1, float dstY2)
            {
                vertices[index * 4 + 0] = new Vector3(dstX1, dstY1, 0);
                vertices[index * 4 + 1] = new Vector3(dstX2, dstY1, 0);
                vertices[index * 4 + 2] = new Vector3(dstX1, dstY2, 0);
                vertices[index * 4 + 3] = new Vector3(dstX2, dstY2, 0);

                tri[index * 6 + 0] = index * 4 + 0;
                tri[index * 6 + 1] = index * 4 + 2;
                tri[index * 6 + 2] = index * 4 + 1;

                tri[index * 6 + 3] = index * 4 + 2;
                tri[index * 6 + 4] = index * 4 + 3;
                tri[index * 6 + 5] = index * 4 + 1;

                normals[index * 4 + 0] = -Vector3.forward;
                normals[index * 4 + 1] = -Vector3.forward;
                normals[index * 4 + 2] = -Vector3.forward;
                normals[index * 4 + 3] = -Vector3.forward;

                uv[index * 4 + 0] = new Vector2(srcX1, srcY1);
                uv[index * 4 + 1] = new Vector2(srcX2, srcY1);
                uv[index * 4 + 2] = new Vector2(srcX1, srcY2);
                uv[index * 4 + 3] = new Vector2(srcX2, srcY2);
            }

            float percentScale = 1 + (WrapPercent * 2);
            float pScaled = WrapPercent / percentScale;
            float s1 = 0;
            float s2 = pScaled;
            float s3 = 1 - pScaled;
            float s4 = 1;
            float d1 = 0;
            float d2 = WrapPercent;
            float d3 = 1 - WrapPercent;
            float d4 = 1;
            MeshSetQuad(0, s2, s3, d1, d4, s2, s3, d1, d4); // Middle
            MeshSetQuad(1, s1, s2, d3, d4, s2, s3, d1, d4); // Right
            MeshSetQuad(2, s3, s4, d1, d2, s2, s3, d1, d4); // Left
            MeshSetQuad(3, s2, s3, d1, d4, s1, s2, d3, d4); // Top
            MeshSetQuad(4, s2, s3, d1, d4, s3, s4, d1, d2); // Bottom
            MeshSetQuad(5, s3, s4, d1, d2, s3, s4, d1, d2); // Top left
            MeshSetQuad(6, s1, s2, d3, d4, s3, s4, d1, d2); // Top Right
            MeshSetQuad(7, s3, s4, d1, d2, s1, s2, d3, d4); // Bottom left
            MeshSetQuad(8, s1, s2, d3, d4, s1, s2, d3, d4); // Bottom Right

            mesh.vertices = vertices;
            mesh.triangles = tri;
            mesh.normals = normals;
            mesh.uv = uv;
            mf.mesh = mesh;
        }

        private void CalculateScreenWorldDimensions()
        {
            WorldBottomLeft = OriginalCamera.ViewportToWorldPoint(new Vector3(0, 0, 0));
            WorldTopRight = OriginalCamera.ViewportToWorldPoint(new Vector3(1, 1, 0));
            WorldWidth = WorldTopRight.x - WorldBottomLeft.x;
            WorldHeight = WorldTopRight.y - WorldBottomLeft.y;
        }

        /// <summary>
        /// Wrap the center of a transform if it's gone off screen
        /// </summary>
        /// <param name="transform"></param>
        public bool WrapToScreen(Transform transform)
        {
            bool wrapped = false;
            float x = transform.position.x;
            float y = transform.position.y;
            while (x < WorldBottomLeft.x)
            {
                x += WorldWidth;
                wrapped = true;
            }
            while (x > WorldTopRight.x)
            {
                x -= WorldWidth;
                wrapped = true;
            }
            while (y < WorldBottomLeft.y)
            {
                y += WorldHeight;
                wrapped = true;
            }
            while (y > WorldTopRight.y)
            {
                y -= WorldHeight;
                wrapped = true;
            }

            if (wrapped)
            {
                transform.position = new Vector3(x, y, transform.position.z);
            }

            return wrapped;
        }

        /*
        private void OnPostRender()
        {
            OriginalCamera.targetTexture.DiscardContents(true, true);
            OriginalCamera.targetTexture.Create();
        }
        */

        /*
        void OnRenderImage(RenderTexture src, RenderTexture dest)
        {
            Graphics.Blit(src, dest, new Vector2(1,1), new Vector2(0.1f, 0.1f));
        }
        */
    }
}