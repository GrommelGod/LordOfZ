using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace SMG.EzScreenshot
{
    [InitializeOnLoad]
    public class EzSS_Window : EditorWindow
    {
        private Vector2 scrollPos;

        private EzSS_Template template;
        private EzSS_EncodeSettings encodeSettings;
        private EzSS_Resolutions resolutions;
        private EzSS_Mockup mockup;
        private EzSS_Shadow shadow;
        private EzSS_Background background;
        private EzSS_TakeScreenshot takeScreenshot;
        private EzSS_DataManager dataManager;

        private string sceneName = string.Empty;
        private bool onPlayMode = false;

        private void OnEnable()
        {
            Init();
            sceneName = EditorSceneManager.GetActiveScene().name;
            EditorApplication.update += EditorUpdate;
        }

        private void OnDisable()
        {
            EditorApplication.update -= EditorUpdate;
        }

        private void Init()
        {
            // Create instances
            dataManager = ScriptableObject.CreateInstance<EzSS_DataManager>();
            template = ScriptableObject.CreateInstance<EzSS_Template>();
            encodeSettings = ScriptableObject.CreateInstance<EzSS_EncodeSettings>();
            mockup = ScriptableObject.CreateInstance<EzSS_Mockup>();
            shadow = ScriptableObject.CreateInstance<EzSS_Shadow>();
            background = ScriptableObject.CreateInstance<EzSS_Background>();
            resolutions = ScriptableObject.CreateInstance<EzSS_Resolutions>();
            takeScreenshot = ScriptableObject.CreateInstance<EzSS_TakeScreenshot>();
            // Init the data manager
            dataManager.Init(encodeSettings, resolutions, mockup, shadow, background);
            // Load saved data			
            dataManager.LoadTemplate(dataManager.data.index);
            // Init the others
            template.Init(dataManager);
            resolutions.Init(encodeSettings, mockup);
            takeScreenshot.Init(encodeSettings, resolutions, mockup, shadow, background);
            mockup.Init(encodeSettings, resolutions);
            shadow.Init(encodeSettings);
            background.Init(encodeSettings);
        }

        private void EditorUpdate()
        {
            if (sceneName != EditorSceneManager.GetActiveScene().name)
            {
                sceneName = EditorSceneManager.GetActiveScene().name;
                Init();
            }
            if (EditorApplication.isPlaying && !onPlayMode)
            {
                onPlayMode = true;
            }
            if (!EditorApplication.isPlaying && onPlayMode)
            {
                onPlayMode = false;
                Init();
            }
            if (GUI.changed)
            {
                GUI.changed = false;
                Repaint();
            }
        }

        private void OnGUI()
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            template.Draw();
            EditorGUI.BeginDisabledGroup(dataManager.data.templates.Count <= 0 || dataManager.data.index < 0);
            EditorGUI.BeginChangeCheck();
            encodeSettings.Draw();
            resolutions.Draw(); ;
            mockup.Draw();
            shadow.Draw();
            background.Draw(position);
            if (EditorGUI.EndChangeCheck())
            {
                dataManager.UpdateTemplate();
                template.UpdateTemplatesReList();
                encodeSettings.UpdateCamerasReList();
                background.UpdateBgColorsReList();
            }
            EditorGUI.EndDisabledGroup();
            EzSS_Style.DrawUILine(EzSS_Style.uiLineColor);
            EditorGUILayout.EndScrollView();
            takeScreenshot.Draw(position);

        }

        #region    [ Menu Items  ]
        [MenuItem("Window/Ez Screenshot/Open")]
        public static void OpenWindow()
        {
            EzSS_Window _window = (EzSS_Window)GetWindow(typeof(EzSS_Window));
            _window.minSize = new Vector2(380, 650);
            _window.maxSize = new Vector2(500, 1000);
            _window.titleContent = new GUIContent(" Ez Screenshot", EditorGUIUtility.FindTexture("d_RectTransformBlueprint"));
            _window.autoRepaintOnSceneChange = true;
            _window.Show();
        }

        [MenuItem("Window/Ez Screenshot/Guide")]
        public static void Guide()
        {
            Application.OpenURL("https://solomidgames.com/guides/ez-screenshot/quick-overview.html");
        }

        [MenuItem("Window/Ez Screenshot/Help")]
        public static void Help()
        {
            Application.OpenURL("mailto:help@solomidgames.com");
        }

        [MenuItem("Window/Ez Screenshot/Forum Thread")]
        public static void ForumThread()
        {
            Application.OpenURL("https://forum.unity.com/threads/released-ez-screenshot.633328/");
        }

        [MenuItem("Window/Ez Screenshot/More Assets")]
        public static void MoreAssets()
        {
            Application.OpenURL("https://www.assetstore.unity3d.com/en/#!/search/page=1/sortby=popularity/query=publisher:11524");
        }

        [MenuItem("Window/Ez Screenshot/Website")]
        public static void Website()
        {
            Application.OpenURL("https://solomidgames.com");
        }

        [MenuItem("Window/Ez Screenshot/Follow us on Twitter")]
        public static void FollowTwitter()
        {
            Application.OpenURL("https://twitter.com/solomidgames");
        }

        [MenuItem("Window/Ez Screenshot/Take Screenshot %t")]
        public static void TakeScreenshot()
        {
            EzSS_Window[] _window = Resources.FindObjectsOfTypeAll<EzSS_Window>();
            if (_window.Length > 0)
            {
                _window[0].takeScreenshot.TakeScreenshot(false);
            }
            else
            {
                Debug.LogWarning(FEEDBACKS.Configuration.ezScreenshotWindowMustBeOpened);
            }
        }
        #endregion [ Menu Items ]
    }
}