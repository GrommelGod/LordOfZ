using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System;

namespace SMG.EzScreenshot
{
    public class EzSS_Mockup : ScriptableObject
    {
        // General
        private EzSS_EncodeSettings encodeSettings;
        private EzSS_Resolutions resolutions;
        private bool showMockup = true;
        private EzSS_Smartphones smartphones = new EzSS_Smartphones();
        private EzSS_Consoles consoles = new EzSS_Consoles();
        private EzSS_Computers computers = new EzSS_Computers();
        private EzSS_Displays displays = new EzSS_Displays();
        private EzSS_Tablets tablets = new EzSS_Tablets();
        private EzSS_Watches watches = new EzSS_Watches();
        // These dictionaries will chance when the category changes
        public Dictionary<Categories, List<string>> mockups = new Dictionary<Categories, List<string>>();
        public Dictionary<string, List<string>> mockupsColors = new Dictionary<string, List<string>>();
        // These dictionaries will be populated inside the init method
        public Dictionary<string, bool> mockupsMultiOriented = new Dictionary<string, bool>();
        public Dictionary<string, Vector2> mockupsPreviewSizes = new Dictionary<string, Vector2>();
        public Dictionary<string, Vector2> mockupsTextureSize = new Dictionary<string, Vector2>();
        public Dictionary<string, string> mockupsAspects = new Dictionary<string, string>();
        public Dictionary<string, Vector2> mockupsScreenSize = new Dictionary<string, Vector2>();
        public Dictionary<string, Vector2> mockupsScreenOffset = new Dictionary<string, Vector2>();
        // The available categories
        public enum Categories
        {
            computer,
            console,
            display,
            smartphone,
            tablet,
            watch
        }
        public Categories selectedCategory = Categories.smartphone;
        // The orientations available
        public enum Orientations
        {
            portrait,
            landscapeLeft,
            landscapeRight
        }
        public Orientations selectedOrientation = Orientations.portrait;
        // Offset
        public Vector2 mockupOffset = Vector2.zero;
        // Selection variables
        public int selectedMockupIndex;
        public int selectedColorIndex;
        private string selectedCategoryName;
        private string selectedMockupName;
        private string selectedColorName;
        private string selectedOrientationName;
        public string selectedAspect;
        public bool selectedMultiOriented;
        public Vector2 selectedPreviewSize;
        public Vector2 selectedTextureSize;
        public Vector2 selectedScreenSize;
        public Vector2 selectedScreenOffset;
        // Mockup textures
        public Texture2D mockupTexture;
        public Texture2D screenTexture;
        private readonly string baseTexturesURL = "https://solomidgames.com/projects/ezScreenshot/mockups/";
        public List<string> mockupsTexturesKeys = new List<string>();
        public List<string> screensTexturesKeys = new List<string>();
        public List<Texture2D> mockupsTextures = new List<Texture2D>();
        public List<Texture2D> screensTextures = new List<Texture2D>();
        private string lastMockupName;
        private string lastColorName;
        private string lastOrientationName;
        // Download manager
        #if UNITY_2017
        WWW wwwRequest;
        #else
        UnityWebRequest webRequest;
        #endif
        private string mockupTextureURL;
        private string screenTextureURL;
        public bool editorUpdateIsRunning = false;
        private bool downloadMockupTexture = false;
        private bool downloadScreenTexture = false;
        private bool isDownloadingMockupTexture = false;
        private bool isDownloadingScreenTexture = false;

        public void Init(EzSS_EncodeSettings encodeSettings, EzSS_Resolutions resolutions)
        {
            this.encodeSettings = encodeSettings;
            this.resolutions = resolutions;
            smartphones.Init(this);
            consoles.Init(this);
            computers.Init(this);
            displays.Init(this);
            tablets.Init(this);
            watches.Init(this);
            // Set the names
            selectedMockupName = mockups[selectedCategory][selectedMockupIndex];
            selectedColorName = mockupsColors[selectedMockupName][selectedColorIndex];
            selectedCategoryName = selectedCategory.ToString();
            selectedOrientationName = selectedOrientation.ToString();
        }

