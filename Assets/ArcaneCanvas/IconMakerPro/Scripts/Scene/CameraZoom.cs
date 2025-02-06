using UnityEngine;
using UnityEngine.UI;

namespace ObjectMovement
{
    public class ObjectMovement : MonoBehaviour
    {
        public Slider zSliderMovement;  // Slider to control the Y position

        private void Start()
        {
            // Add listeners to the slider
            zSliderMovement.onValueChanged.AddListener(UpdatePosition);
        }

        private void UpdatePosition(float value)
        {
            // Update the position of the object based on the slider's value
            float zPosition = zSliderMovement.value;

            // Apply the movement on the Y-axis (you can modify which axis you want to use here)
            transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
        }

        private void OnDestroy()
        {
            // Remove listeners to prevent memory leaks
            zSliderMovement.onValueChanged.RemoveListener(UpdatePosition);
        }
    }
}
