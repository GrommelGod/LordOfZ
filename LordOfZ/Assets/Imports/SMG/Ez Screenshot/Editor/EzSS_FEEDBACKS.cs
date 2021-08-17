namespace SMG.EzScreenshot.FEEDBACKS
{
    public class Titles
    {
        public const string attention = "Attention!";
        public const string wait = "Wait...";
    }

    public class Buttons
    {
        public const string close = "Close";
        public const string continune = "Continue";
        public const string cancel = "Cancel";
    }

    public class Template
    {
        public const string noTemplateLoaded = "No template loaded.";
        public const string changesAreBeingSaved = "All changes are being saved automatically.";
        public const string creteTemplateToContinue = "You must create at least one template to continue.";
        public const string loadTemplateToContinue = "You must load a template to continue.";
    }

    public class Configuration
    {
        public const string noNeedtoOrganizeCameras = "There's no need to organize the cameras because they will be sorted based on the depth.";
        public const string mustBrowseLocation = "You must browse the location";
        public const string ezScreenshotWindowMustBeOpened = "Ez Screenshot window must be opened to take the screenshot.";
        public const string screenCaptureWarning = "BETA! The screenshot will be capture using ScreenCapture. Dont't worry if you receive warnings and errors messages after taking the screenshot, because it's a Unity bug related to the ScreenCapture method.";
        public const string screenSpaceOverlayWarning = "Setting the cameras manually prevent the elements that are inside a Screen Spave - Overlay canvas to appear on the screenshot.";
    }

    public class Mockup
    {
        public const string definingMockTexture = "Defining mockup texture...";
        public const string definingMockupScreen = "Defining mockup screen area...";
        public const string errorDefiningMockupTexture = "Something went wrong while defining the mockup texture. Please, check your connection and try again. \n";
        public const string errorDefiningMockupScreen = "Something went wrong while defining the mockup screen area. Please, check your connection and try again. \n";
    }

    public class TakeScreenshot
    {
        public const string noCameraInsideArray = "There's no camera inside the cameras array.";
        public const string selectSaveAtLocation = "You must select the location to save the screenshot.";
        public const string takingScreenshot = "Taking the screenshot...";
        public const string gameViewWidthIsBigger = "The gameView width is bigger than the screenshot width.";
        public const string gameViewHightIsBigger = "The gameView height is bigger than the screenshot height.";
        public const string mockupWidthIsBigger = "The mockup width is bigger than the screenshot width.";
        public const string mockupHeightIsBigger = "The mockup height is bigger than the screenshot height.";
    }

    public class DataManager
    {

    }
}