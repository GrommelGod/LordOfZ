using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace SMG.EzScreenshot
{
    public class EzSS_Template : ScriptableObject
    {
        // General
        private bool showTemplate = true;
        private EzSS_DataManager dataManager;
        // Reorderable List
        private ReorderableList templatesReList;

        public void Init(EzSS_DataManager dataManager)
        {
            this.dataManager = dataManager;
        }

        public void UpdateTemplatesReList()
        {
            SerializedObject _serializedData = new SerializedObject(dataManager.data);
            templatesReList = new ReorderableList(_serializedData, _serializedData.FindProperty("templates"), true, true, true, true);
            // Draws the list header
            templatesReList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, new GUIContent("Your Templates"));
            };
            // Draws the list elements
            templatesReList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty _element = templatesReList.serializedProperty.GetArrayElementAtIndex(index);
                rect.height = EditorGUIUtility.singleLineHeight;
                // Display the image that shows if the template is selected or not
                if (index == dataManager.data.index)
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, 20), EditorGUIUtility.IconContent("d_PlayButton On"));
                }
                else
                {
                    EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, 20), EditorGUIUtility.IconContent("d_PlayButton"));
                }
                // Display the template name, it's also possible to edit it
                EditorGUI.PropertyField(new Rect(rect.x + 20, rect.y + 1.5f, rect.width - 80, rect.height), _element.FindPropertyRelative("templateName"), GUIContent.none);
                // Display the load button
                EditorGUI.BeginDisabledGroup(index == dataManager.data.index);
                if (GUI.Button(new Rect(rect.width - 10, rect.y + 1.5f, 50, 15), "Load"))
                {
                    dataManager.LoadTemplate(index);
                }
                EditorGUI.EndDisabledGroup();
                // Save the changes
                _element.serializedObject.ApplyModifiedProperties();
            };

            templatesReList.onAddCallback = (ReorderableList list) =>
            {
                dataManager.NewTemplate("New template");
            };

            templatesReList.onRemoveCallback = (ReorderableList list) =>
            {
                dataManager.data.templates.RemoveAt(templatesReList.index);
                dataManager.data.index = -1;
            };

            templatesReList.onReorderCallback = (ReorderableList list) =>
            {
                dataManager.data.index = -1;
            };
        }

        public void Draw()
        {
            GUILayout.Space(5);
            EditorGUILayout.BeginHorizontal();
            showTemplate = EzSS_Style.DrawFoldoutHeader("Templates", showTemplate);
            EzSS_Style.DrawHelpButton("https://solomidgames.com/guides/ez-screenshot/templates.html");
            EditorGUILayout.EndHorizontal();
            if (showTemplate)
            {
                // To do the list indentation spaces are needed on the start and on end
                DrawTemplatesList();
            }
            EditorGUI.indentLevel++;
            DrawAutoSaveInfo();
            DrawWarnings();
            EditorGUI.indentLevel--;
        }

        private void DrawTemplatesList()
        {
            if (templatesReList == null || templatesReList.count != dataManager.data.templates.Count)
            {
                UpdateTemplatesReList();
            }
            // Draw the list with spaces on start and on end 
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUILayout.BeginVertical();
            templatesReList.DoLayoutList();
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawAutoSaveInfo()
        {
            if (dataManager.data.index >= 0)
            {
                EditorGUILayout.HelpBox(FEEDBACKS.Template.changesAreBeingSaved, MessageType.None);
            }
        }

        private void DrawWarnings()
        {
            if (dataManager.data.templates.Count <= 0)
            {
                EditorGUILayout.HelpBox(FEEDBACKS.Template.creteTemplateToContinue, MessageType.Warning);
            }
            else if (dataManager.data.index < 0)
            {
                EditorGUILayout.HelpBox(FEEDBACKS.Template.loadTemplateToContinue, MessageType.Warning);
            }
        }
    }
}