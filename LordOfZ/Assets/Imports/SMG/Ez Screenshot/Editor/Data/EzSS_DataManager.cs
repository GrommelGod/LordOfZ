using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System;

namespace SMG.EzScreenshot
{
    public class EzSS_DataManager : ScriptableObject
    {
        public EzSS_Data data;
        private EzSS_EncodeSettings encodeSettings;
        private EzSS_Resolutions resolutions;
        private EzSS_Mockup mockup;
        private EzSS_Shadow shadow;
        private EzSS_Background background;

        private void OnEnable()
        {
            data = (EzSS_Data)AssetDatabase.LoadAssetAtPath("Assets/SMG/Ez Screenshot/Editor/Data/Data.asset", typeof(EzSS_Data));
        }

        public void Init(EzSS_EncodeSettings encodeSettings, EzSS_Resolutions resolutions, EzSS_Mockup mockup, EzSS_Shadow shadow, EzSS_Background background)
        {
            this.encodeSettings = encodeSettings;
            this.resolutions = resolutions;
            this.mockup = mockup;
            this.shadow = shadow;
            this.background = background;
        }

        public void LoadTemplate(int index)
        {
            if (index >= 0 && index < data.templates.Count)
            {
                Load(index);
            }
            else
            {
                data.index = -1;
            }
        }

        public void NewTemplate(string templateName)
        {
            data.templates.Add(new Templates());
            int _index = data.templates.Count - 1;
            // Set to default the new template
            data.templates[_index].encodeSettings = new DataEncodeSettings();
            data.templates[_index].resolutions = new DataResolutions();
            data.templates[_index].mockup = new DataMockup();
            data.templates[_index].shadow = new DataShadow();
            data.templates[_index].background = new DataBackground();
            data.templates[_index].templateName = templateName;
            // Set the index and load the new template
            data.index = _index;
            Load(_index);
        }

        public void UpdateTemplate()
        {
            if (data.index >= 0)
            {
                SaveTemplate(data.index);
            }
        }

        private void Load(int index)
        {
            // Encode Settings
            encodeSettings.cameras.Clear();
            Camera[] _cameras = FindObjectsOfType<Camera>();
            for (int i = 0; i < _cameras.Length; i++)
            {
                if (data.templates[index].encodeSettings.camerasNames.Contains(_cameras[i].name))
                {
                    encodeSettings.cameras.Add(_cameras[i]);
                }
            }
            encodeSettings.encodeType = data.templates[index].encodeSettings.encodeType;
            encodeSettings.jpgQuality = data.templates[index].encodeSettings.jpgQuality;
            encodeSettings.namePrefix = data.templates[index].encodeSettings.namePrefix;
            encodeSettings.useDate = data.templates[index].encodeSettings.useDate;
            encodeSettings.useTime = data.templates[index].encodeSettings.useTime;
            encodeSettings.useMockup = data.templates[index].encodeSettings.useMockup;
            encodeSettings.useShadow = data.templates[index].encodeSettings.useShadow;
            encodeSettings.useBackground = data.templates[index].encodeSettings.useBackground;
            encodeSettings.saveAtPath = data.templates[index].encodeSettings.saveAtPath;
            // Resolutions - General
            resolutions.aspectVector = data.templates[index].resolutions.aspectVector;
            // Resolutions - Screenshot
            resolutions.screenshotAspectType = data.templates[index].resolutions.screenshotAspectType;
            resolutions.lastScreenshotAspectType = data.templates[index].resolutions.lastScreenshotAspectType;
            resolutions.screenshotPortraitAspectIndex = data.templates[index].resolutions.screenshotPortraitAspectIndex;
            resolutions.screenshotLandscapeAspectIndex = data.templates[index].resolutions.screenshotLandscapeAspectIndex;
            resolutions.screenshotWidth = data.templates[index].resolutions.screenshotWidth;
            resolutions.screenshotHeight = data.templates[index].resolutions.screenshotHeight;
            resolutions.screenshotAspect = data.templates[index].resolutions.screenshotAspect;
            // Resolution - GameView
            resolutions.gameViewAspectType = data.templates[index].resolutions.gameViewAspectType;
            resolutions.lastGameViewAspectType = data.templates[index].resolutions.lastGameViewAspectType;
            resolutions.gameViewPortraitAspectIndex = data.templates[index].resolutions.gameViewPortraitAspectIndex;
            resolutions.gameViewLandscapeAspectIndex = data.templates[index].resolutions.gameViewLandscapeAspectIndex;
            resolutions.gameViewWidth = data.templates[index].resolutions.gameViewWidth;
            resolutions.gameViewHeight = data.templates[index].resolutions.gameViewHeight;
            resolutions.gameViewAspect = data.templates[index].resolutions.gameViewAspect;
            // Resolution - Mockup
            resolutions.mockupAspectType = data.templates[index].resolutions.mockupAspectType;
            resolutions.lastMockupAspectType = data.templates[index].resolutions.lastMockupAspectType;
            resolutions.mockupPortraitAspectIndex = data.templates[index].resolutions.mockupPortraitAspectIndex;
            resolutions.mockupLandscapeAspectIndex = data.templates[index].resolutions.mockupLandscapeAspectIndex;
            resolutions.mockupWidth = data.templates[index].resolutions.mockupWidth;
            resolutions.mockupHeight = data.templates[index].resolutions.mockupHeight;
            resolutions.mockupAspect = data.templates[index].resolutions.mockupResAspect;
            // Mockup
            if (encodeSettings.useMockup)
            {
                mockup.selectedCategory = data.templates[index].mockup.selectedCategory;
                mockup.selectedOrientation = data.templates[index].mockup.selectedOrientation;
                mockup.selectedMockupIndex = data.templates[index].mockup.selectedMockupIndex;
                mockup.selectedColorIndex = data.templates[index].mockup.selectedColorIndex;
                mockup.mockupOffset = data.templates[index].mockup.mockupOffset;
            }
            // Shadow
            if (encodeSettings.useShadow)
            {
                shadow.color = data.templates[index].shadow.color;
                shadow.direction = data.templates[index].shadow.direction;
                shadow.softness = data.templates[index].shadow.softness;
            }
            // Background
            if (encodeSettings.useBackground)
            {
                background.bgColors = new List<BgColorProperties>(data.templates[index].background.bgColors);
                background.bgType = data.templates[index].background.bgType;
            }
            // Reset the reorderable lists
            encodeSettings.camerasReList = null;
            // Update the index
            data.index = index;
        }

