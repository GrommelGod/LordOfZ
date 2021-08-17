using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SMG.EzScreenshot
{
    [Serializable]
    public class EzSS_Data : ScriptableObject
    {
        public int index = -1;
        public List<Templates> templates;
    }

    [Serializable]
    public class Templates
    {
        public string templateName;
        public DataEncodeSettings encodeSettings;
        public DataResolutions resolutions;
        public DataMockup mockup;
        public DataShadow shadow;
        public DataBackground background;
    }

    [Serializable]
    public class DataEncodeSettings
    {
        public List<string> camerasNames = new List<string>();
        public EzSS_EncodeSettings.EncodeType encodeType;
        public int jpgQuality;
        public string namePrefix;
        public bool useDate;
        public bool useTime;
        public bool useMockup;
        public bool useShadow;
        public bool useBackground;
        public string saveAtPath;
    }

    [Serializable]
    public class DataMockup
    {
        public EzSS_Mockup.Categories selectedCategory;
        public EzSS_Mockup.Orientations selectedOrientation;
        public int selectedMockupIndex;
        public int selectedColorIndex;
        public Vector2 mockupOffset;
    }

    [Serializable]
    public class DataShadow
    {
        public Color color;
        public Vector2 direction;
        public int softness;
    }

    [Serializable]
    public class DataBackground
    {
        public List<BgColorProperties> bgColors = new List<BgColorProperties>();
        public EzSS_Background.BgTypes bgType;
    }

    [Serializable]
    public class DataResolutions
    {
        // Resolution - Screenshot
        public EzSS_AspectRatio.AspectType screenshotAspectType;
        public EzSS_AspectRatio.AspectType lastScreenshotAspectType;
        public int screenshotPortraitAspectIndex;
        public int screenshotLandscapeAspectIndex;
        public int screenshotWidth;
        public int screenshotHeight;
        public string screenshotAspect;

        // Resolution - GameView
        public EzSS_AspectRatio.AspectType gameViewAspectType;
        public EzSS_AspectRatio.AspectType lastGameViewAspectType;
        public int gameViewPortraitAspectIndex;
        public int gameViewLandscapeAspectIndex;
        public int gameViewWidth;
        public int gameViewHeight;
        public string gameViewAspect;

        // Resolution - Mockup
        public EzSS_AspectRatio.AspectType mockupAspectType;
        public EzSS_AspectRatio.AspectType lastMockupAspectType;
        public int mockupPortraitAspectIndex;
        public int mockupLandscapeAspectIndex;
        public int mockupWidth;
        public int mockupHeight;
        public string mockupResAspect;

        // Resolution - General
        public Vector2 aspectVector;
    }
}

