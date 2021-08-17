using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditorInternal;

namespace SMG.EzScreenshot
{
    [Serializable]
    public class BgColorProperties
    {
        public Color color = Color.white;
        [Range(0, 1)]
        public float time;
    }

    public class EzSS_Background : ScriptableObject
    {
        // General
        private EzSS_EncodeSettings encodeSettings;
        private bool showBackground = true;
        // Reorderable List
        private UnityEngine.Object target;
        SerializedObject serializedObject;
        private SerializedProperty serializedProperty;
        private ReorderableList bgColorsReList;
        public List<BgColorProperties> bgColors = new List<BgColorProperties>();
        // Mixing Type
        public enum BgTypes
        {
            solidVertical,
            solidHorizontal,
            gradientVertical,
            gradientHorizontal
        }
        public BgTypes bgType = BgTypes.solidVertical;
        // Preview
        private bool updatePreview = false;
        private Rect windowRect;
        private Texture2D backgroundTexturePreview;

        public void Init(EzSS_EncodeSettings encodeSettings)
        {
            this.encodeSettings = encodeSettings;
        }

        public void UpdateBgColorsReList()
        {
            serializedObject = new SerializedObject(this);
            bgColorsReList = new ReorderableList(serializedObject, serializedObject.FindProperty("bgColors"), true, true, true, true);
            bgColorsReList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, new GUIContent("Colors"));
            };
            // Draw
            bgColorsReList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                GUIContent _content = new GUIContent(string.Empty);
                // Properties
                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width - 150, rect.height), bgColorsReList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("color"), _content);
                EditorGUI.PropertyField(new Rect(rect.width - 100, rect.y, 135, rect.height), bgColorsReList.serializedProperty.GetArrayElementAtIndex(index).FindPropertyRelative("time"), _content);
                // Update
                updatePreview = true;
                if (serializedObject.targetObject != null)
                {
                    serializedObject.ApplyModifiedProperties();
                }
            };
            // Add
            bgColorsReList.onAddCallback = (ReorderableList list) =>
            {
                bgColors.Add(new BgColorProperties());
                updatePreview = true;
            };
            // Remove
            bgColorsReList.onRemoveCallback = (ReorderableList list) =>
            {
                bgColors.RemoveAt(bgColorsReList.index);
                updatePreview = true;
            };
        }

        public void Draw(Rect windowRect)
        {
            if (encodeSettings.useBackground)
            {
                this.windowRect = windowRect;
                Rect _rect = EditorGUILayout.BeginVertical();
                EzSS_Style.DrawUILine(EzSS_Style.uiLineColor);
                EditorGUILayout.BeginHorizontal();
                showBackground = EzSS_Style.DrawFoldoutHeader("Background", showBackground);
                EzSS_Style.DrawHelpButton("https://solomidgames.com/guides/ez-screenshot/background.html");
                EditorGUILayout.EndHorizontal();
                if (showBackground)
                {
                    DrawBgColorsReList();
                    EditorGUI.indentLevel++;
                    DrawBgType();
                    DrawPreview();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();
                // Draws the colored line
                if (!showBackground)
                {
                    EditorGUI.DrawRect(new Rect(_rect.x - 3, _rect.y + 7, 2f, _rect.height), new Color32(242, 162, 64, 255));
                }
                else
                {
                    EditorGUI.DrawRect(new Rect(_rect.x + 1, _rect.y + 7, 2f, _rect.height), new Color32(242, 162, 64, 255));
                }
            }
        }

        private void DrawBgColorsReList()
        {
            if (bgColorsReList == null || bgColorsReList.count != bgColors.Count)
            {
                UpdateBgColorsReList();
            }
            // Draw the list with spaces on start and on end 
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();
            bgColorsReList.DoLayoutList();
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawBgType()
        {
            EditorGUI.BeginDisabledGroup(bgColors.Count <= 1);
            bgType = (BgTypes)EditorGUILayout.EnumPopup("Type:", bgType);
            EditorGUI.EndDisabledGroup();
        }

        private void DrawPreview()
        {
            if (updatePreview)
            {
                updatePreview = false;
                backgroundTexturePreview = EzSS_TextureCreator.Background(this, (int)windowRect.width - 45, 75);
            }
            if (bgColors.Count > 0)
            {
                EditorGUILayout.LabelField("Preview:");
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                GUILayout.Label(backgroundTexturePreview, GUILayout.Width(windowRect.width - 45));
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}