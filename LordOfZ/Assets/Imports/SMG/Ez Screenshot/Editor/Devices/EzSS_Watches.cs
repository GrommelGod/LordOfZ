using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG.EzScreenshot
{
    public class EzSS_Watches : EzSS_Devices
    {
        public override void SetMockups()
        {
            mockup.mockups.Add(EzSS_Mockup.Categories.watch, new List<string>()
            {
                "apple-watch",
                "apple-watch4",
                "motorola-moto360-42mm",
                "motorola-moto360-46mm",
            });
        }

        public override void SetMockupsColors()
        {
            mockup.mockupsColors.Add("apple-watch", new List<string>() { "blackSteel+black", "edition", "gold+midnightBlue", "goldAluminum+cocoa", "roseGold+blackLeather", "roseGold+lavender", "roseGoldAluminum+midnightBlue", 
            "silderAluminum+concrete", "silverAluminum+flatSilverVoltNike", "silverAluminum+flatSilverWhiteNike", "silverAluminum+green", "silverAluminum+lightPink", "silverAluminum+oceanBlue", "silverAluminum+pinkSand", 
            "silverAluminum+red", "silverAluminum+turquoise", "silverAluminum+white", "silverAluminum+yellow", "spaceBlackSteel+black", "spaceGrayAluminum+black", "spaceGrayAluminum+blackCoolGrayNike", 
            "spaceGrayAluminum+blackVoltNike", "sportAluminum+blue", "sportAluminum+fog", "sportAluminum+green", "sportAluminum+red", "sportAluminum+walnut", "sportAluminum+white", "sportAluminumGold+antiqueWhite", 
            "sportAluminumRoseGold+stone", "sportSpaceGray+black", "stainlessSteel+blackLeather", "steel+snowWhite", "steel+white" });
            mockup.mockupsColors.Add("apple-watch4", new List<string>() { "black", "roseGold", "white" });
            mockup.mockupsColors.Add("motorola-moto360-42mm", new List<string>() { "gold+blush", "gold+stoneGrey", "roseGold+blush", "roseGold+stoneGrey", "silver+blush", "silver+stoneGrey", });
            mockup.mockupsColors.Add("motorola-moto360-46mm", new List<string>() { "black+black", "black+cognac", "gold+black", "gold+cognac", "silver+black", "gold+cognac" });
        }

        public override void SetMockupsMultiOriented()
        {
            mockup.mockupsMultiOriented.Add("apple-watch", false);
            mockup.mockupsMultiOriented.Add("apple-watch4", false);
            mockup.mockupsMultiOriented.Add("motorola-moto360-42mm", false);
            mockup.mockupsMultiOriented.Add("motorola-moto360-46mm", false);
        }

        public override void SetMockupsPreviewSizes()
        {
            mockup.mockupsPreviewSizes.Add("apple-watch", new Vector2(100f, 175f));
            mockup.mockupsPreviewSizes.Add("apple-watch4", new Vector2(100f, 150f));
            mockup.mockupsPreviewSizes.Add("motorola-moto360-42mm", new Vector2(100f, 150f));
            mockup.mockupsPreviewSizes.Add("motorola-moto360-46mm", new Vector2(100f, 150f));
        }

        public override void SetMockupsSizes()
        {
            mockup.mockupsTextureSize.Add("apple-watch", new Vector2(486f, 870f));
            mockup.mockupsTextureSize.Add("apple-watch4", new Vector2(556f, 944f));
            mockup.mockupsTextureSize.Add("motorola-moto360-42mm", new Vector2(422f, 597f));
            mockup.mockupsTextureSize.Add("motorola-moto360-46mm", new Vector2(422f, 638f));
        }

        public override void SetMockupsAspects()
        {
            mockup.mockupsAspects.Add("apple-watch", "4:5");
            mockup.mockupsAspects.Add("apple-watch4", "13:16");
            mockup.mockupsAspects.Add("motorola-moto360-42mm", "1:1");
            mockup.mockupsAspects.Add("motorola-moto360-46mm", "1:1");
        }

        public override void SetMockupsScreenSizes()
        {
            mockup.mockupsScreenSize.Add("apple-watch", new Vector2(65.19f, 45.82f));
            mockup.mockupsScreenSize.Add("apple-watch4", new Vector2(82.83f, 58.41f));
            mockup.mockupsScreenSize.Add("motorola-moto360-42mm", new Vector2(86.78f, 55.77f));
            mockup.mockupsScreenSize.Add("motorola-moto360-46mm", new Vector2(86.78f, 52.25f));
        }

        public override void SetMockupsScreenOffsets()
        {
            mockup.mockupsScreenOffset.Add("apple-watch4", new Vector2(-13f, 0f));
            mockup.mockupsScreenOffset.Add("motorola-moto360-42mm", new Vector2(0f, 19f));
            mockup.mockupsScreenOffset.Add("motorola-moto360-46mm", new Vector2(0f, 18f));
        }
    }
}