using System.Reflection;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using IconMakerPro.Utils;
using IconMakerPro.Editor;
using IconMakerPro;


namespace Core
{
    public class IconGenerator : EditorWindow
    {
        private GameObject selectedAsset;
        private string iconName = "NewIcon";
        private Color backgroundColor = Color.grey;
        private int selectedSizeIndex = 4;
        private int selectedFormatIndex = 2;
        private float rotationX = 0f;
        private float rotationY = 0f;
        private float rotationZ = 0f;
        private float zoom = 2f;
        private Texture2D previewIcon;
        private float lightRotation = 0f;  
        private Texture2D backgroundTexture;
        private bool showIconMakerManual = false;
        private bool showTextureToSpriteManual = false;
        private bool isBackground;
        public float lockIconScale = 1f;  // Store slider value




        private bool hideAllObjects = true;
        private bool locked = false;
        private float lightIntensity = 1f;
        private Texture2D lockIcon;
        private float transparencyAdjustment = 1f; 

        private enum BackgroundType
        {
            BackgroundColor,
            TransparentBackground,
        }

        private BackgroundType selectedBackgroundType = BackgroundType.BackgroundColor;


        private static string[] iconSizeOptions = { "32x32", "64x64", "128x128", "256x256", "512x512" };
        private static string[] shapeOptions = { "Square", "Fade", "Smooth corners", "Rhombus", "Circle", "Box", "Diamond" };

        private Vector2 previewScrollPosition = Vector2.zero;

        public int selectedShapeIndex = 0;

        private int selectedTab = 0;

        // New fields for Texture to Sprite functionality
        private Texture2D textureToConvert;
        private string spriteName = "NewSprite";
        private string savePath = "Assets/ArcaneCanvas/IconMakerPro/Art/Sprites";

        [MenuItem("Tools/Icon Maker Pro")]
        public static void ShowWindow()
        {
            
            IconGenerator window = GetWindow<IconGenerator>("Icon Maker");

            // minWidth minHeight
            window.minSize = new Vector2(849, 899); 
            window.maxSize = new Vector2(1200, 1200); 

        }


        private void OnEnable()
        {
            backgroundTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/ArcaneCanvas/IconMakerPro/Scripts/Editor/GUILayout/Forge.png");
        }

        private void OnGUI()
        {

            // Draw the background texture 
            if (backgroundTexture != null)
            {
                Rect backgroundRect = new Rect(0, 0, position.width, position.height);
                GUI.DrawTexture(backgroundRect, backgroundTexture);
            }

            EditorGUILayout.BeginHorizontal();

            // Left-side menu panel
            EditorGUILayout.BeginVertical(GUILayout.Width(150));
            GUILayout.Label("Menu", EditorStyles.boldLabel);

            // Buttons
            GUIStyle tabStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleLeft };

            if (GUILayout.Button("🎨 Icon Maker Pro", tabStyle))
            {
                selectedTab = 0;
            }

            if (GUILayout.Button("🖼️ Texture to Sprite", tabStyle))
            {
                selectedTab = 1;
            }

            if (GUILayout.Button("📖 Manual", tabStyle))
            {
                selectedTab = 2;
            }

            EditorGUILayout.EndVertical();


            GUILayout.Space(10); // space before the separator
            EditorGUILayout.BeginVertical(GUILayout.Width(2));  // Width of the separator line
            GUILayout.Box("", GUILayout.ExpandHeight(true));  // This creates a vertical line
            EditorGUILayout.EndVertical();

            // Main panel
            EditorGUILayout.BeginVertical();
            switch (selectedTab)
            {
                case 0:
                    DrawIconMakerSettings();
                    break;

                case 1:
                    DrawTextureToSpriteSettings();
                    break;

                case 2:
                    DrawManual();
                    break;
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }



