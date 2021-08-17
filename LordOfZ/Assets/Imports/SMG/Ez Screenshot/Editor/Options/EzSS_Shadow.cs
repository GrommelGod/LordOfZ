using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace SMG.EzScreenshot
{
    public class EzSS_Shadow : ScriptableObject
    {
        // General
        private EzSS_EncodeSettings encodeSettings;
        private bool showShadow = true;
        // Shadow
        public Color color = new Color32(0, 0, 0, 75);
        public Vector2 direction = new Vector2(-2, -3);
        public int softness = 10;

        public void Init(EzSS_EncodeSettings encodeSettings)
        {
            this.encodeSettings = encodeSettings;
        }

        public void Draw()
        {
            if (encodeSettings.useShadow)
            {
                Rect _rect = EditorGUILayout.BeginVertical();
                EzSS_Style.DrawUILine(EzSS_Style.uiLineColor);
                EditorGUILayout.BeginHorizontal();
                showShadow = EzSS_Style.DrawFoldoutHeader("Shadow", showShadow);
                EzSS_Style.DrawHelpButton("https://solomidgames.com/guides/ez-screenshot/shadow.html");
                EditorGUILayout.EndHorizontal();
                if (showShadow)
                {
                    EditorGUI.indentLevel++;
                    DrawShadow();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();
                EditorGUI.DrawRect(new Rect(_rect.x - 3, _rect.y + 7, 2f, _rect.height), new Color32(46, 113, 173, 255));
            }
        }

        private void DrawShadow()
        {
            color = EditorGUILayout.ColorField("Color:", color);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Direction:");
            GUILayout.Space(-15);
            direction = EditorGUILayout.Vector2Field(string.Empty, direction);
            EditorGUILayout.EndHorizontal();
            softness = EditorGUILayout.IntSlider("Softness:", softness, 0, 50);
        }
    }
}