using UnityEngine;
using UnityEngine.UI;

namespace Alpha
{

    public class ImageAlphaControl : MonoBehaviour
    {
        public Image image;  // Reference to the image
        public Slider alphaSlider;  // Reference to the slider that controls the alpha
        public float alphaValue = 1f;  // Default alpha value (1 is fully opaque, 0 is fully transparent)

        void Start()
        {
            // Ensure the slider's value is set to the current alpha value at the start
            if (alphaSlider != null)
            {
                alphaSlider.value = alphaValue;
                alphaSlider.onValueChanged.AddListener(UpdateAlphaFromSlider);
            }
        }

        void Update()
        {
            // Ensure the alpha value is clamped between 0 and 1
            alphaValue = Mathf.Clamp01(alphaValue);

            // Get the current color of the image
            Color currentColor = image.color;

            // Set the new color with adjusted alpha
            image.color = new Color(currentColor.r, currentColor.g, currentColor.b, alphaValue);
        }

        // Method that is called when the slider value changes
        void UpdateAlphaFromSlider(float value)
        {
            alphaValue = value;  // Update alpha value based on the slider value
        }

        // Optionally, you can create a method to change alpha value dynamically
        public void SetAlpha(float newAlpha)
        {
            alphaValue = Mathf.Clamp01(newAlpha);
            alphaSlider.value = alphaValue;  // Sync the slider value with the alpha
        }
    }
}