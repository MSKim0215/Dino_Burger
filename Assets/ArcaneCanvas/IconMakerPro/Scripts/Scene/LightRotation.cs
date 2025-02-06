using UnityEngine;
using UnityEngine.UI;

namespace LightRotation
{
    public class LightRotation : MonoBehaviour
    {
        
        public Slider ySliderLight;
        

        private void Start()
        {
            // Add listeners to sliders
            
            ySliderLight.onValueChanged.AddListener(UpdateRotation);
            
        }

        private void UpdateRotation(float value)
        {
            // Update the rotation when sliders change
            
            float yRotation = ySliderLight.value;
            

            transform.rotation = Quaternion.Euler(0, yRotation,0);
        }

        private void OnDestroy()
        {
            // Remove listeners to prevent memory leaks
            
            ySliderLight.onValueChanged.RemoveListener(UpdateRotation);
            
        }
    }
}