        private void DrawIconMakerSettings()
        {
            GUIStyle customStyle = new GUIStyle(EditorStyles.boldLabel);
            customStyle.fontSize = 20; 

            GUILayout.Label("Generate 2D Icons from Assets", customStyle);


            // Input fields
            selectedAsset = (GameObject)EditorGUILayout.ObjectField(
            new GUIContent("Target Asset", "Select or drag and drop the GameObject that will have the icon applied to it."),
            selectedAsset,
            typeof(GameObject),
            false
    );
            iconName = EditorGUILayout.TextField(
            new GUIContent("Icon Name", "Enter the name for the icon."),
            iconName
    );
            selectedSizeIndex = EditorGUILayout.Popup(
            new GUIContent("Icon Size", "Select the size of the icon from the available options."),
            selectedSizeIndex,
            iconSizeOptions
    );

            


            // Background dropdown
            selectedBackgroundType = (BackgroundType)EditorGUILayout.EnumPopup(
            new GUIContent("Background Type", "Choose the background type: Color for a solid background, or Transparent for no background."),
            selectedBackgroundType
    );


            // Handle different background options
            if (selectedBackgroundType == BackgroundType.BackgroundColor)
            {
                if (backgroundColor == Color.clear)  // transparency
                {
                    backgroundColor = Color.grey;  // transparency -> grey
                }
                backgroundColor.a = 1f;  // reset opaque
                backgroundColor = EditorGUILayout.ColorField(
                new GUIContent("Background Color", "Select the background color for the icon."),
                backgroundColor
    );

                selectedShapeIndex = EditorGUILayout.Popup(
                new GUIContent("Background Shape", "Choose the shape of the color (e.g., square, circle, etc.)."),
                selectedShapeIndex,
                shapeOptions);


            }



            else if (selectedBackgroundType == BackgroundType.TransparentBackground)
            {

                
                backgroundColor = Color.clear;  
                selectedShapeIndex = 0;

            }

            {
                GUIStyle tempStyle = new GUIStyle(EditorStyles.boldLabel);
                tempStyle.fontSize = 15;
                GUILayout.Label("gameObject rotation", tempStyle);
            }




            // Sliders
            rotationX = EditorGUILayout.Slider(
        new GUIContent("Rotate Object X", "Adjust the rotation of the object along the X-axis from 0 to 360 degrees."),
        rotationX,
        0f,
        360f
    );

            rotationY = EditorGUILayout.Slider(
                new GUIContent("Rotate Object Y", "Adjust the rotation of the object along the Y-axis from 0 to 360 degrees."),
                rotationY,
                0f,
                360f
            );

            rotationZ = EditorGUILayout.Slider(
                new GUIContent("Rotate Object Z", "Adjust the rotation of the object along the Z-axis from 0 to 360 degrees."),
                rotationZ,
                0f,
                360f
            );

            {
                GUIStyle tempStyle = new GUIStyle(EditorStyles.boldLabel);
                tempStyle.fontSize = 15;
                GUILayout.Label("Camera Zoom", tempStyle);
            }

            zoom = EditorGUILayout.Slider(
                new GUIContent("Zoom", "Adjust the zoom level, controlling the object's size or camera field of view."),
                zoom,
                0f,
                20f



            );

            {
                GUIStyle tempStyle = new GUIStyle(EditorStyles.boldLabel);
                tempStyle.fontSize = 15;
                GUILayout.Label("Light settings", tempStyle);
            }
            // Lights
            lightRotation = EditorGUILayout.Slider(
                new GUIContent("Light Rotation", "Adjust the rotation angle of the directional light source."),
                lightRotation,
                0f,
                360f
            );

            lightIntensity = EditorGUILayout.Slider(
                new GUIContent("Light Intensity", "Adjust the intensity of the light source affecting the object."),
                lightIntensity,
                0f,
                5f
            );

            {
                GUIStyle tempStyle = new GUIStyle(EditorStyles.boldLabel);
                tempStyle.fontSize = 15;
                GUILayout.Label("Custom Icon overlay", tempStyle);
            }

            locked = EditorGUILayout.Toggle(
            new GUIContent("Add Overlay", "Enable this option to add an overlay to the icon."),
            locked
    );

            // Icon Overlay
            if (locked)
            {
                lockIcon = (Texture2D)EditorGUILayout.ObjectField(
                    new GUIContent("Overlay Sprite", "Select or drag and drop a sprite to use as an overlay on the icon."),
                    lockIcon,
                    typeof(Texture2D),
                    false
                );
                // Slider to control the scale of the overlay icon
                lockIconScale = EditorGUILayout.Slider(
                new GUIContent("Overlay Scale", "Adjust the scale of the overlay icon."),
                lockIconScale, // Current value of the slider
                0.1f,          // Min value (10%)
                2.0f           // Max value (200%)
                );

                if (lockIcon != null)
                {
                    string assetPath = AssetDatabase.GetAssetPath(lockIcon);
                    TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                    if (importer != null && !importer.isReadable)
                    {
                        EditorGUILayout.HelpBox(
                           "The selected texture isn't in a supported format. Please use 'Texture to Sprite' on this selected texture first before applying.",
                            MessageType.Warning
                        );
                    }
                }
            }








            if (GUILayout.Button("Generate Icon"))
            {
                if (selectedAsset != null && !string.IsNullOrEmpty(iconName))
                {
                    if (lockIcon != null)
                    {
                        string assetPath = AssetDatabase.GetAssetPath(lockIcon);
                        TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

                        if (importer != null && !importer.isReadable)
                        {
                            EditorUtility.DisplayDialog(
                                "Error",
                                "The selected texture isn't in a supported format. Please use 'Texture to Sprite' on this texture first or enable 'Read/Write Enabled' in the texture's import settings.",
                                "OK"
                            );
                            return; // Exit without proceeding
                        }
                    }

                    IconSaver.GenerateIcon(
                        selectedAsset,
                        iconName,
                        selectedSizeIndex,
                        selectedFormatIndex,
                        selectedBackgroundType == BackgroundType.TransparentBackground,
                        backgroundColor,
                        previewIcon,
                        locked,
                        lockIcon,
                        lockIconScale
                    );
                }
                else
                {
                    EditorUtility.DisplayDialog("Error", "Please select an asset and provide an icon name.", "OK");
                }
            }



            if (selectedAsset != null)
            {
                // Generate the icon 
                IconPreview.GenerateIconPreviewFromAsset(
                    selectedAsset,
                    selectedSizeIndex,
                    selectedBackgroundType == BackgroundType.TransparentBackground,
                    backgroundColor,
                    ref previewIcon,
                    rotationX,
                    rotationY,
                    rotationZ,
                    zoom,
                    hideAllObjects,
                    lightIntensity,
                    lightRotation,
                    selectedShapeIndex
                );

                // Shapemasks
                if (selectedShapeIndex == 1)  // Circle Mask
                {
                    previewIcon = ApplyCircleMask(previewIcon);
                }
                else if (selectedShapeIndex == 2)
                {
                    previewIcon = ApplySmoothCornersMask(previewIcon);
                }
                else if (selectedShapeIndex == 3)
                {
                    previewIcon = ApplyRhombusMask(previewIcon);
                }
                else if (selectedShapeIndex == 4)
                {
                    previewIcon = ApplyStarMask(previewIcon);
                }
                else if (selectedShapeIndex == 5)
                {
                    previewIcon = ApplyBoxMask(previewIcon);
                }
                else if (selectedShapeIndex == 6)
                {
                    previewIcon = ApplyDiamondMask(previewIcon);
                }

                GUILayout.BeginVertical();
                GUILayout.FlexibleSpace(); 
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace(); 
                GUILayout.Label(new GUIContent(previewIcon));
                GUILayout.FlexibleSpace(); 
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace(); 
                GUILayout.EndVertical();

            }
           
            // Horizontal line after the "Generate Icon" button
            GUILayout.Box("", GUILayout.ExpandWidth(true), GUILayout.Height(2));

            GUILayout.Space(15);
            // GUILayout.Label("Icon Preview", EditorStyles.boldLabel);

            previewScrollPosition = EditorGUILayout.BeginScrollView(previewScrollPosition, GUILayout.Width(0), GUILayout.Height(0));

            if (previewIcon != null)
            {
                // Size from IconUtils
                int selectedIconSize = IconUtils.GetIconSize(selectedSizeIndex);



                // Dynamically scale
                Rect previewRect = new Rect(0, 0, selectedIconSize, selectedIconSize);

                
                EditorGUI.DrawPreviewTexture(previewRect, previewIcon);
            }

            EditorGUILayout.EndScrollView();

        }


