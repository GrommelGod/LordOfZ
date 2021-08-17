using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG.EzScreenshot
{
    public class EzSS_Smartphones : EzSS_Devices
    {
        public override void SetMockups()
        {
            mockup.mockups.Add(EzSS_Mockup.Categories.smartphone, new List<string>()
            {
                "apple-iphone8",
                "apple-iphoneXr",
                "apple-iphoneXs",
                "google-pixel3",
                "google-pixel3Xl",
                "huawei-nova4",
                "lg-v40",
                "onePlus-6T",
                "samsung-galaxyFoldClosed",
                "samsung-galaxyFoldOpened",
                "samsung-galaxyS9",
                "samsung-galaxyS10",
                "samsung-galaxyS10Plus"
            });
        }

        public override void SetMockupsColors()
        {
            mockup.mockupsColors.Add("apple-iphone8", new List<string>() { "gold", "goldRose", "silver", "spaceGray" });
            mockup.mockupsColors.Add("apple-iphoneXr", new List<string>() { "black", "blue", "coral", "red", "white", "yellow" });
            mockup.mockupsColors.Add("apple-iphoneXs", new List<string>() { "gold", "silver", "spaceGray" });
            mockup.mockupsColors.Add("google-pixel3", new List<string>() { "clearlyWhite", "justBlack", "notPink" }); 
            mockup.mockupsColors.Add("google-pixel3Xl", new List<string>() { "clearlyWhite", "justBlack", "notPink" }); 
            mockup.mockupsColors.Add("huawei-nova4", new List<string>() { "blue", "pink" });
            mockup.mockupsColors.Add("lg-v40", new List<string>() { "black" });
            mockup.mockupsColors.Add("onePlus-6T", new List<string>() { "flatBlack" });
            mockup.mockupsColors.Add("samsung-galaxyFoldClosed", new List<string>() { "black" });
            mockup.mockupsColors.Add("samsung-galaxyFoldOpened", new List<string>() { "black" });
            mockup.mockupsColors.Add("samsung-galaxyS9", new List<string>() { "coralBlue", "midNightBlack", "lilacPurple", "titaniumGray" });
            mockup.mockupsColors.Add("samsung-galaxyS10", new List<string>() { "black" });
            mockup.mockupsColors.Add("samsung-galaxyS10Plus", new List<string>() { "black" });
        }

        public override void SetMockupsMultiOriented()
        {
            mockup.mockupsMultiOriented.Add("apple-iphone8", true);
            mockup.mockupsMultiOriented.Add("apple-iphoneXr", true);
            mockup.mockupsMultiOriented.Add("apple-iphoneXs", true);
            mockup.mockupsMultiOriented.Add("google-pixel3", true);
            mockup.mockupsMultiOriented.Add("google-pixel3Xl", true);
            mockup.mockupsMultiOriented.Add("huawei-nova4", true);
            mockup.mockupsMultiOriented.Add("lg-v40", true);
            mockup.mockupsMultiOriented.Add("onePlus-6T", true);
            mockup.mockupsMultiOriented.Add("samsung-galaxyFoldClosed", true);
            mockup.mockupsMultiOriented.Add("samsung-galaxyFoldOpened", true);
            mockup.mockupsMultiOriented.Add("samsung-galaxyS9", true);
            mockup.mockupsMultiOriented.Add("samsung-galaxyS10", true);
            mockup.mockupsMultiOriented.Add("samsung-galaxyS10Plus", true);
        }

        public override void SetMockupsPreviewSizes()
        {
            mockup.mockupsPreviewSizes.Add("apple-iphone8", new Vector2(100f, 200f));
            mockup.mockupsPreviewSizes.Add("apple-iphoneXr", new Vector2(100f, 200f));
            mockup.mockupsPreviewSizes.Add("apple-iphoneXs", new Vector2(100f, 200f));
            mockup.mockupsPreviewSizes.Add("google-pixel3", new Vector2(100f, 200f));
            mockup.mockupsPreviewSizes.Add("google-pixel3Xl", new Vector2(100f, 200f));
            mockup.mockupsPreviewSizes.Add("huawei-nova4", new Vector2(100f, 200f));
            mockup.mockupsPreviewSizes.Add("lg-v40", new Vector2(100f, 200f));
            mockup.mockupsPreviewSizes.Add("onePlus-6T", new Vector2(100f, 200f));
            mockup.mockupsPreviewSizes.Add("samsung-galaxyFoldClosed", new Vector2(80f, 200f));
            mockup.mockupsPreviewSizes.Add("samsung-galaxyFoldOpened", new Vector2(150f, 200f));
            mockup.mockupsPreviewSizes.Add("samsung-galaxyS9", new Vector2(100f, 200f));
            mockup.mockupsPreviewSizes.Add("samsung-galaxyS10", new Vector2(100f, 200f));
            mockup.mockupsPreviewSizes.Add("samsung-galaxyS10Plus", new Vector2(100f, 200f));
        }

        public override void SetMockupsSizes()
        {
            mockup.mockupsTextureSize.Add("apple-iphone8", new Vector2(993f, 1999f));
            mockup.mockupsTextureSize.Add("apple-iphoneXr", new Vector2(978f, 1959f));
            mockup.mockupsTextureSize.Add("apple-iphoneXs", new Vector2(1066f, 2147f));
            mockup.mockupsTextureSize.Add("google-pixel3", new Vector2(1199f, 2543f));
            mockup.mockupsTextureSize.Add("google-pixel3Xl", new Vector2(1591f, 3248f));
            mockup.mockupsTextureSize.Add("huawei-nova4", new Vector2(328f, 679f));
            mockup.mockupsTextureSize.Add("lg-v40", new Vector2(615f, 1267f));
            mockup.mockupsTextureSize.Add("onePlus-6T", new Vector2(325f, 662f));
            mockup.mockupsTextureSize.Add("samsung-galaxyFoldClosed", new Vector2(879f, 2296f));
            mockup.mockupsTextureSize.Add("samsung-galaxyFoldOpened", new Vector2(1703f, 2298f));
            mockup.mockupsTextureSize.Add("samsung-galaxyS9", new Vector2(1306f, 2777f));
            mockup.mockupsTextureSize.Add("samsung-galaxyS10", new Vector2(875f, 1832f));
            mockup.mockupsTextureSize.Add("samsung-galaxyS10Plus", new Vector2(580f, 1219f)); 
        }

        public override void SetMockupsAspects()
        {
            mockup.mockupsAspects.Add("apple-iphone8", "9:16");
            mockup.mockupsAspects.Add("apple-iphoneXr", "9:19.5");
            mockup.mockupsAspects.Add("apple-iphoneXs", "9:19.5");
            mockup.mockupsAspects.Add("google-pixel3", "9:18");
            mockup.mockupsAspects.Add("google-pixel3Xl", "9:18.5");
            mockup.mockupsAspects.Add("huawei-nova4", "9:19.5");
            mockup.mockupsAspects.Add("lg-v40", "9:19.5");
            mockup.mockupsAspects.Add("onePlus-6T", "9:19.5");
            mockup.mockupsAspects.Add("samsung-galaxyFoldClosed", "9:21");
            mockup.mockupsAspects.Add("samsung-galaxyFoldOpened", "3:4");
            mockup.mockupsAspects.Add("samsung-galaxyS9", "9:18.5");
            mockup.mockupsAspects.Add("samsung-galaxyS10", "9:19.5");
            mockup.mockupsAspects.Add("samsung-galaxyS10Plus", "9:19.5");
        }

        public override void SetMockupsScreenSizes()
        {
            mockup.mockupsScreenSize.Add("apple-iphone8", new Vector2(88.41f, 77.38f));
            mockup.mockupsScreenSize.Add("apple-iphoneXr", new Vector2(87.01f, 94.12f));
            mockup.mockupsScreenSize.Add("apple-iphoneXs", new Vector2(88.46f, 95.01f));
            mockup.mockupsScreenSize.Add("google-pixel3", new Vector2(91.07f, 85.93f));
            mockup.mockupsScreenSize.Add("google-pixel3Xl", new Vector2(91.63f, 92.13f));
            mockup.mockupsScreenSize.Add("huawei-nova4", new Vector2(92.98f, 95.43f));
            mockup.mockupsScreenSize.Add("lg-v40", new Vector2(96.12f, 98.94f));
            mockup.mockupsScreenSize.Add("onePlus-6T", new Vector2(95.15f, 98.28f));
            mockup.mockupsScreenSize.Add("samsung-galaxyFoldClosed", new Vector2(74.94f, 66.98f));
            mockup.mockupsScreenSize.Add("samsung-galaxyFoldOpened", new Vector2(90.66f, 94.08f));
            mockup.mockupsScreenSize.Add("samsung-galaxyS9", new Vector2(94.72f, 91.38f));
            mockup.mockupsScreenSize.Add("samsung-galaxyS10", new Vector2(97.71f, 96.45f));
            mockup.mockupsScreenSize.Add("samsung-galaxyS10Plus", new Vector2(96.72f, 96.06f));
        }

        public override void SetMockupsScreenOffsets()
        {
            mockup.mockupsScreenOffset.Add("google-pixel3", new Vector2(-5, 0f));
            mockup.mockupsScreenOffset.Add("google-pixel3Xl", new Vector2(0, 75f));
            mockup.mockupsScreenOffset.Add("huawei-nova4", new Vector2(0, 4f));
            mockup.mockupsScreenOffset.Add("lg-v40", new Vector2(0, -4.5f));
            mockup.mockupsScreenOffset.Add("onePlus-6T", new Vector2(0, 10f));
            mockup.mockupsScreenOffset.Add("samsung-galaxyFoldClosed", new Vector2(-25f, 19f));
        }
    }
}