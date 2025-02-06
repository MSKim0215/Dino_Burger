using UnityEditor;
using UnityEngine;
using System.IO;

namespace IconMakerPro
{
    public static class IconSaver
    {
        private const string folderName = "Assets/ArcaneCanvas/IconMakerPro/Art/Icons";


        public static void GenerateIcon(GameObject selectedObject, string iconName, int selectedSizeIndex, int selectedFormatIndex, bool transparentBackground, Color backgroundColor, Texture2D previewIcon, bool locked, Texture2D lockIcon = null, float lockIconScale = 1f)

        {
            if (!AssetDatabase.IsValidFolder(folderName))
            {
                AssetDatabase.CreateFolder("Assets/ArcaneCanvas/IconMakerPro/Art", "Icons");
            }

            string path = $"{folderName}/{iconName}";
            int index = 1;

            while (File.Exists(path + ".png") || File.Exists(path + ".jpg"))
            {
                path = $"{folderName}/{iconName}{index}";
                index++;
            }

            EditorUtility.DisplayProgressBar("Icon Generation", "Preparing the icon...", 0f);

            if (transparentBackground)
            {
            }
            else
            {
                previewIcon = ApplyBackgroundColor(previewIcon, backgroundColor);
            }

            EditorUtility.DisplayProgressBar("Icon Generation", "Applying background and overlays...", 0.3f);

            // Apply lock icon overlay if the option is enabled
            if (locked && lockIcon != null)
            {
                previewIcon = OverlayLockIcon(previewIcon, lockIcon, lockIconScale); // Pass the scale value here
            }

            EditorUtility.DisplayProgressBar("Icon Generation", "Saving icon...", 0.6f);

            if (selectedFormatIndex == 0)
            {
                path += ".png";
                File.WriteAllBytes(path, previewIcon.EncodeToPNG());
            }
            else if (selectedFormatIndex == 1)
            {
                if (!path.EndsWith(".jpg"))
                    path += ".jpg";
                File.WriteAllBytes(path, previewIcon.EncodeToJPG());
            }
            else if (selectedFormatIndex == 2)
            {
                path += ".png";
                File.WriteAllBytes(path, previewIcon.EncodeToPNG());
                EditorApplication.delayCall += () => GenerateSprite(path);
            }

            EditorUtility.DisplayProgressBar("Icon Generation", "Finalizing icon...", 0.9f);

            AssetDatabase.Refresh();

            EditorUtility.ClearProgressBar();

            EditorUtility.DisplayDialog("Success", $"Texture converted to sprite and saved to: Assets/ArcaneCanvas/IconMakerPro/Art/Icons", "OK");

            Debug.Log($"Icon saved to {path}");
        }



        private static Texture2D ApplyBackgroundColor(Texture2D icon, Color backgroundColor)
        {
            Texture2D newIcon = new Texture2D(icon.width, icon.height, TextureFormat.RGBA32, false);

            for (int y = 0; y < icon.height; y++)
            {
                for (int x = 0; x < icon.width; x++)
                {
                    newIcon.SetPixel(x, y, backgroundColor);
                }
            }

            for (int y = 0; y < icon.height; y++)
            {
                for (int x = 0; x < icon.width; x++)
                {
                    Color iconColor = icon.GetPixel(x, y);
                    newIcon.SetPixel(x, y, iconColor.a > 0 ? iconColor : backgroundColor);
                }
            }

            newIcon.Apply();
            return newIcon;
        }

        private static Texture2D OverlayLockIcon(Texture2D baseIcon, Texture2D overlayIcon, float lockIconScale)
        {
            Texture2D combinedIcon = new Texture2D(baseIcon.width, baseIcon.height, TextureFormat.RGBA32, false);

            float scaleFactor = lockIconScale;  // Use the scale passed from the slider
            int overlayIconWidth = Mathf.FloorToInt(baseIcon.width * scaleFactor);
            int overlayIconHeight = Mathf.FloorToInt(baseIcon.height * scaleFactor);

            Texture2D resizedOverlayIcon = new Texture2D(overlayIconWidth, overlayIconHeight);
            for (int y = 0; y < overlayIconHeight; y++)
            {
                for (int x = 0; x < overlayIconWidth; x++)
                {
                    int srcX = Mathf.FloorToInt((x / (float)overlayIconWidth) * overlayIcon.width);
                    int srcY = Mathf.FloorToInt((y / (float)overlayIconHeight) * overlayIcon.height);
                    resizedOverlayIcon.SetPixel(x, y, overlayIcon.GetPixel(srcX, srcY));
                }
            }
            resizedOverlayIcon.Apply();

            combinedIcon.SetPixels(baseIcon.GetPixels());

            int offsetX = (baseIcon.width - resizedOverlayIcon.width) / 2;
            int offsetY = (baseIcon.height - resizedOverlayIcon.height) / 2;

            for (int y = 0; y < resizedOverlayIcon.height; y++)
            {
                for (int x = 0; x < resizedOverlayIcon.width; x++)
                {
                    int targetX = offsetX + x;
                    int targetY = offsetY + y;

                    if (targetX < combinedIcon.width && targetY < combinedIcon.height)
                    {
                        Color baseColor = combinedIcon.GetPixel(targetX, targetY);
                        Color overlayColor = resizedOverlayIcon.GetPixel(x, y);

                        combinedIcon.SetPixel(targetX, targetY, Color.Lerp(baseColor, overlayColor, overlayColor.a));
                    }
                }
            }

            combinedIcon.Apply();
            return combinedIcon;
        }


        private static void GenerateSprite(string path)
        {
            TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;

            // Set texture type to sprite
            textureImporter.textureType = TextureImporterType.Sprite;
            textureImporter.spriteImportMode = SpriteImportMode.Single;

            // Ensure transparency is enabled
            textureImporter.alphaIsTransparency = true;
            textureImporter.mipmapEnabled = false;

            // Apply the settings
            textureImporter.SaveAndReimport();

            // Load the sprite to confirm it's been set correctly
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null)
            {
                Debug.Log("Sprite saved to " + path);
            }
        }

    }
}
