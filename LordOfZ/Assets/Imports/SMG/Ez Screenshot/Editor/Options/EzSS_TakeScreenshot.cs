using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace SMG.EzScreenshot
{
    public class EzSS_TakeScreenshot : ScriptableObject
    {
        // General
        private EzSS_EncodeSettings encodeSettings;
        private EzSS_Resolutions resolutions;
        private EzSS_Mockup mockup;
        private EzSS_Shadow shadow;
        private EzSS_Background background;
        private bool showTakeScreenshot = true;
        // Textures
        private Texture2D backgroundTexture;
        private Texture2D transparentTexture;
        private Texture2D gameViewTexture;
        private Texture2D mockupTexture;
        private Texture2D mockupScreenTexture;
        private Texture2D shadowTexture;
        private Texture2D textureToBeEncoded;
        // Warning
        private List<string> warningMessages = new List<string>();

        public bool takingScreenshot = false;

        public void Init(EzSS_EncodeSettings encodeSettings, EzSS_Resolutions resolutions, EzSS_Mockup mockup, EzSS_Shadow shadow, EzSS_Background background)
        {
            this.encodeSettings = encodeSettings;
            this.resolutions = resolutions;
            this.mockup = mockup;
            this.shadow = shadow;
            this.background = background;
        }

        public void Draw(Rect window)
        {
            Color _bgColor = EditorGUIUtility.isProSkin ? new Color32(36, 36, 36, 255) : new Color32(152, 152, 152, 255);
            Color _skinColor = EditorGUIUtility.isProSkin ? new Color32(56, 56, 56, 255) : new Color32(194, 194, 194, 255);
            // Begins the background area
            Rect _bgRect = EditorGUILayout.BeginVertical();
            GUILayout.Space(10);
            EditorGUI.DrawRect(new Rect(_bgRect.x, _bgRect.y, window.width, _bgRect.height), _bgColor);
            // Begins the content area 
            EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(new Rect(_bgRect.x, _bgRect.y + 5, window.width, _bgRect.height), _skinColor);
            // Content goes over here
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(5);
            EzSS_Style.DrawHeader("Take Screenshot");
            EzSS_Style.DrawHelpButton("https://solomidgames.com/guides/ez-screenshot/take-screenshot.html");
            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();
            if (showTakeScreenshot)
            {
                EditorGUI.indentLevel++;
                DrawWarnings();
                DrawButtons();
                EditorGUI.indentLevel--;
            }
            GUILayout.Space(20);
            // Closes
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndVertical();
        }

        private void DrawWarnings()
        {
            CheckWarnings();
            if (warningMessages.Count > 0)
            {
                for (int i = 0; i < warningMessages.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox(warningMessages[i], MessageType.Warning);
                    GUILayout.Space(5);
                    EditorGUILayout.EndHorizontal();
                }
            }
        }

        private void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            EditorGUI.BeginDisabledGroup(mockup.editorUpdateIsRunning);
            if (GUILayout.Button("Take Screenshot", "ButtonLeft", GUILayout.Height(EzSS_Style.bigBtnHeight), GUILayout.MinWidth(260), GUILayout.Height(EzSS_Style.bigBtnHeight)))
            {
                TakeScreenshot(true);
            }
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Show", "ButtonRight", GUILayout.Height(EzSS_Style.bigBtnHeight)))
            {
                Application.OpenURL("file://" + encodeSettings.saveAtPath);
            }
            GUILayout.Space(5);
            EditorGUILayout.EndHorizontal();
        }

        public void TakeScreenshot(bool exitGUI)
        {
            // Has warnings?
            if (warningMessages.Count > 0)
            {
                string _dialogMessage = string.Empty;
                for (int i = 0; i < warningMessages.Count; i++)
                {
                    _dialogMessage += "- " + warningMessages[i] + "\n";
                }
                EditorUtility.DisplayDialog(FEEDBACKS.Titles.attention, _dialogMessage, FEEDBACKS.Buttons.close);
                return;
            }
            // Verifies if there's a null value inside the list and remove it
            int _i = 0;
            while (_i < encodeSettings.cameras.Count)
            {
                if (encodeSettings.cameras[_i] == null)
                {
                    encodeSettings.cameras.RemoveAt(_i);
                    _i = 0;
                }
                else
                    _i++;
            }
            // Take the screenshot
            backgroundTexture = null;
            transparentTexture = null;
            gameViewTexture = null;
            mockupTexture = null;
            shadowTexture = null;
            textureToBeEncoded = null;

            EditorUtility.DisplayProgressBar(FEEDBACKS.Titles.wait, FEEDBACKS.TakeScreenshot.takingScreenshot, 0);
            takingScreenshot = true;
            EditorCoroutine.start(CreateTextures(exitGUI));
        }

        private IEnumerator CreateTextures(bool exitGUI)
        {
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            yield return new WaitForEndOfFrame();
            // Gameview
            if (!encodeSettings.useBackground && !encodeSettings.useMockup && !encodeSettings.useShadow)
            {
                // Creates the gameView texture
                gameViewTexture = EzSS_TextureCreator.GameView(encodeSettings, resolutions.gameViewWidth, resolutions.gameViewHeight);
                // Creates the transparent texture
                transparentTexture = EzSS_TextureCreator.Transparent(resolutions.screenshotWidth, resolutions.screenshotHeight);
                // Combine transparent and gameview
                textureToBeEncoded = EzSS_TextureCombinator.Simple(transparentTexture, gameViewTexture, false);
            }
            // GameView + Shadow
            else if (!encodeSettings.useBackground && !encodeSettings.useMockup && encodeSettings.useShadow)
            {
                // Creates the gameView texture
                gameViewTexture = EzSS_TextureCreator.GameView(encodeSettings, resolutions.gameViewWidth, resolutions.gameViewHeight);
                // Creates the shadow texture
                shadowTexture = EzSS_TextureCreator.Shadow(shadow, gameViewTexture);
                // Creates the transparent texture based on the screenshot size
                transparentTexture = EzSS_TextureCreator.Transparent(resolutions.screenshotWidth, resolutions.screenshotHeight);
                // Combine transparent and shadow
                textureToBeEncoded = EzSS_TextureCombinator.Shadow(transparentTexture, shadowTexture, shadow);
                // Combine the result and gameView
                textureToBeEncoded = EzSS_TextureCombinator.Simple(textureToBeEncoded, gameViewTexture, true);
            }
            // GameView + Background
            else if (encodeSettings.useBackground && !encodeSettings.useMockup && !encodeSettings.useShadow)
            {
                // Creates the gameView texture
                gameViewTexture = EzSS_TextureCreator.GameView(encodeSettings, resolutions.gameViewWidth, resolutions.gameViewHeight);
                // Creates the background texture
                backgroundTexture = EzSS_TextureCreator.Background(background, resolutions.screenshotWidth, resolutions.screenshotHeight);
                // Combine background and gameView
                textureToBeEncoded = EzSS_TextureCombinator.Simple(backgroundTexture, gameViewTexture, false);
            }
            // GameView + Shadow + Background
            else if (encodeSettings.useBackground && !encodeSettings.useMockup && encodeSettings.useShadow)
            {
                // Creates the gameView texture
                gameViewTexture = EzSS_TextureCreator.GameView(encodeSettings, resolutions.gameViewWidth, resolutions.gameViewHeight);
                // Creates the shadow texture
                shadowTexture = EzSS_TextureCreator.Shadow(shadow, gameViewTexture);
                // Creates the background texture
                backgroundTexture = EzSS_TextureCreator.Background(background, resolutions.screenshotWidth, resolutions.screenshotHeight);
                // Combines background and shadow
                textureToBeEncoded = EzSS_TextureCombinator.Shadow(backgroundTexture, shadowTexture, shadow);
                // Combines the previous result with gameView
                textureToBeEncoded = EzSS_TextureCombinator.Simple(textureToBeEncoded, gameViewTexture, false);
            }
            // Mockup
            else if (!encodeSettings.useBackground && encodeSettings.useMockup && !encodeSettings.useShadow)
            {
                // Creates the gameView texture
                gameViewTexture = EzSS_TextureCreator.GameView(encodeSettings, resolutions.mockupScreenWidth, resolutions.mockupScreenHeight);
                // Creates the mockup texture
                mockupTexture = EzSS_TextureCreator.Mockup(mockup, resolutions.mockupWidth, resolutions.mockupHeight);
                // Creates the mockup screen texture
                mockupScreenTexture = EzSS_TextureCreator.MockupScreen(mockup, resolutions.mockupWidth, resolutions.mockupHeight);
                // Creates the transparent texture based on the screenshot size
                transparentTexture = EzSS_TextureCreator.Transparent(resolutions.screenshotWidth, resolutions.screenshotHeight);
                // Combines gameView and mockup screen
                Texture2D _mockupScreenAndGameView = EzSS_TextureCombinator.GameViewAndMockupScreen(mockupScreenTexture, gameViewTexture, mockup, resolutions);
                // Combines _mockupScreenAndGameView and mockup
                textureToBeEncoded = EzSS_TextureCombinator.MockupAndMockupScreen(_mockupScreenAndGameView, mockupTexture);
                // Final image
                textureToBeEncoded = EzSS_TextureCombinator.Simple(transparentTexture, textureToBeEncoded, false, mockup.mockupOffset);
            }
            // Mockup + Background
            else if (encodeSettings.useBackground && encodeSettings.useMockup && !encodeSettings.useShadow)
            {
                // Creates the gameView texture
                gameViewTexture = EzSS_TextureCreator.GameView(encodeSettings, resolutions.mockupScreenWidth, resolutions.mockupScreenHeight);
                // Creates the mockup texture
                mockupTexture = EzSS_TextureCreator.Mockup(mockup, resolutions.mockupWidth, resolutions.mockupHeight);
                // Creates the mockup screen texture
                mockupScreenTexture = EzSS_TextureCreator.MockupScreen(mockup, resolutions.mockupWidth, resolutions.mockupHeight);
                // Creates the background texture
                backgroundTexture = EzSS_TextureCreator.Background(background, resolutions.screenshotWidth, resolutions.screenshotHeight);
                // Combines gameView and mockup screen
                Texture2D _mockupScreenAndGameView = EzSS_TextureCombinator.GameViewAndMockupScreen(mockupScreenTexture, gameViewTexture, mockup, resolutions);
                // Combines _mockupScreenAndGameView and mockup
                textureToBeEncoded = EzSS_TextureCombinator.MockupAndMockupScreen(_mockupScreenAndGameView, mockupTexture);
                // Final image
                textureToBeEncoded = EzSS_TextureCombinator.Simple(backgroundTexture, textureToBeEncoded, false, mockup.mockupOffset);
            }
            // Mockup + Shadow
            else if (!encodeSettings.useBackground && encodeSettings.useMockup && encodeSettings.useShadow)
            {
                // Creates the gameView texture
                gameViewTexture = EzSS_TextureCreator.GameView(encodeSettings, resolutions.mockupScreenWidth, resolutions.mockupScreenHeight);
                // Creates the mockup texture
                mockupTexture = EzSS_TextureCreator.Mockup(mockup, resolutions.mockupWidth, resolutions.mockupHeight);
                // Creates the mockup screen texture
                mockupScreenTexture = EzSS_TextureCreator.MockupScreen(mockup, resolutions.mockupWidth, resolutions.mockupHeight);
                // Combines gameView and mockup screen
                Texture2D _mockupScreenAndGameView = EzSS_TextureCombinator.GameViewAndMockupScreen(mockupScreenTexture, gameViewTexture, mockup, resolutions);
                // Combines _mockupScreenAndGameView and mockup
                _mockupScreenAndGameView = EzSS_TextureCombinator.MockupAndMockupScreen(_mockupScreenAndGameView, mockupTexture);
                // Creates the shadow
                shadowTexture = EzSS_TextureCreator.Shadow(shadow, _mockupScreenAndGameView);
                // Creates the transparent texture
                transparentTexture = EzSS_TextureCreator.Transparent(resolutions.screenshotWidth, resolutions.screenshotHeight);
                // Combines the transparent and shadow
                textureToBeEncoded = EzSS_TextureCombinator.Shadow(transparentTexture, shadowTexture, shadow, mockup.mockupOffset);
                // Combines the result 
                textureToBeEncoded = EzSS_TextureCombinator.Simple(textureToBeEncoded, _mockupScreenAndGameView, false, mockup.mockupOffset);
            }
            // Mockup + Background + Shadow
            else if (encodeSettings.useBackground && encodeSettings.useMockup && encodeSettings.useShadow)
            {
                // Creates the gameView texture
                gameViewTexture = EzSS_TextureCreator.GameView(encodeSettings, resolutions.mockupScreenWidth, resolutions.mockupScreenHeight);
                // Creates the mockup texture
                mockupTexture = EzSS_TextureCreator.Mockup(mockup, resolutions.mockupWidth, resolutions.mockupHeight);
                // Creates the mockup screen texture
                mockupScreenTexture = EzSS_TextureCreator.MockupScreen(mockup, resolutions.mockupWidth, resolutions.mockupHeight);
                // Combines gameView and mockup screen
                Texture2D _mockupScreenAndGameView = EzSS_TextureCombinator.GameViewAndMockupScreen(mockupScreenTexture, gameViewTexture, mockup, resolutions);
                // Combines _mockupScreenAndGameView and mockup
                _mockupScreenAndGameView = EzSS_TextureCombinator.MockupAndMockupScreen(_mockupScreenAndGameView, mockupTexture);
                // Creates the shadow
                shadowTexture = EzSS_TextureCreator.Shadow(shadow, _mockupScreenAndGameView);
                // Creates the background texture
                backgroundTexture = EzSS_TextureCreator.Background(background, resolutions.screenshotWidth, resolutions.screenshotHeight);
                // Combines the background and shadow
                textureToBeEncoded = EzSS_TextureCombinator.Shadow(backgroundTexture, shadowTexture, shadow, mockup.mockupOffset);
                // Combines the result 
                textureToBeEncoded = EzSS_TextureCombinator.Simple(textureToBeEncoded, _mockupScreenAndGameView, false, mockup.mockupOffset);
            }

            Encode(textureToBeEncoded);
            takingScreenshot = false;
            CleanTextures();
            EditorUtility.DisplayProgressBar(FEEDBACKS.Titles.wait, FEEDBACKS.TakeScreenshot.takingScreenshot, 1);
            EditorUtility.ClearProgressBar();
        }

        private void CleanTextures()
        {
            DestroyImmediate(gameViewTexture);
            DestroyImmediate(transparentTexture);
            DestroyImmediate(shadowTexture);
            DestroyImmediate(backgroundTexture);
            DestroyImmediate(mockupTexture);
            DestroyImmediate(mockupScreenTexture);
            DestroyImmediate(textureToBeEncoded);
        }

        private void CheckWarnings()
        {
            warningMessages.Clear();
            bool _allNull = true;
            // Check if the list has at least one comera
            if (encodeSettings.setCamerasManually)
            {
                for (int i = 0; i < encodeSettings.cameras.Count; i++)
                {
                    if (encodeSettings.cameras[i] != null)
                    {
                        _allNull = false;
                        break;
                    }
                }
            }
            else
            {
                _allNull = false;
            }
            if (_allNull)
            {
                warningMessages.Add(FEEDBACKS.TakeScreenshot.noCameraInsideArray);
            }
            // Check the save at path
            if (string.IsNullOrEmpty(encodeSettings.saveAtPath))
            {
                warningMessages.Add(FEEDBACKS.TakeScreenshot.selectSaveAtLocation);
            }
            // Check if gameView size is bigger than screenshot size
            if (!encodeSettings.useMockup)
            {
                if (resolutions.gameViewWidth > resolutions.screenshotWidth)
                {
                    warningMessages.Add(FEEDBACKS.TakeScreenshot.gameViewWidthIsBigger);
                }
                if (resolutions.gameViewHeight > resolutions.screenshotHeight)
                {
                    warningMessages.Add(FEEDBACKS.TakeScreenshot.gameViewHightIsBigger);
                }
            }
            // Check if mockup size is bigger than screenshot size
            else
            {
                if (resolutions.mockupWidth > resolutions.screenshotWidth)
                {
                    warningMessages.Add(FEEDBACKS.TakeScreenshot.mockupWidthIsBigger);
                }
                if (resolutions.mockupHeight > resolutions.screenshotHeight)
                {
                    warningMessages.Add(FEEDBACKS.TakeScreenshot.mockupHeightIsBigger);
                }
            }
        }

        /// <summary>
        /// Generates a name to the picture
        /// </summary>
        private string GenerateScreenshotName()
        {
            int _i = 0;
            string _number = "";
            bool _nameAvailable = false;

            // Verifies if the name exists and if so add a number after it
            while (!_nameAvailable)
            {
                if (_i > 0)
                    _number = _i.ToString();
                if (File.Exists(encodeSettings.saveAtPath + "/" + encodeSettings.nameFinal + "_" + _number + ".png"))
                    _i++;
                else
                    _nameAvailable = true;
            }

            return string.Format("{0}/{1}_{2}.png", encodeSettings.saveAtPath, encodeSettings.nameFinal, _number);
        }

        /// <summary>
        /// Get the bytes and encode the texture to PNG
        /// </summary>
        private void Encode(Texture2D image)
        {
            Byte[] bytes;
            if (encodeSettings.encodeType == EzSS_EncodeSettings.EncodeType.PNG)
                bytes = image.EncodeToPNG();
            else
                bytes = image.EncodeToJPG(encodeSettings.jpgQuality);

            string _imageName = GenerateScreenshotName();
            File.WriteAllBytes(_imageName, bytes);
        }
    }
}