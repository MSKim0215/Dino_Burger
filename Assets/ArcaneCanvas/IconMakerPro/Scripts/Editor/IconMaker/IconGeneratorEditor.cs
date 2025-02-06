using UnityEditor;
using UnityEngine;
using Core;

namespace IGEditor
{
    [CustomEditor(typeof(IconGenerator))]
    public class IconGeneratorEditor : Editor
    {
        private string hoverText = string.Empty;
        private GUIStyle labelStyle;

        private void OnEnable()
        {
            labelStyle = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true,
                fontSize = 12,
                normal = { textColor = Color.white }
            };
        }

        public override void OnInspectorGUI()
        {
            IconGenerator iconGenerator = (IconGenerator)target;

            DrawDefaultInspector();

            Event e = Event.current;
            Rect iconSizeRect = GUILayoutUtility.GetLastRect();

            if (iconSizeRect.Contains(e.mousePosition))
            {
                hoverText = "The size of the icon. Choose from predefined sizes (e.g., 16, 32, 64, etc.).";
                Repaint();
            }

            if (!string.IsNullOrEmpty(hoverText))
            {
                GUIStyle popupStyle = new GUIStyle(GUI.skin.box)
                {
                    padding = new RectOffset(10, 10, 10, 10),
                    normal = { background = EditorGUIUtility.whiteTexture },
                    fontSize = 12,
                    wordWrap = true,
                    richText = true
                };

                Rect popupRect = new Rect(e.mousePosition.x + 10, e.mousePosition.y + 10, 200, 100);
                GUI.Box(popupRect, hoverText, popupStyle);
            }
        }
    }
}
