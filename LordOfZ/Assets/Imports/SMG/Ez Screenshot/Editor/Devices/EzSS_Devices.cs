using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SMG.EzScreenshot
{
    public class EzSS_Devices
    {
        public EzSS_Mockup mockup;

        public void Init(EzSS_Mockup mockup)
        {
            this.mockup = mockup; 
            SetMockups();
            SetMockupsColors();
            SetMockupsMultiOriented();
            SetMockupsPreviewSizes();
            SetMockupsSizes();
            SetMockupsAspects();
            SetMockupsScreenSizes();
            SetMockupsScreenOffsets();
        }

        public virtual void SetMockups() { }
        public virtual void SetMockupsColors() { }
        public virtual void SetMockupsMultiOriented() { }
        public virtual void SetMockupsPreviewSizes() { }
        public virtual void SetMockupsSizes() { }
        public virtual void SetMockupsAspects() { }
        public virtual void SetMockupsScreenSizes() { }
        public virtual void SetMockupsScreenOffsets() { }
    }
}