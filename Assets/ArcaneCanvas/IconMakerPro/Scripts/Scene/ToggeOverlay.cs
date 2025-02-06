using UnityEngine;
using UnityEngine.UI;

namespace SetActive
{

    public class ImageActive : MonoBehaviour
    {
        public Image image;
        public Slider OverlayTransparency;
        public Toggle boolToggle;

        void Start()
        {

            if (boolToggle != null)
            {
                image.gameObject.SetActive(boolToggle.isOn);
            }
        }

        void Update()
        {
            // Update the image visibility whenever the Toggle changes
            if (boolToggle != null)
            {
                image.gameObject.SetActive(boolToggle.isOn);
                OverlayTransparency.gameObject.SetActive(boolToggle.isOn);
            }
        }
    }
}