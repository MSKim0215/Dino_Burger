using UnityEngine;
using TMPro;  // Import the TextMeshPro namespace
using UnityEngine.UI;  // Import the UI namespace for Image component
using System.Collections.Generic;  // Add this line to fix the List<> issue

namespace SizeChanger
{
    public class CubeAndImageSizeChanger : MonoBehaviour
    {
        public TMP_Dropdown sizeDropdown;  // Reference to the TMP_Dropdown
        public GameObject cube;  // Reference to the Cube object
        public Image picture;  // Reference to the Image UI element (for the frame)
        public Image secondPicture;  // Reference to the second Image UI element

        // Size options for the cube and pictures
        private Vector3[] sizeOptions = new Vector3[]
        {
            new Vector3(1, 1, 1), // Small size
            new Vector3(2, 2, 2), // Medium size
            new Vector3(3, 3, 3)  // Large size
        };

        void Start()
        {
            // Populate the dropdown with size options
            sizeDropdown.ClearOptions();
            sizeDropdown.AddOptions(new List<string> { "Small", "Medium", "Large" });

            // Add listener for when the dropdown value changes
            sizeDropdown.onValueChanged.AddListener(OnSizeChanged);

            // Set initial size (optional)
            OnSizeChanged(sizeDropdown.value);
        }

        // Method that will be called when the dropdown value changes
        void OnSizeChanged(int value)
        {
            if (cube != null)
            {
                // Set the cube's size based on the dropdown value
                cube.transform.localScale = sizeOptions[value];
            }

            if (picture != null)
            {
                // Scale the first image according to the selected size (adjust the scale accordingly)
                picture.transform.localScale = sizeOptions[value];
            }

            if (secondPicture != null)
            {
                // Scale the second image according to the selected size (adjust the scale accordingly)
                secondPicture.transform.localScale = sizeOptions[value];
            }
        }

        // Clean up listeners to prevent memory leaks
        private void OnDestroy()
        {
            sizeDropdown.onValueChanged.RemoveListener(OnSizeChanged);
        }
    }
}