        private void DrawTextureToSpriteSettings()
        {
            GUIStyle customStyle = new GUIStyle(EditorStyles.boldLabel);
            customStyle.fontSize = 20; // Set the desired font size

            GUILayout.Label("Texture to Sprite", customStyle);

            // Input for texture to convert
            textureToConvert = (Texture2D)EditorGUILayout.ObjectField(
                new GUIContent("Texture to Convert", "Select or drag and drop a JPG, PNG, or Sprite texture to convert."),
                textureToConvert,
                typeof(Texture2D),
                false
            );

            spriteName = EditorGUILayout.TextField(
                new GUIContent("Sprite Name", "Enter the name for the sprite. This will be used to identify the sprite."),
                spriteName
            
            );

            // Slider to adjust transparency
            transparencyAdjustment = EditorGUILayout.Slider(
                new GUIContent("Transparency", "Adjust the transparency level of the texture. 0 is fully transparent, and 1 is fully opaque."),
                transparencyAdjustment,
                0f,
                1f
            );

            // New checkbox for Background option
            isBackground = EditorGUILayout.Toggle(
                new GUIContent("Background", "Enable if this texture is intended as a background."),
                isBackground
            );

            if (GUILayout.Button("Convert to Sprite"))
            {
                if (textureToConvert != null)
                    ConvertTextureToSprite(textureToConvert, spriteName, savePath, isBackground);
                else
                    EditorUtility.DisplayDialog("Error", "Please select a texture to convert.", "OK");
            }
        }

