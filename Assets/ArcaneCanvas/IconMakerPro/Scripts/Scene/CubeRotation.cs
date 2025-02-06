using UnityEngine;
using UnityEngine.UI;

namespace CubeRotation
{
    public class CubeRotator : MonoBehaviour
    {
        public Slider xSlider; 
        public Slider ySlider; 
        public Slider zSlider; 

        private void Start()
        {
            // Add listeners to sliders
            xSlider.onValueChanged.AddListener(UpdateRotation);
            ySlider.onValueChanged.AddListener(UpdateRotation);
            zSlider.onValueChanged.AddListener(UpdateRotation);
        }

        private void UpdateRotation(float value)
        {
            // Update the rotation when sliders change
            float xRotation = xSlider.value;
            float yRotation = ySlider.value;
            float zRotation = zSlider.value;

            transform.rotation = Quaternion.Euler(xRotation, yRotation, zRotation);
        }

        private void OnDestroy()
        {
            // Remove listeners to prevent memory leaks
            xSlider.onValueChanged.RemoveListener(UpdateRotation);
            ySlider.onValueChanged.RemoveListener(UpdateRotation);
            zSlider.onValueChanged.RemoveListener(UpdateRotation);
        }
    }
}
