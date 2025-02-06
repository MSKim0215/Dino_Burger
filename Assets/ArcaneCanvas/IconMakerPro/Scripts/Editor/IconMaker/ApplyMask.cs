using UnityEngine;


namespace ApplyMask
{
    public class TextureMasking : MonoBehaviour
    {
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