        private void ConvertTextureToSprite(Texture2D texture, string name, string path, bool isBackground)
        {
            string texturePath = AssetDatabase.GetAssetPath(texture);
            TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;

            if (textureImporter == null)
            {
                EditorUtility.DisplayDialog("Error", "Could not find the texture importer for the selected texture.", "OK");
                return;
            }

            // Ensure Read/Write is enabled
            bool originalReadWriteState = textureImporter.isReadable;
            if (!originalReadWriteState)
            {
                textureImporter.isReadable = true;
                AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
            }

            // Set texture format to RGBA32 if necessary (this format supports alpha)
            if (texture.format != TextureFormat.RGBA32)
            {
                Texture2D tempTexture = new Texture2D(texture.width, texture.height, TextureFormat.RGBA32, texture.mipmapCount > 1);
                tempTexture.SetPixels(texture.GetPixels());
                tempTexture.Apply();
                texture = tempTexture;
            }

            // Modify texture importer settings
            textureImporter.alphaIsTransparency = true;
            textureImporter.mipmapEnabled = false;
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;

            if (isBackground)
            {
                textureImporter.textureType = TextureImporterType.Sprite;  // Treat as a sprite for background
                textureImporter.spriteImportMode = SpriteImportMode.Single; // Import as a single sprite
            }
            else
            {
                textureImporter.textureType = TextureImporterType.Default; // Default type for non-background
                textureImporter.spriteImportMode = SpriteImportMode.None;  // Not treated as a sprite
            }

            // Save and reimport the texture (before transparency adjustment)
            textureImporter.SaveAndReimport();
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

            // Now, adjust the transparency directly on the texture
            Color[] pixels = texture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                Color pixel = pixels[i];
                pixel.a *= transparencyAdjustment; // Apply transparency adjustment
                pixels[i] = pixel;
            }
            texture.SetPixels(pixels);
            texture.Apply();

