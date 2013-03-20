using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace BumblebeeIOS
{
    public class IOSCapabilities : DesiredCapabilities
    {
        public static IOSCapabilities Iphone(string bundleName, string bundleVersion)
        {
            IOSCapabilities res = new IOSCapabilities();
            res.SetCapability(DEVICE, 1);
            res.SetCapability(LANGUAGE, "en");
            res.SetCapability(LOCALE, "en_GB");
            res.SetCapability(BUNDLE_NAME, bundleName);
            res.SetCapability(BUNDLE_VERSION, bundleVersion);
            return res;
        }

        public static IOSCapabilities MobileSafariIpad()
        {
            return IOSCapabilities.Ipad("Safari");
        }

        public static IOSCapabilities Iphone(string bundleName)
        {
            IOSCapabilities res = new IOSCapabilities();
            res.SetCapability(DEVICE, "iphone");
            res.SetCapability(LANGUAGE, "en");
            res.SetCapability(LOCALE, "en_GB");
            res.SetCapability(BUNDLE_NAME, bundleName);
            return res;
        }

        public static IOSCapabilities Ipad(string bundleName)
        {
            IOSCapabilities res = new IOSCapabilities();
            res.SetCapability(DEVICE, "ipad");
            res.SetCapability(LANGUAGE, "en");
            res.SetCapability(LOCALE, "en_GB");
            res.SetCapability(BUNDLE_NAME, bundleName);
            return res;
        }

        public IOSCapabilities()
        {
            SetCapability(TIME_HACK, false);
            SetCapability(VARIATION, "Regular");
            SetCapability(SIMULATOR, true);
        }

        public IOSCapabilities(string obj)
        {
            var json = new JavaScriptSerializer().Deserialize<Dictionary<string, object>>(obj);

            foreach (KeyValuePair<string, object> kvp in json)
            {
                SetCapability(kvp.Key, kvp.Value);
            }
        }

        public string getBundleName()
        {
            return (string)GetCapability(BUNDLE_NAME);
        }


        // UIAutomation properties called from instuments
        // UIAAplication.bundleID();
        // UIATarget.systemName();
        public static string UI_SYSTEM_NAME = "systemName";
        // UIATarget.systemVersion();
        public static string UI_SDK_VERSION = "sdkVersion";
        // UIATarget.name();
        public static string UI_NAME = "name";
        // UIAAplication.bundleVersion();
        public static string UI_BUNDLE_VERSION = "bundleVersion";
        // UIAAplication.version();
        public static string UI_VERSION = "version";

        // plist + envt variable
        public static string DEVICE = "device";
        public static string VARIATION = "variation";
        public static string SIMULATOR = "simulator";

        public static string IOS_SWITCHES = "ios.switches";
        public static string LANGUAGE = "language";
        public static string SUPPORTED_LANGUAGES = "supportedLanguages";
        public static string SUPPORTED_DEVICES = "supportedDevices";
        public static string LOCALE = "locale";
        public static string AUT = "aut";
        public static string TIME_HACK = "timeHack";

        public static string BUNDLE_VERSION = "CFBundleVersion";
        public static string BUNDLE_ID = "CFBundleIdentifier";
        public static string BUNDLE_SHORT_VERSION = "CFBundleShortVersionString";
        public static string BUNDLE_DISPLAY_NAME = "CFBundleDisplayName";
        public static string BUNDLE_NAME = "CFBundleName";
        public static string DEVICE_FAMILLY = "UIDeviceFamily";
        // http://developer.apple.com/library/ios/#documentation/general/Reference/InfoPlistKeyReference/Articles/iPhoneOSKeys.html
        public static string ICON = "CFBundleIconFile";

        public static string MAGIC_PREFIX = "plist_";
    }
}
