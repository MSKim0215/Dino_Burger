using UnityEngine;
using UnityEngine.UI;

namespace LightIntensity
{
    public class LightIntensityController : MonoBehaviour
    {
        public Slider intensitySlider; // Assign the slider in the Inspector
        public Light directionalLight; // Assign the directional light in the Inspector

        private void Start()
        {
            // Add listener to the slider
            if (intensitySlider != null && directionalLight != null)
            {
                intensitySlider.onValueChanged.AddListener(AdjustLightIntensity);
            }
        }

        private void AdjustLightIntensity(float intensity)
        {
            // Adjust the intensity of the directional light
            directionalLight.intensity = intensity;
        }

        private void OnDestroy()
        {
            // Remove listener to avoid memory leaks
            if (intensitySlider != null)
            {
                intensitySlider.onValueChanged.RemoveListener(AdjustLightIntensity);
            }
        }
    }
}
