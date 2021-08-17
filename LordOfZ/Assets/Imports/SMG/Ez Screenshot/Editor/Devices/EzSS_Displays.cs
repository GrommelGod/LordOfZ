using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG.EzScreenshot
{
    public class EzSS_Displays : EzSS_Devices
    {
        public override void SetMockups()
        {
            mockup.mockups.Add(EzSS_Mockup.Categories.display, new List<string>()
            {
                "apple-thunderboltDisplay",
                "dell-ultraSharpHd"
            });
        }

        public override void SetMockupsColors()
        {
            mockup.mockupsColors.Add("apple-thunderboltDisplay", new List<string>() { "silver" });
            mockup.mockupsColors.Add("dell-ultraSharpHd", new List<string>() { "silver" });
        }

        public override void SetMockupsMultiOriented()
        {
            mockup.mockupsMultiOriented.Add("apple-thunderboltDisplay", false);
            mockup.mockupsMultiOriented.Add("dell-ultraSharpHd", false);
        }

        public override void SetMockupsPreviewSizes()
        {
            mockup.mockupsPreviewSizes.Add("apple-thunderboltDisplay", new Vector2(200f, 150f));
            mockup.mockupsPreviewSizes.Add("dell-ultraSharpHd", new Vector2(200f, 150f));
        }

        public override void SetMockupsSizes()
        {
            mockup.mockupsTextureSize.Add("apple-thunderboltDisplay", new Vector2(2784f, 2124f));
            mockup.mockupsTextureSize.Add("dell-ultraSharpHd", new Vector2(5454f, 4367f));
        }

        public override void SetMockupsAspects()
        {
            mockup.mockupsAspects.Add("apple-thunderboltDisplay", "16:9");
            mockup.mockupsAspects.Add("dell-ultraSharpHd", "16:9");
        }

        public override void SetMockupsScreenSizes()
        {
            mockup.mockupsScreenSize.Add("apple-thunderboltDisplay", new Vector2(92.95f, 68.79f));
            mockup.mockupsScreenSize.Add("dell-ultraSharpHd", new Vector2(94.87f, 66.94f));
        }

        public override void SetMockupsScreenOffsets()
        {
            mockup.mockupsScreenOffset.Add("apple-thunderboltDisplay", new Vector2(0f, 228f));
            mockup.mockupsScreenOffset.Add("dell-ultraSharpHd", new Vector2(0f, 578f));
        }
    }
}