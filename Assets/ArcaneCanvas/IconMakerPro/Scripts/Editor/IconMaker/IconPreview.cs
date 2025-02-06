using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using IconMakerPro.Utils;
using Core;

namespace IconMakerPro.Editor
{
    public static class IconPreview
    {
        public static void GenerateIconPreviewFromAsset(GameObject asset, int selectedSizeIndex, bool transparentBackground, Color backgroundColor, ref Texture2D previewIcon, float rotationX, float rotationY, float rotationZ, float zoom, bool hideAllObjects, float lightIntensity, float lightRotation, int selectedShapeIndex)
        {
            string folderName = "Assets/ArcaneCanvas/IconMakerPro/Art/Icons";
            if (!AssetDatabase.IsValidFolder(folderName))
            {
                Debug.Log($"Creating folder: {folderName}");
                string parentFolder = "Assets/ArcaneCanvas/IconMakerPro/Art";
                if (!AssetDatabase.IsValidFolder(parentFolder))
                {
                    AssetDatabase.CreateFolder("Assets/ArcaneCanvas", "IconMakerPro");
                    AssetDatabase.CreateFolder("Assets/ArcaneCanvas/IconMakerPro", "Art");
                }
                AssetDatabase.CreateFolder(parentFolder, "Icons");
            }

            Dictionary<GameObject, bool> originalObjectStates = new Dictionary<GameObject, bool>();

            if (hideAllObjects)
            {
                foreach (GameObject obj in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
                {
                    if (obj != asset)
                    {
                        originalObjectStates[obj] = obj.activeSelf;
                        obj.SetActive(false);
                    }
                }
            }

            GameObject tempObject = null;
            GameObject camObject = null;
            GameObject lightObject = null;

            try
            {
                Vector3 isolatedPosition = Vector3.zero;

                // Instantiate asset for preview
                tempObject = GameObject.Instantiate(asset, isolatedPosition, Quaternion.identity);
                tempObject.transform.rotation = Quaternion.Euler(rotationX, rotationY, rotationZ);

                // Setup camera
                camObject = new GameObject("IconCamera");
                Camera renderCamera = camObject.AddComponent<Camera>();
                renderCamera.backgroundColor = backgroundColor;
                renderCamera.clearFlags = CameraClearFlags.SolidColor;

                // Calculate bounds and camera position
                Bounds bounds = IconUtils.GetObjectBounds(tempObject);
                Vector3 cameraPosition = bounds.center + new Vector3(0, 0, -zoom);
                renderCamera.transform.position = cameraPosition + isolatedPosition;
                renderCamera.transform.LookAt(bounds.center + isolatedPosition);

                // Setup light
                lightObject = new GameObject("IconLight");
                Light light = lightObject.AddComponent<Light>();
                light.type = LightType.Directional;
                light.intensity = lightIntensity;
                light.transform.rotation = Quaternion.Euler(lightRotation, lightRotation, lightRotation);

                // Render texture setup
                int iconSize = IconUtils.GetIconSize(selectedSizeIndex);
                RenderTexture renderTexture = new RenderTexture(iconSize, iconSize, 24, RenderTextureFormat.ARGB32);
                renderCamera.targetTexture = renderTexture;
                RenderTexture.active = renderTexture;

                renderCamera.Render();

                // Generate texture
                previewIcon = new Texture2D(iconSize, iconSize, TextureFormat.RGBA32, false);
                previewIcon.ReadPixels(new Rect(0, 0, iconSize, iconSize), 0, 0);

                // Apply mask if necessary
                switch (selectedShapeIndex)
                {
                    case 1:
                        previewIcon = IconGenerator.ApplyCircleMask(previewIcon);
                        break;
                    case 2:
                        previewIcon = IconGenerator.ApplySmoothCornersMask(previewIcon);
                        break;
                }

                previewIcon.Apply();

                // Cleanup render texture
                RenderTexture.active = null;
                renderCamera.targetTexture = null;
                Object.DestroyImmediate(renderTexture);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error generating icon preview: {ex.Message}");
            }
            finally
            {
                // Cleanup temporary objects
                if (tempObject != null) Object.DestroyImmediate(tempObject);
                if (camObject != null) Object.DestroyImmediate(camObject);
                if (lightObject != null) Object.DestroyImmediate(lightObject);

                // Restore hidden objects
                if (hideAllObjects)
                {
                    foreach (var kvp in originalObjectStates)
                    {
                        kvp.Key.SetActive(kvp.Value);
                    }
                }
            }
        }
    }
}
