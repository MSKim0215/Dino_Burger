using UnityEngine;

namespace IconMakerPro.Utils
{
    public static class IconUtils
    {
        public static int GetIconSize(int selectedSizeIndex)
        {
            switch (selectedSizeIndex)
            {
                
                case 0: return 32;
                case 1: return 64;
                case 2: return 128;
                case 3: return 256;
                case 4: return 512;
                default: return 512;
            }
        }

        public static Bounds GetObjectBounds(GameObject obj)
        {
            Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();

            if (renderers.Length == 0)
            {
                return new Bounds(obj.transform.position, Vector3.zero);
            }

            Bounds bounds = renderers[0].bounds;

            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            return bounds;
        }
    }
}