        public void Draw()
        {
            if (encodeSettings.useMockup)
            {
                Rect _rect = EditorGUILayout.BeginVertical();
                EzSS_Style.DrawUILine(EzSS_Style.uiLineColor);
                EditorGUILayout.BeginHorizontal();
                showMockup = EzSS_Style.DrawFoldoutHeader("Mockup", showMockup);
                EzSS_Style.DrawHelpButton("https://solomidgames.com/guides/ez-screenshot/mockup.html");
                EditorGUILayout.EndHorizontal();
                if (showMockup)
                {
                    EditorGUI.indentLevel++;
                    DrawMockupSelector();
                    DrawPreview();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.EndVertical();
                // Draws the colored line
                if (!showMockup || mockupTexture == null)
                {
                    EditorGUI.DrawRect(new Rect(_rect.x - 3, _rect.y + 7, 2f, _rect.height), new Color32(185, 84, 67, 255));
                }
                else
                {
                    EditorGUI.DrawRect(new Rect(_rect.x + 1, _rect.y + 7, 2f, _rect.height), new Color32(185, 84, 67, 255));
                }
            }
        }

        private void DrawMockupSelector()
        {
            // Choose the category to filter the available mockups
            selectedCategory = (Categories)EditorGUILayout.EnumPopup("Category:", selectedCategory);
            selectedCategoryName = selectedCategory.ToString();
            // Choose the mockup and set it's name
            if (selectedMockupIndex >= mockups[selectedCategory].Count)
            {
                selectedMockupIndex = 0;
            }
            selectedMockupIndex = EditorGUILayout.Popup("Mockup:", selectedMockupIndex, mockups[selectedCategory].ToArray());
            selectedMockupName = mockups[selectedCategory][selectedMockupIndex];
            // Before choosing the color is important to check if the selected index is bigger than the available colors length
            if (selectedColorIndex >= mockupsColors[selectedMockupName].Count)
            {
                selectedColorIndex = 0;
            }
            // Choose the mockup color based on the mokcup name
            selectedColorIndex = EditorGUILayout.Popup("Color:", selectedColorIndex, mockupsColors[selectedMockupName].ToArray());
            selectedColorName = mockupsColors[selectedMockupName][selectedColorIndex];
            // Set other basic values
            selectedTextureSize = mockupsTextureSize[selectedMockupName];
            selectedScreenSize = mockupsScreenSize[selectedMockupName];
            selectedPreviewSize = mockupsPreviewSizes[selectedMockupName];
            selectedAspect = mockupsAspects[selectedMockupName];
            if (mockupsScreenOffset.ContainsKey(selectedMockupName))
            {
                selectedScreenOffset = mockupsScreenOffset[selectedMockupName];
            }
            else
            {
                selectedScreenOffset = Vector2.zero;
            }
            // Choose the mockup orientation if it is available
            selectedMultiOriented = mockupsMultiOriented[selectedMockupName];
            if (selectedMultiOriented)
            {
                selectedOrientation = (Orientations)EditorGUILayout.EnumPopup("Orientation:", selectedOrientation);
                if (EzSS_AspectRatio.AspectRatioResults[selectedAspect] < 1 && (selectedOrientation == Orientations.landscapeLeft || selectedOrientation == Orientations.landscapeRight))
                {
                    selectedAspect = EzSS_AspectRatio.InvertAspectRatio[selectedAspect];
                }
            }
            else
            {
                selectedOrientation = Orientations.portrait;
            }
            // Ofsset
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Offset:");
            GUILayout.Space(-15);
            mockupOffset = EditorGUILayout.Vector2Field(string.Empty, mockupOffset);
            EditorGUILayout.EndHorizontal();
            // Is offset bigger or smaller than the screenshot size?
            if (mockupOffset.x > resolutions.screenshotWidth)
            {
                mockupOffset = new Vector2(resolutions.screenshotWidth, mockupOffset.y);
            }
            else if (mockupOffset.x < -resolutions.screenshotWidth)
            {
                mockupOffset = new Vector2(-resolutions.screenshotWidth, mockupOffset.y);
            }
            if (mockupOffset.y > resolutions.screenshotHeight)
            {
                mockupOffset = new Vector2(mockupOffset.x, resolutions.screenshotHeight);
            }
            else if (mockupOffset.y < -resolutions.screenshotHeight)
            {
                mockupOffset = new Vector2(mockupOffset.x, -resolutions.screenshotHeight);
            }
            selectedOrientationName = selectedOrientation.ToString();
            // After the selection check if the textures exists on the project
            CheckMockupTextures();
        }

        private void DrawPreview()
        {
            EditorGUILayout.LabelField("Preview:");
            if (mockupTexture != null)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Space();
                if (selectedOrientation == Orientations.portrait)
                    GUILayout.Label(mockupTexture, GUILayout.Width(selectedPreviewSize.x), GUILayout.Height(selectedPreviewSize.y));
                else
                    GUILayout.Label(mockupTexture, GUILayout.Width(selectedPreviewSize.y), GUILayout.Height(selectedPreviewSize.x));
                EditorGUILayout.Space();
                EditorGUILayout.EndHorizontal();
            }
        }

