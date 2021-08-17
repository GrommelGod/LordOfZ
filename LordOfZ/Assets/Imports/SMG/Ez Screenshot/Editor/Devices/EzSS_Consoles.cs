using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SMG.EzScreenshot
{
    public class EzSS_Consoles : EzSS_Devices
    {
        public override void SetMockups()
        {
            mockup.mockups.Add(EzSS_Mockup.Categories.console, new List<string>()
            {
                "nintendo-switch"
            });
        }

        public override void SetMockupsColors()
        {
            mockup.mockupsColors.Add("nintendo-switch", new List<string>() { "gray", "redBlue" });
        }

        public override void SetMockupsMultiOriented()
        {
            mockup.mockupsMultiOriented.Add("nintendo-switch", false);
        }

        public override void SetMockupsPreviewSizes()
        {
            mockup.mockupsPreviewSizes.Add("nintendo-switch", new Vector2(230f, 100f));
        }

        public override void SetMockupsSizes()
        {
            mockup.mockupsTextureSize.Add("nintendo-switch", new Vector2(1190f, 513f));
        }

        public override void SetMockupsAspects()
        {
            mockup.mockupsAspects.Add("nintendo-switch", "16:9");
        }

        public override void SetMockupsScreenSizes()
        {
            mockup.mockupsScreenSize.Add("nintendo-switch", new Vector2(57.89f, 75.63f));
        }

        public override void SetMockupsScreenOffsets()
        {

        }
    }
}