            // Force Unity to refresh the asset so that changes are visible
            AssetDatabase.Refresh();

            // Display success message
            EditorUtility.DisplayDialog("Success", $"Texture '{name}' has been successfully modified and saved.", "OK");
        }




        private Texture2D AdjustTextureTransparency(Texture2D originalTexture, float transparencyAdjustment)
        {
            // Ensure the transparency value is between 0 and 1
            transparencyAdjustment = Mathf.Clamp01(transparencyAdjustment);

            // Ensure the texture is readable
            UpdateTextureImporter(originalTexture);

            // Adjust transparency on each pixel
            Color[] pixels = originalTexture.GetPixels();
            for (int i = 0; i < pixels.Length; i++)
            {
                Color pixel = pixels[i];
                pixel.a *= transparencyAdjustment; // Adjust alpha
                pixels[i] = pixel;
            }

            originalTexture.SetPixels(pixels);
            originalTexture.Apply();

            // Reimport the asset after modifying it
            string texturePath = AssetDatabase.GetAssetPath(originalTexture);
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

            return originalTexture;
        }

        private void UpdateTextureImporter(Texture2D texture)
        {
            string texturePath = AssetDatabase.GetAssetPath(texture);
            TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(texturePath);

            if (!textureImporter.isReadable)
            {
                textureImporter.isReadable = true;
                AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
            }

            textureImporter.alphaIsTransparency = true; // Ensure transparency is enabled
            textureImporter.textureCompression = TextureImporterCompression.Uncompressed;
            textureImporter.SaveAndReimport();
        }






        private void DrawManual()
        {
            GUILayout.Label("Manual", EditorStyles.boldLabel);

            // Manual map
            if (GUILayout.Button("Icon Maker Pro"))
            {
                showIconMakerManual = !showIconMakerManual; // Toggle visibility of Icon Maker Pro manual
                showTextureToSpriteManual = false; // Hide Texture to Sprite manual if Icon Maker Pro is shown
            }

            if (GUILayout.Button("Texture to Sprite"))
            {
                showTextureToSpriteManual = !showTextureToSpriteManual; // Toggle visibility of Texture to Sprite manual
                showIconMakerManual = false; // Hide Icon Maker Pro manual if Texture to Sprite is shown
            }

            
            if (showIconMakerManual)
            {
                DrawIconMakerProManual();
            }

            if (showTextureToSpriteManual)
            {
                DrawTextureToSpriteManual();
            }
        }