        private void CheckMockupTextures()
        {
            if (encodeSettings.useMockup &&
            (!string.Equals(lastMockupName, selectedMockupName) ||
            !string.Equals(lastColorName, selectedColorName) ||
            !string.Equals(lastOrientationName, selectedOrientationName) ||
            mockupTexture == null))
            {
                // creates the final url
                string _mockupUrl = baseTexturesURL + selectedCategoryName + "/" + selectedMockupName + "-" + selectedColorName + "-" + selectedOrientationName + ".png";
                string _screenUrl = baseTexturesURL + selectedCategoryName + "/" + selectedMockupName + "-screen" + "-" + selectedOrientationName + ".png";
                // Set the variables to download the textures
                downloadScreenTexture = true;
                downloadMockupTexture = true;
                // The screen texture will be downloaded if the key isn't present inside the dictionary
                if (screensTexturesKeys.Contains(_screenUrl))
                {
                    downloadScreenTexture = false;
                    screenTexture = screensTextures[screensTexturesKeys.IndexOf(_screenUrl)];
                }
                else
                {
                    screenTextureURL = _screenUrl;
                    screenTexture = null;
                }
                // The mockup texture will be downloaded if the key isn't present inside the dictionary
                if (mockupsTexturesKeys.Contains(_mockupUrl))
                {
                    downloadMockupTexture = false;
                    mockupTexture = mockupsTextures[mockupsTexturesKeys.IndexOf(_mockupUrl)];
                    // Updated the mockup information
                    lastMockupName = selectedMockupName;
                    lastColorName = selectedColorName;
                    lastOrientationName = selectedOrientationName;
                }
                else
                {
                    mockupTextureURL = _mockupUrl;
                    mockupTexture = null;
                }

                if (!editorUpdateIsRunning && (downloadMockupTexture || downloadScreenTexture))
                {
                    editorUpdateIsRunning = true;
                    EditorApplication.update += EditorUpdate;
                }
            }
        }

#if UNITY_2017
        private void EditorUpdate()
        {
            if (downloadScreenTexture && !isDownloadingScreenTexture)
            {
                isDownloadingScreenTexture = true;
                wwwRequest = new WWW(screenTextureURL);
            }
            else if (screenTexture != null && downloadMockupTexture && !isDownloadingMockupTexture)
            {
                isDownloadingMockupTexture = true;
                wwwRequest = new WWW(mockupTextureURL);
            }
            
            if (isDownloadingMockupTexture || isDownloadingScreenTexture)
            {
                // Wait until is done
                if (!wwwRequest.isDone)
                {
                    if (isDownloadingMockupTexture)
                    {
                        EditorUtility.DisplayProgressBar(FEEDBACKS.Titles.wait, FEEDBACKS.Mockup.definingMockTexture, wwwRequest.progress);
                    }
                    else if (isDownloadingScreenTexture)
                    {
                        EditorUtility.DisplayProgressBar(FEEDBACKS.Titles.wait, FEEDBACKS.Mockup.definingMockupScreen, wwwRequest.progress);
                    }
                    return;
                }
                // Check for erros
                if (!string.IsNullOrEmpty(wwwRequest.error))
                {
                    if (isDownloadingMockupTexture)
                    {
                        EditorUtility.DisplayDialog(FEEDBACKS.Titles.attention, FEEDBACKS.Mockup.errorDefiningMockupTexture + wwwRequest.error, FEEDBACKS.Buttons.close);
                    }
                    else if (isDownloadingScreenTexture)
                    {
                        EditorUtility.DisplayDialog(FEEDBACKS.Titles.attention, FEEDBACKS.Mockup.errorDefiningMockupScreen + wwwRequest.error, FEEDBACKS.Buttons.close);
                    }
                    encodeSettings.useMockup = false;
                    FinishEditorUpdate();
                }
                else
                {
                    if (downloadMockupTexture && isDownloadingMockupTexture)
                    {
                        isDownloadingMockupTexture = false;
                        downloadMockupTexture = false;
                        mockupTexture = new Texture2D(wwwRequest.texture.width, wwwRequest.texture.height, TextureFormat.ARGB32, false);
                        mockupTexture.SetPixels(wwwRequest.texture.GetPixels());
                        mockupTexture.Apply();
                        mockupsTexturesKeys.Add(mockupTextureURL);
                        mockupsTextures.Add(mockupTexture);
                    }
                    else if (downloadScreenTexture && isDownloadingScreenTexture)
                    {
                        isDownloadingScreenTexture = false;
                        downloadScreenTexture = false;
                        screenTexture = new Texture2D(wwwRequest.texture.width, wwwRequest.texture.height, TextureFormat.ARGB32, false);
                        screenTexture.SetPixels(wwwRequest.texture.GetPixels());
                        screenTexture.Apply();
                        screensTexturesKeys.Add(screenTextureURL);
                        screensTextures.Add(screenTexture);
                    }
                }
            }

            // Finish if there's nothing more to download
            if (mockupTexture != null && screenTexture != null)
            {
                FinishEditorUpdate();
            }
        }
#else
        private void EditorUpdate()
        {
            if (downloadScreenTexture && !isDownloadingScreenTexture)
            {
                isDownloadingScreenTexture = true;
                webRequest = UnityWebRequestTexture.GetTexture(screenTextureURL);
                // Starts the web request
                webRequest.SendWebRequest();
            }
            else if (screenTexture != null && downloadMockupTexture && !isDownloadingMockupTexture)
            {
                isDownloadingMockupTexture = true;
                webRequest = UnityWebRequestTexture.GetTexture(mockupTextureURL);
                // Starts the web request
                webRequest.SendWebRequest();
            }
            if (isDownloadingMockupTexture || isDownloadingScreenTexture)
            {
                // Wait until is done
                if (!webRequest.isDone)
                {
                    if (isDownloadingMockupTexture)
                    {
                        EditorUtility.DisplayProgressBar(FEEDBACKS.Titles.wait, FEEDBACKS.Mockup.definingMockTexture, webRequest.downloadProgress);
                    }
                    else if (isDownloadingScreenTexture)
                    {
                        EditorUtility.DisplayProgressBar(FEEDBACKS.Titles.wait, FEEDBACKS.Mockup.definingMockupScreen, webRequest.downloadProgress);
                    }
                    return;
                }
                // Check for erros
                if (webRequest.isNetworkError || webRequest.isHttpError)
                {
                    if (isDownloadingMockupTexture)
                    {
                        EditorUtility.DisplayDialog(FEEDBACKS.Titles.attention, FEEDBACKS.Mockup.errorDefiningMockupTexture + webRequest.error, FEEDBACKS.Buttons.close);
                    }
                    else if (isDownloadingScreenTexture)
                    {
                        EditorUtility.DisplayDialog(FEEDBACKS.Titles.attention, FEEDBACKS.Mockup.errorDefiningMockupScreen + webRequest.error, FEEDBACKS.Buttons.close);
                    }
                    encodeSettings.useMockup = false;
                    FinishEditorUpdate();
                }
                else
                {
                    if (downloadMockupTexture && isDownloadingMockupTexture)
                    {
                        isDownloadingMockupTexture = false;
                        downloadMockupTexture = false;
                        mockupTexture = new Texture2D(DownloadHandlerTexture.GetContent(webRequest).width, DownloadHandlerTexture.GetContent(webRequest).height, TextureFormat.ARGB32, false);
                        Graphics.CopyTexture(DownloadHandlerTexture.GetContent(webRequest), mockupTexture);
                        mockupsTexturesKeys.Add(mockupTextureURL);
                        mockupsTextures.Add(mockupTexture);
                    }
                    else if (downloadScreenTexture && isDownloadingScreenTexture)
                    {
                        isDownloadingScreenTexture = false;
                        downloadScreenTexture = false;
                        screenTexture = new Texture2D(DownloadHandlerTexture.GetContent(webRequest).width, DownloadHandlerTexture.GetContent(webRequest).height, TextureFormat.ARGB32, false);
                        Graphics.CopyTexture(DownloadHandlerTexture.GetContent(webRequest), screenTexture);
                        screensTexturesKeys.Add(screenTextureURL);
                        screensTextures.Add(screenTexture);
                    }
                }
            }

            // Finish if there's nothing more to download
            if (mockupTexture != null && screenTexture != null)
            {
                FinishEditorUpdate();
            }
        }
#endif

        private void FinishEditorUpdate()
        {
            // Update is no longer necessary
            EditorApplication.update -= EditorUpdate;
            editorUpdateIsRunning = false;
            // editorUpdateIsRunning = false;
            EditorUtility.ClearProgressBar();
            // Updated the mockup information
            lastMockupName = selectedMockupName;
            lastColorName = selectedColorName;
            lastOrientationName = selectedOrientationName;
            GUI.changed = true;
        }
    }
}