        private void SaveTemplate(int index)
        {
            // Configuration
            data.templates[index].encodeSettings.camerasNames.Clear();
            for (int i = 0; i < encodeSettings.cameras.Count; i++)
            {
                if (encodeSettings.cameras[i] != null)
                    data.templates[index].encodeSettings.camerasNames.Add(encodeSettings.cameras[i].name);
            }
            data.templates[index].encodeSettings.encodeType = encodeSettings.encodeType;
            data.templates[index].encodeSettings.jpgQuality = encodeSettings.jpgQuality;
            data.templates[index].encodeSettings.namePrefix = encodeSettings.namePrefix;
            data.templates[index].encodeSettings.useDate = encodeSettings.useDate;
            data.templates[index].encodeSettings.useTime = encodeSettings.useTime;
            data.templates[index].encodeSettings.useMockup = encodeSettings.useMockup;
            data.templates[index].encodeSettings.useShadow = encodeSettings.useShadow;
            data.templates[index].encodeSettings.useBackground = encodeSettings.useBackground;
            data.templates[index].encodeSettings.saveAtPath = encodeSettings.saveAtPath;
            // Resolutions - General
            data.templates[index].resolutions.aspectVector = resolutions.aspectVector;
            // Resolution - Screenshot
            data.templates[index].resolutions.screenshotAspectType = resolutions.screenshotAspectType;
            data.templates[index].resolutions.lastScreenshotAspectType = resolutions.lastScreenshotAspectType;
            data.templates[index].resolutions.screenshotPortraitAspectIndex = resolutions.screenshotPortraitAspectIndex;
            data.templates[index].resolutions.screenshotLandscapeAspectIndex = resolutions.screenshotLandscapeAspectIndex;
            data.templates[index].resolutions.screenshotWidth = resolutions.screenshotWidth;
            data.templates[index].resolutions.screenshotHeight = resolutions.screenshotHeight;
            data.templates[index].resolutions.screenshotAspect = resolutions.screenshotAspect;
            // Resolution - GameView
            data.templates[index].resolutions.gameViewAspectType = resolutions.gameViewAspectType;
            data.templates[index].resolutions.lastGameViewAspectType = resolutions.lastGameViewAspectType;
            data.templates[index].resolutions.gameViewPortraitAspectIndex = resolutions.gameViewPortraitAspectIndex;
            data.templates[index].resolutions.gameViewLandscapeAspectIndex = resolutions.gameViewLandscapeAspectIndex;
            data.templates[index].resolutions.gameViewWidth = resolutions.gameViewWidth;
            data.templates[index].resolutions.gameViewHeight = resolutions.gameViewHeight;
            data.templates[index].resolutions.gameViewAspect = resolutions.gameViewAspect;
            // Resolution - Mockup
            data.templates[index].resolutions.mockupAspectType = resolutions.mockupAspectType;
            data.templates[index].resolutions.lastMockupAspectType = resolutions.lastMockupAspectType;
            data.templates[index].resolutions.mockupPortraitAspectIndex = resolutions.mockupPortraitAspectIndex;
            data.templates[index].resolutions.mockupLandscapeAspectIndex = resolutions.mockupLandscapeAspectIndex;
            data.templates[index].resolutions.mockupWidth = resolutions.mockupWidth;
            data.templates[index].resolutions.mockupHeight = resolutions.mockupHeight;
            data.templates[index].resolutions.mockupResAspect = resolutions.mockupAspect;
            // Mockup
            data.templates[index].mockup.selectedCategory = mockup.selectedCategory;
            data.templates[index].mockup.selectedOrientation = mockup.selectedOrientation;
            data.templates[index].mockup.selectedMockupIndex = mockup.selectedMockupIndex;
            data.templates[index].mockup.selectedColorIndex = mockup.selectedColorIndex;
            data.templates[index].mockup.mockupOffset = mockup.mockupOffset;
            // Shadow
            data.templates[index].shadow.color = shadow.color;
            data.templates[index].shadow.direction = shadow.direction;
            data.templates[index].shadow.softness = shadow.softness;
            // Background
            data.templates[index].background.bgColors = new List<BgColorProperties>(background.bgColors);
            data.templates[index].background.bgType = background.bgType;
            // Set the current index
            data.index = index;
            // To complete the save object must be set dirty
            EditorUtility.SetDirty(data);
            if (!EditorApplication.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
    }
}