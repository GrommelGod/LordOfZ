using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG.EzScreenshot
{
    public class EzSS_Computers : EzSS_Devices
    {
        public override void SetMockups()
        {
            mockup.mockups.Add(EzSS_Mockup.Categories.computer, new List<string>()
            {
                "apple-imac",
                "apple-imacPro",
                "apple-macbook",
                "apple-macbookPro",
                "dell-xps13",
                "dell-xps15"
            });
        }

        public override void SetMockupsColors()
        {
            mockup.mockupsColors.Add("apple-imac", new List<string>() { "silver" });
            mockup.mockupsColors.Add("apple-imacPro", new List<string>() { "spaceGray" });
            mockup.mockupsColors.Add("apple-macbook", new List<string>() { "gold", "silver", "spaceGray" });
            mockup.mockupsColors.Add("apple-macbookPro", new List<string>() { "silver", "spaceGray" });
            mockup.mockupsColors.Add("dell-xps13", new List<string>() { "silver" });
            mockup.mockupsColors.Add("dell-xps15", new List<string>() { "silver" });
        }

        public override void SetMockupsMultiOriented()
        {
            mockup.mockupsMultiOriented.Add("apple-imac", false);
            mockup.mockupsMultiOriented.Add("apple-imacPro", false);
            mockup.mockupsMultiOriented.Add("apple-macbook", false);
            mockup.mockupsMultiOriented.Add("apple-macbookPro", false);
            mockup.mockupsMultiOriented.Add("dell-xps13", false);
            mockup.mockupsMultiOriented.Add("dell-xps15", false);
        }

        public override void SetMockupsPreviewSizes()
        {
            mockup.mockupsPreviewSizes.Add("apple-imac", new Vector2(200f, 150f));
            mockup.mockupsPreviewSizes.Add("apple-imacPro", new Vector2(200f, 150f));
            mockup.mockupsPreviewSizes.Add("apple-macbook", new Vector2(250f, 150f));
            mockup.mockupsPreviewSizes.Add("apple-macbookPro", new Vector2(250f, 150f));
            mockup.mockupsPreviewSizes.Add("dell-xps13", new Vector2(250f, 150f));
            mockup.mockupsPreviewSizes.Add("dell-xps15", new Vector2(250f, 130f));
        }

        public override void SetMockupsSizes()
        {
            mockup.mockupsTextureSize.Add("apple-imac", new Vector2(5577f, 4610f));
            mockup.mockupsTextureSize.Add("apple-imacPro", new Vector2(2041f, 1693f));
            mockup.mockupsTextureSize.Add("apple-macbook", new Vector2(1916f, 1102f));
            mockup.mockupsTextureSize.Add("apple-macbookPro", new Vector2(2382f, 1379f));
            mockup.mockupsTextureSize.Add("dell-xps13", new Vector2(3834f, 2207f));
            mockup.mockupsTextureSize.Add("dell-xps15", new Vector2(5194f, 2645f));
        }

        public override void SetMockupsAspects()
        {
            mockup.mockupsAspects.Add("apple-imac", "16:10");
            mockup.mockupsAspects.Add("apple-imacPro", "16:10");
            mockup.mockupsAspects.Add("apple-macbook", "16:10");
            mockup.mockupsAspects.Add("apple-macbookPro", "16:10");
            mockup.mockupsAspects.Add("dell-xps13", "16:9");
            mockup.mockupsAspects.Add("dell-xps15", "16:9");
        }

        public override void SetMockupsScreenSizes()
        {
            mockup.mockupsScreenSize.Add("apple-imac", new Vector2(92.80f, 63.47f));
            mockup.mockupsScreenSize.Add("apple-imacPro", new Vector2(93.16f, 63.43f));
            mockup.mockupsScreenSize.Add("apple-macbook", new Vector2(76.15f, 82.66f));
            mockup.mockupsScreenSize.Add("apple-macbookPro", new Vector2(77.49f, 83.59f));
            mockup.mockupsScreenSize.Add("dell-xps13", new Vector2(84.46f, 82.55f));
            mockup.mockupsScreenSize.Add("dell-xps15", new Vector2(74.95f, 82.70f));
        }

        public override void SetMockupsScreenOffsets()
        {
            mockup.mockupsScreenOffset.Add("apple-imac", new Vector2(0f, 608f));
            mockup.mockupsScreenOffset.Add("apple-imacPro", new Vector2(0f, 236f));
            mockup.mockupsScreenOffset.Add("apple-macbook", new Vector2(0f, 20f));
            mockup.mockupsScreenOffset.Add("apple-macbookPro", new Vector2(0f, 29f));
            mockup.mockupsScreenOffset.Add("dell-xps13", new Vector2(0f, 142f));
            mockup.mockupsScreenOffset.Add("dell-xps15", new Vector2(0f, 173f));
        }
    }
}