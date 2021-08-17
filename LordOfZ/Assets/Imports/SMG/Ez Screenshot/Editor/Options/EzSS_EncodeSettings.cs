using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditorInternal;

namespace SMG.EzScreenshot
{
    public class EzSS_EncodeSettings : ScriptableObject
    {
        // General
        private SerializedProperty property;
        private bool showConfiguration = true;
        // Camera
        SerializedObject serializedObject;
        SerializedProperty element;
        public ReorderableList camerasReList;
        public List<Camera> cameras = new List<Camera>();
        public bool setCamerasManually = true;
        // Encode
        public enum EncodeType
        {
            PNG,
            JPG
        };
        public EncodeType encodeType = EncodeType.PNG;
        public int jpgQuality = 75;
        // Name
        public string namePrefix = "screenshot";
        public bool useDate = false;
        public bool useTime = false;
        public string nameFinal = "";
        // Save at
        public string saveAtPath = "";
        // Add-ons
        public bool useMockup = false;
        public bool useShadow = false;
        public bool useBackground = false;

        public void UpdateCamerasReList()
        {
            serializedObject = new SerializedObject(this);
            camerasReList = new ReorderableList(serializedObject, serializedObject.FindProperty("cameras"), true, true, true, true);
            camerasReList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, new GUIContent("Cameras"));
            };

            camerasReList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                rect.height = EditorGUIUtility.singleLineHeight;
                GUIContent _content = new GUIContent(string.Format("Camera {0}", index));
                EditorGUI.PropertyField(rect, camerasReList.serializedProperty.GetArrayElementAtIndex(index), _content);
                if (serializedObject.targetObject != null)
                {
                    serializedObject.ApplyModifiedProperties();
                }
            };

            camerasReList.onAddCallback = (ReorderableList list) =>
            {
                cameras.Add(new Camera());
            };

            camerasReList.onRemoveCallback = (ReorderableList list) =>
            {
                cameras.RemoveAt(camerasReList.index);
            };
        }

        public void Draw()
        {
            EzSS_Style.DrawUILine(EzSS_Style.uiLineColor);
            EditorGUILayout.BeginHorizontal();
            showConfiguration = EzSS_Style.DrawFoldoutHeader("Encode Settings", showConfiguration);
            EzSS_Style.DrawHelpButton("https://solomidgames.com/guides/ez-screenshot/encode-settings.html");
            EditorGUILayout.EndHorizontal();
            if (showConfiguration)
            {
                // TakeWithScreenCapture();   
                // To do the list indentation spaces are needed on the start and on end
                DrawCameraConfig();
                EditorGUI.indentLevel++;
                DrawEncodeConfig();
                DrawNameConfig();
                GUILayout.Space(2);
                DrawSaveAtConfig();
                DrawAddonsConfig();
                EditorGUI.indentLevel--;
            }
        }

        private void TakeWithScreenCapture()
        {
            EditorGUI.indentLevel++;
            setCamerasManually = EditorGUILayout.Toggle("Set Cameras Manually:", setCamerasManually);
            if (!setCamerasManually)
            {
                EditorGUILayout.HelpBox(FEEDBACKS.Configuration.screenCaptureWarning, MessageType.None);
            }
            else
            {
                EditorGUILayout.HelpBox(FEEDBACKS.Configuration.screenSpaceOverlayWarning, MessageType.None);
            }
            EditorGUI.indentLevel--;
        }

        private void DrawCameraConfig()
        {
            if (setCamerasManually)
            {
                if (camerasReList == null || camerasReList.count != cameras.Count)
                {
                    UpdateCamerasReList();
                }
                // Draw the list with spaces on start and on end 
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(20);
                EditorGUILayout.BeginVertical();
                camerasReList.DoLayoutList();
                EditorGUILayout.EndVertical();
                GUILayout.Space(5);
                EditorGUILayout.EndHorizontal();
            }
        }

        private static string GetGameObjectPath(GameObject obj)
        {
            string path = "/" + obj.name;
            while (obj.transform.parent != null)
            {
                obj = obj.transform.parent.gameObject;
                path = "/" + obj.name + path;
            }
            return path;
        }

        private void DrawEncodeConfig()
        {
            encodeType = (EncodeType)EditorGUILayout.EnumPopup("Encode Type:", encodeType);
            if (encodeType == EncodeType.JPG)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Quality:");
                GUILayout.Space(-30);
                jpgQuality = EditorGUILayout.IntSlider(jpgQuality, 1, 100);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }
        }

        private void DrawNameConfig()
        {
            namePrefix = EditorGUILayout.TextField("Name:", namePrefix);
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Use Date:");
            GUILayout.Space(-30);
            useDate = EditorGUILayout.Toggle(useDate);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Use Time:");
            GUILayout.Space(-30);
            useTime = EditorGUILayout.Toggle(useTime);
            EditorGUILayout.EndHorizontal();
            // Cofigurates the name and display it
            nameFinal = namePrefix;
            if (useDate)
                nameFinal = nameFinal + "_" + DateTime.Now.ToString("yy-MMM-dd");
            if (useTime)
                nameFinal = nameFinal + "-" + DateTime.Now.ToString("HH-mm-ss");
            EditorGUI.indentLevel--;
            // Name Preview
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Name Preview:", nameFinal);
            EditorGUI.EndDisabledGroup();
        }

        private void DrawSaveAtConfig()
        {
            // Check if the path still exists
            if (!string.IsNullOrEmpty(saveAtPath) && !System.IO.Directory.Exists(saveAtPath))
            {
                saveAtPath = string.Empty;
            }
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(" ");
            GUILayout.Space(-15);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.BeginVertical();
            GUILayout.Space(-1);
            // Location Text Area
            if (string.IsNullOrEmpty(saveAtPath))
            {
                EditorGUILayout.TextField(FEEDBACKS.Configuration.mustBrowseLocation);
            }
            else
            {
                EditorGUILayout.TextField(saveAtPath);
            }
            EditorGUILayout.EndVertical();
            EditorGUI.EndDisabledGroup();
            GUILayout.Space(-10);
            EzSS_Style.DrawFindButton(delegate
            {
                saveAtPath = EditorUtility.SaveFolderPanel("Save At", saveAtPath, Application.dataPath);
            });
            EditorGUILayout.EndHorizontal();
            // Draw the Prefix Label
            GUILayout.Space(-23);
            EditorGUILayout.LabelField("Save At:");
        }

        private void DrawAddonsConfig()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Add-ons:");
            useMockup = GUILayout.Toggle(useMockup, "Mockup", "ButtonLeft", GUILayout.Height(EzSS_Style.mediumBtnHeight));
            useShadow = GUILayout.Toggle(useShadow, "Shadow", "ButtonMid", GUILayout.Height(EzSS_Style.mediumBtnHeight));
            useBackground = GUILayout.Toggle(useBackground, "Background", "ButtonRight", GUILayout.Height(EzSS_Style.mediumBtnHeight));
            EditorGUILayout.EndHorizontal();
        }
    }
}