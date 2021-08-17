using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace SMG.EzScreenshot
{
    public class EzSS_Resolutions : ScriptableObject
    {
        // General
        private EzSS_EncodeSettings encodeSettings;
        private bool showResolution = true;
        private EzSS_Mockup mockup;
        private Rect rect;
        private int maxSize = 4096;
        private int minSize = 100;
        // Screenshot
        public EzSS_AspectRatio.AspectType screenshotAspectType = EzSS_AspectRatio.AspectType.Free;
        public EzSS_AspectRatio.AspectType lastScreenshotAspectType = EzSS_AspectRatio.AspectType.Free;
        public int screenshotPortraitAspectIndex;
        public int screenshotLandscapeAspectIndex;
        public int screenshotWidth = 1280;
        public int screenshotHeight = 720;
        public string screenshotAspect;
        // Gameview
        public EzSS_AspectRatio.AspectType gameViewAspectType = EzSS_AspectRatio.AspectType.Free;
        public EzSS_AspectRatio.AspectType lastGameViewAspectType = EzSS_AspectRatio.AspectType.Free;
        public int gameViewPortraitAspectIndex;
        public int gameViewLandscapeAspectIndex;
        public int gameViewWidth = 1280;
        public int gameViewHeight = 720;
        public string gameViewAspect;
        // Mockup
        public EzSS_AspectRatio.AspectType mockupAspectType = EzSS_AspectRatio.AspectType.Free;
        public EzSS_AspectRatio.AspectType lastMockupAspectType = EzSS_AspectRatio.AspectType.Free;
        public int mockupPortraitAspectIndex;
        public int mockupLandscapeAspectIndex;
        public int mockupWidth = 1280;
        public int mockupHeight = 720;
        public int mockupScreenWidth;
        public int mockupScreenHeight;
        public string mockupAspect;
        // Aspect Setter
        public Vector2 aspectVector = Vector2.zero;
        // If mockup is enabled the name will be the mockup aspect otherwise it will be the screenshot aspect
        public static string currentAspectName;

        public void Init(EzSS_EncodeSettings encodeSettings, EzSS_Mockup mockup)
        {
            this.encodeSettings = encodeSettings;
            this.mockup = mockup;
            currentAspectName = string.Empty;
        }

        public void Draw()
        {
            EzSS_Style.DrawUILine(EzSS_Style.uiLineColor);
            EditorGUILayout.BeginHorizontal();
            showResolution = EzSS_Style.DrawFoldoutHeader("Resolutions", showResolution);
            EzSS_Style.DrawHelpButton("https://solomidgames.com/guides/ez-screenshot/resolutions.html");
            EditorGUILayout.EndHorizontal();
            if (showResolution)
            {
                EditorGUI.indentLevel++;
                if (encodeSettings.useMockup)
                {
                    DrawScreenshot();
                    EditorGUILayout.Space();
                    DrawMockup();
                    VerifyAspect(mockupAspect);
                }
                else
                {
                    DrawScreenshot();
                    EditorGUILayout.Space();
                    DrawGameView();
                    VerifyAspect(gameViewAspect);
                }
                EditorGUI.indentLevel--;
            }
        }

        private void DrawScreenshot()
        {
            screenshotAspectType = (EzSS_AspectRatio.AspectType)EditorGUILayout.EnumPopup("Screenshot Ratio:", screenshotAspectType);

            EditorGUI.indentLevel++;
            if (screenshotAspectType == EzSS_AspectRatio.AspectType.Portrait)
            {
                if (lastScreenshotAspectType != screenshotAspectType)
                {
                    screenshotHeight = screenshotWidth;
                    lastScreenshotAspectType = screenshotAspectType;
                }
                screenshotPortraitAspectIndex = EditorGUILayout.Popup("Aspect Ratio:", screenshotPortraitAspectIndex, EzSS_AspectRatio.portraitAspects);
                screenshotAspect = EzSS_AspectRatio.portraitAspects[screenshotPortraitAspectIndex];
            }
            else if (screenshotAspectType == EzSS_AspectRatio.AspectType.Landscape)
            {
                if (lastScreenshotAspectType != screenshotAspectType)
                {
                    screenshotWidth = screenshotHeight;
                    lastScreenshotAspectType = screenshotAspectType;
                }
                screenshotLandscapeAspectIndex = EditorGUILayout.Popup("Aspect Ratio:", screenshotLandscapeAspectIndex, EzSS_AspectRatio.landscapeAspects);
                screenshotAspect = EzSS_AspectRatio.landscapeAspects[screenshotLandscapeAspectIndex];
            }
            else
            {
                screenshotAspect = EzSS_AspectRatio.AspectType.Free.ToString();
            }
            Vector2 _size = SizeConstructor(screenshotAspectType, screenshotAspect, screenshotWidth, screenshotHeight, false);
            screenshotWidth = (int)_size.x;
            screenshotHeight = (int)_size.y;
            EditorGUI.indentLevel--;
        }

        private void DrawGameView()
        {
            gameViewAspectType = (EzSS_AspectRatio.AspectType)EditorGUILayout.EnumPopup("GameView Ratio:", gameViewAspectType);
            EditorGUI.indentLevel++;
            if (gameViewAspectType == EzSS_AspectRatio.AspectType.Portrait)
            {
                if (lastGameViewAspectType != gameViewAspectType)
                {
                    gameViewHeight = gameViewWidth;
                    lastGameViewAspectType = gameViewAspectType;
                }
                gameViewPortraitAspectIndex = EditorGUILayout.Popup("Aspect Ratio:", gameViewPortraitAspectIndex, EzSS_AspectRatio.portraitAspects);
                gameViewAspect = EzSS_AspectRatio.portraitAspects[gameViewPortraitAspectIndex];
            }
            else if (gameViewAspectType == EzSS_AspectRatio.AspectType.Landscape)
            {
                if (lastGameViewAspectType != gameViewAspectType)
                {
                    gameViewWidth = gameViewHeight;
                    lastGameViewAspectType = gameViewAspectType;
                }
                gameViewLandscapeAspectIndex = EditorGUILayout.Popup("Aspect Ratio:", gameViewLandscapeAspectIndex, EzSS_AspectRatio.landscapeAspects);
                gameViewAspect = EzSS_AspectRatio.landscapeAspects[gameViewLandscapeAspectIndex];
            }
            else
            {
                gameViewAspect = EzSS_AspectRatio.AspectType.Free.ToString();
            }
            Vector2 _size = SizeConstructor(gameViewAspectType, gameViewAspect, gameViewWidth, gameViewHeight, false);
            gameViewWidth = (int)_size.x;
            gameViewHeight = (int)_size.y;
            EditorGUI.indentLevel--;
        }

        private void DrawMockup()
        {
            if (mockup.selectedOrientation == EzSS_Mockup.Orientations.portrait)
                mockupAspectType = EzSS_AspectRatio.AspectType.Portrait;
            else
                mockupAspectType = EzSS_AspectRatio.AspectType.Landscape;
            EditorGUI.BeginDisabledGroup(true);
            mockupAspectType = (EzSS_AspectRatio.AspectType)EditorGUILayout.EnumPopup("Mockup Ratio:", mockupAspectType);
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel++;
            if (mockupAspectType == EzSS_AspectRatio.AspectType.Portrait)
            {
                if (lastMockupAspectType != mockupAspectType)
                {
                    mockupHeight = mockupWidth;
                    lastMockupAspectType = mockupAspectType;
                }
                for (int i = 0; i < EzSS_AspectRatio.portraitAspects.Length; i++)
                {
                    if (string.Equals(EzSS_AspectRatio.portraitAspects[i], mockup.selectedAspect))
                    {
                        mockupPortraitAspectIndex = i;
                        break;
                    }
                }
                EditorGUI.BeginDisabledGroup(true);
                mockupPortraitAspectIndex = EditorGUILayout.Popup("Aspect Ratio:", mockupPortraitAspectIndex, EzSS_AspectRatio.portraitAspects);
                EditorGUI.EndDisabledGroup();
                mockupAspect = EzSS_AspectRatio.portraitAspects[mockupPortraitAspectIndex];
            }
            else if (mockupAspectType == EzSS_AspectRatio.AspectType.Landscape)
            {
                if (lastMockupAspectType != mockupAspectType)
                {
                    mockupWidth = mockupHeight;
                    lastMockupAspectType = mockupAspectType;
                }
                for (int i = 0; i < EzSS_AspectRatio.landscapeAspects.Length; i++)
                {
                    if (string.Equals(EzSS_AspectRatio.landscapeAspects[i], mockup.selectedAspect))
                    {
                        mockupLandscapeAspectIndex = i;
                        break;
                    }
                }
                EditorGUI.BeginDisabledGroup(true);
                mockupLandscapeAspectIndex = EditorGUILayout.Popup("Aspect Ratio:", mockupLandscapeAspectIndex, EzSS_AspectRatio.landscapeAspects);
                EditorGUI.EndDisabledGroup();
                mockupAspect = EzSS_AspectRatio.landscapeAspects[mockupLandscapeAspectIndex];
            }

            Vector2 _size = SizeConstructor(mockupAspectType, mockupAspect, mockupWidth, mockupHeight, true);
            mockupWidth = (int)_size.x;
            mockupHeight = (int)_size.y;

            // Set the screenshot width and height based on the mockup width and height
            // If the mockup orientation is on landscape, the width and height are inverted
            if (mockupAspectType == EzSS_AspectRatio.AspectType.Landscape)
            {
                mockupScreenWidth = Mathf.RoundToInt((float)(mockupWidth * mockup.selectedScreenSize.y) / 100);
                mockupScreenHeight = Mathf.RoundToInt((float)(mockupHeight * mockup.selectedScreenSize.x) / 100);
            }
            else
            {
                mockupScreenWidth = Mathf.RoundToInt(((float)mockupWidth * mockup.selectedScreenSize.x) / 100);
                mockupScreenHeight = Mathf.RoundToInt(((float)mockupHeight * mockup.selectedScreenSize.y) / 100);
            }
            EditorGUI.indentLevel--;
        }

        private Vector2 SizeConstructor(EzSS_AspectRatio.AspectType aspectType, string aspectRatio, int width, int height, bool isMockup)
        {
            int _width = width;
            int _height = height;

            if (aspectType == EzSS_AspectRatio.AspectType.Portrait)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Width:");
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.TextArea(_width.ToString());
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Height:");
                GUILayout.Space(-30);
                _height = EditorGUILayout.IntSlider(_height, minSize, maxSize);
                EditorGUILayout.EndHorizontal();

                if (isMockup && encodeSettings.useMockup && mockup.mockupTexture != null)
                    _width = Mathf.RoundToInt(_height / (mockup.selectedTextureSize.y / mockup.selectedTextureSize.x));
                else
                    _width = Mathf.RoundToInt((float)_height * EzSS_AspectRatio.AspectRatioResults[aspectRatio]);
            }
            else if (aspectType == EzSS_AspectRatio.AspectType.Landscape)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Width:");
                GUILayout.Space(-30);
                _width = EditorGUILayout.IntSlider(_width, minSize, maxSize);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Height:");
                EditorGUI.BeginDisabledGroup(true);
                GUILayout.TextArea(_height.ToString());
                EditorGUI.EndDisabledGroup();
                EditorGUILayout.EndHorizontal();

                if (isMockup && encodeSettings.useMockup && mockup.mockupTexture != null)
                {
                    // This is important when the width is bigger than the height on portrait only mockups
                    if (!mockup.selectedMultiOriented && EzSS_AspectRatio.AspectRatioResults[mockup.selectedAspect] > 0)
                    {
                        _height = Mathf.RoundToInt(_width / (mockup.selectedTextureSize.x / mockup.selectedTextureSize.y));
                    }
                    else
                    {
                        _height = Mathf.RoundToInt(_width / (mockup.selectedTextureSize.y / mockup.selectedTextureSize.x));
                    }
                }
                else
                    _height = Mathf.RoundToInt(_width / EzSS_AspectRatio.AspectRatioResults[aspectRatio]);
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Width:");
                GUILayout.Space(-30);
                _width = EditorGUILayout.IntSlider(_width, minSize, maxSize);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel("Height:");
                GUILayout.Space(-30);
                _height = EditorGUILayout.IntSlider(_height, minSize, maxSize);
                EditorGUILayout.EndHorizontal();
            }

            return new Vector2(_width, _height);
        }

        private void VerifyAspect(string aspect)
        {
            string _newAspectName = "";
            aspectVector = Vector2.zero;
            if (EzSS_AspectRatio.AspectsVectors.ContainsKey(aspect))
                aspectVector = EzSS_AspectRatio.AspectsVectors[aspect];
            // Set the name of the new aspect
            if (Equals(aspectVector, Vector2.zero))
                _newAspectName = "";
            else
                _newAspectName = aspectVector.x + ":" + aspectVector.y;

            // If the current name and the new one are different, change the aspect of the game view
            if (!string.Equals(currentAspectName, _newAspectName))
            {
                EzSS_AspectSetter.SetAspectRatio(EzSS_AspectSetter.GameViewSizeType.AspectRatio, (int)aspectVector.x, (int)aspectVector.y, _newAspectName);

                // Reset all the cameras to update the aspect
                Camera[] _tempCameras = FindObjectsOfType(typeof(Camera)) as Camera[];
                for (int i = 0; i < _tempCameras.Length; i++)
                {
                    _tempCameras[i].enabled = false;
                    _tempCameras[i].enabled = true;
                }
            }
        }
    }
}