        private void DrawIconMakerProManual()
        {


            GUILayout.Label("Icon Maker Pro Settings", new GUIStyle(EditorStyles.boldLabel) { fontSize = 18, fontStyle = FontStyle.Italic });

            GUILayout.Space(20); // Adds space of 10 units between the header and the text

            GUILayout.Label("2.1. Select Game Object", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Select or drag and drop the GameObject from your scene that you want to create an icon for.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("2.2. Icon Name", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Set a name for your icon. This name will help identify the icon after it is generated.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("2.3. Icon Size", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Choose the icon size, ranging from 32x32 to 512x512. The preview window will scale to show the selected size.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("It is recommended to use the default 512x512 size for most detailed work. This size allows you to clearly see the GameObject in the preview window without pixelation.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("When you select a smaller icon size, the GameObject in the preview window will be scaled accordingly. This ensures that the preview matches the actual icon size.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("2.4. Background Type", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Choose between Transparent or Background Color.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("For Background Color, you can select the color and shape (Square, Circle, or Smooth Corners).", EditorStyles.wordWrappedLabel);
            GUILayout.Label("Note: The background shape will not be visible in the preview window.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("2.5. Rotation and Camera Controls", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Adjust the GameObject's rotation with the XYZ sliders.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("Use Camera Zoom to zoom in or out. Rotate the light with Light Rotation and control its intensity with Light Intensity.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("Enable Overlay adds extra elements like a lock icon or a custom sprite with text on top of the icon.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("2.6. Preview Window", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("The Preview Window displays the GameObject. The icon will be generated from the exact angle shown in the window.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("The preview window will now scale according to the selected icon size (up to 512x512). This ensures a better preview of the icon as it will be shown in its actual dimensions.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("3. Generate Icon", new GUIStyle(EditorStyles.boldLabel) { fontSize = 18 });
            GUILayout.Label("Once all settings are configured, click 'Generate' to create your icon. The generated icon will match your settings.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("4. Tips & Best Practices", new GUIStyle(EditorStyles.boldLabel) { fontSize = 18 });
            GUILayout.Label("It is recommended to use the default 512x512 size for most detailed work.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("Experiment with different background shapes and lighting to achieve the best results.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("5. Conclusion", new GUIStyle(EditorStyles.boldLabel) { fontSize = 18 });
            GUILayout.Label("Icon Maker Pro is a simple, yet powerful tool for creating custom icons. Use the tips and settings described here to generate stunning icons for your game!", EditorStyles.wordWrappedLabel);


        }

        private void DrawTextureToSpriteManual()
        {
            GUILayout.Label("Texture to Sprite", new GUIStyle(EditorStyles.boldLabel) { fontSize = 18, fontStyle = FontStyle.Italic });

            GUILayout.Space(20); // Adds space of 10 units between the header and the text

            GUILayout.Label("1. Texture to Sprite Overview", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("This section focuses on converting a texture to a sprite that can be used in your game.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("2. Select Texture", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Drag and drop a texture (JPG, PNG, or Sprite) into the 'Texture to Convert' field.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("3. Set Sprite Name", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Enter a name for the sprite. This name will be used to identify the sprite after conversion.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("4. Set Save Path", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Choose where to save the converted sprite by setting the file path in the 'Save Path' field.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("The save path is pre-selected by default, but you can modify it to suit your needs.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("5. Adjust Transparency", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Use the Transparency slider to adjust the transparency of the texture. A value of 0 is fully transparent, while a value of 1 is fully opaque.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("6. Background Option", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Check the 'Background' option if you want the converted sprite to be set up as a 'Sprite (2D and UI)' with 'Sprite Mode: Single'.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("This is particularly useful for creating UI elements or backgrounds, such as main menu screens or overlays.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("When this option is enabled, the converted sprite is ready to be placed directly into your scene or used in Unity UI components.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("7. Convert Texture", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("Click 'Convert to Sprite' to generate the sprite. If a texture is not selected, an error message will appear.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("8. Tips & Best Practices", new GUIStyle(EditorStyles.boldLabel) { fontSize = 16 });
            GUILayout.Label("For best results, choose a texture that has good contrast and clear edges for transparency adjustments.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("This tool is great for converting textures like lock icons, backgrounds, and wallpapers into sprites for use as overlays.", EditorStyles.wordWrappedLabel);

            GUILayout.Label("9. Conclusion", new GUIStyle(EditorStyles.boldLabel) { fontSize = 18 });
            GUILayout.Label("Texture to Sprite is a simple tool to convert your textures into sprites ready for use in your game, including overlays for icons or backgrounds.", EditorStyles.wordWrappedLabel);
            GUILayout.Label("The 'Background' option makes it easier to prepare sprites for scenes, menus, and other UI-related tasks.", EditorStyles.wordWrappedLabel);
        }



        public static Texture2D ApplyCircleMask(Texture2D originalIcon)
        {
            Texture2D maskedIcon = new Texture2D(originalIcon.width, originalIcon.height);
            Color32[] pixels = originalIcon.GetPixels32();
            Color32[] maskedPixels = new Color32[pixels.Length];

            float centerX = originalIcon.width / 2f;
            float centerY = originalIcon.height / 2f;
            float radius = Mathf.Min(originalIcon.width, originalIcon.height) * 0.2f;

            float smoothness = 0.85f;

            for (int i = 0; i < pixels.Length; i++)
            {
                int x = i % originalIcon.width;
                int y = i / originalIcon.width;

                float distanceFromCenter = Vector2.Distance(new Vector2(x, y), new Vector2(centerX, centerY));

                if (distanceFromCenter <= radius)
                {
                    maskedPixels[i] = pixels[i];
                }
                else
                {
                    float fade = Mathf.Lerp(1f, 0f, (distanceFromCenter - radius) / (originalIcon.width * 0.5f) * smoothness);
                    Color32 pixel = pixels[i];
                    pixel.a = (byte)(pixel.a * fade);
                    maskedPixels[i] = pixel;
                }
            }

            maskedIcon.SetPixels32(maskedPixels);
            maskedIcon.Apply();

            return maskedIcon;
        }


        public static Texture2D ApplySmoothCornersMask(Texture2D originalIcon)
        {
            Texture2D maskedIcon = new Texture2D(originalIcon.width, originalIcon.height);
            Color32[] pixels = originalIcon.GetPixels32();
            Color32[] maskedPixels = new Color32[pixels.Length];

            float cornerRadius = 0.2f * Mathf.Min(originalIcon.width, originalIcon.height);
            float smoothness = 0.85f;

            for (int i = 0; i < pixels.Length; i++)
            {
                int x = i % originalIcon.width;
                int y = i / originalIcon.width;

                float distanceFromCorner = Mathf.Min(
                    Mathf.Min(x, originalIcon.width - x),
                    Mathf.Min(y, originalIcon.height - y)
                );

                if (distanceFromCorner < cornerRadius)
                {
                    float fade = Mathf.Lerp(1f, 0f, (cornerRadius - distanceFromCorner) / cornerRadius * smoothness);
                    Color32 pixel = pixels[i];
                    pixel.a = (byte)(pixel.a * fade);
                    maskedPixels[i] = pixel;
                }
                else
                {
                    maskedPixels[i] = pixels[i];
                }
            }

            maskedIcon.SetPixels32(maskedPixels);
            maskedIcon.Apply();
            return maskedIcon;
        }


        public static Texture2D ApplyRhombusMask(Texture2D originalIcon)
        {
            Texture2D maskedIcon = new Texture2D(originalIcon.width, originalIcon.height);
            Color32[] pixels = originalIcon.GetPixels32();
            Color32[] maskedPixels = new Color32[pixels.Length];

            float width = originalIcon.width;
            float height = originalIcon.height;

            float radius = 0.5f * Mathf.Min(width, height);
            float centerX = width / 2f;
            float centerY = height / 2f;
            float smoothness = 0.85f;

            for (int i = 0; i < pixels.Length; i++)
            {
                int x = i % originalIcon.width;
                int y = i / originalIcon.width;

                float relX = (x - centerX) / radius;
                float relY = (y - centerY) / radius;

                float absX = Mathf.Abs(relX);
                float absY = Mathf.Abs(relY);

                float distanceFromEdge = Mathf.Max(absX - 0.5f, absY - Mathf.Sqrt(3f) / 2f, absX + absY / Mathf.Sqrt(3f) - 0.5f);
                float fade = Mathf.Clamp01(1f - (distanceFromEdge * smoothness));
                Color32 pixel = pixels[i];
                pixel.a = (byte)(pixel.a * fade);
                maskedPixels[i] = pixel;
            }

            maskedIcon.SetPixels32(maskedPixels);
            maskedIcon.Apply();
            return maskedIcon;
        }


        public static Texture2D ApplyStarMask(Texture2D originalIcon)
        {
            Texture2D maskedIcon = new Texture2D(originalIcon.width, originalIcon.height);
            Color32[] pixels = originalIcon.GetPixels32();
            Color32[] maskedPixels = new Color32[pixels.Length];

            float radius = 0.5f * Mathf.Min(originalIcon.width, originalIcon.height);
            float smoothness = 0.85f;
            int numPoints = 5;
            float cornerRadius = 0.25f * radius;

            for (int i = 0; i < pixels.Length; i++)
            {
                int x = i % originalIcon.width;
                int y = i / originalIcon.width;

                float relX = x - originalIcon.width / 2f;
                float relY = y - originalIcon.height / 2f;
                float distanceFromCenter = Mathf.Sqrt(relX * relX + relY * relY);

                float angle = Mathf.Atan2(relY, relX);
                angle = (angle + Mathf.PI) % (Mathf.PI * 2);

                float angleStep = Mathf.PI * 2 / numPoints;

                float fade = 0f;
                for (int j = 0; j < numPoints; j++)
                {
                    float startAngle = j * angleStep;
                    float endAngle = (j + 1) * angleStep;

                    if (angle >= startAngle && angle <= endAngle)
                    {
                        fade = Mathf.Clamp01(1f - (distanceFromCenter / radius) * smoothness);
                    }
                }

                if (distanceFromCenter < radius)
                {
                    Color32 pixel = pixels[i];
                    pixel.a = (byte)(pixel.a * fade);
                    maskedPixels[i] = pixel;
                }
                else
                {
                    maskedPixels[i] = pixels[i];
                }
            }

            maskedIcon.SetPixels32(maskedPixels);
            maskedIcon.Apply();
            return maskedIcon;
        }


        public static Texture2D ApplyBoxMask(Texture2D originalIcon)
        {
            Texture2D maskedIcon = new Texture2D(originalIcon.width, originalIcon.height);
            Color32[] pixels = originalIcon.GetPixels32();
            Color32[] maskedPixels = new Color32[pixels.Length];

            int numSquares = 5;
            float smoothness = 1.5f;

            float squareSize = Mathf.Min(originalIcon.width, originalIcon.height) / (float)numSquares;

            for (int i = 0; i < pixels.Length; i++)
            {
                int x = i % originalIcon.width;
                int y = i / originalIcon.width;

                int squareIndex = Mathf.Max(Mathf.Abs(x - originalIcon.width / 2), Mathf.Abs(y - originalIcon.height / 2)) / (int)squareSize;

                float fade = Mathf.Clamp01(1f - (squareIndex / (float)numSquares) * smoothness);
                Color32 pixel = pixels[i];
                pixel.a = (byte)(pixel.a * fade);
                maskedPixels[i] = pixel;
            }

            maskedIcon.SetPixels32(maskedPixels);
            maskedIcon.Apply();
            return maskedIcon;
        }


        public static Texture2D ApplyDiamondMask(Texture2D originalIcon)
        {
            Texture2D maskedIcon = new Texture2D(originalIcon.width, originalIcon.height);
            Color32[] pixels = originalIcon.GetPixels32();
            Color32[] maskedPixels = new Color32[pixels.Length];

            float smoothness = 1.5f;
            float centerX = originalIcon.width / 2f;
            float centerY = originalIcon.height / 2f;

            for (int i = 0; i < pixels.Length; i++)
            {
                int x = i % originalIcon.width;
                int y = i / originalIcon.width;

                float absX = Mathf.Abs(x - centerX);
                float absY = Mathf.Abs(y - centerY);

                float distanceToEdge = (absX + absY) / Mathf.Min(originalIcon.width, originalIcon.height);

                float fade = Mathf.Clamp01(1f - distanceToEdge * smoothness);
                Color32 pixel = pixels[i];
                pixel.a = (byte)(pixel.a * fade);
                maskedPixels[i] = pixel;
            }

            maskedIcon.SetPixels32(maskedPixels);
            maskedIcon.Apply();
            return maskedIcon;
        }



    }
}