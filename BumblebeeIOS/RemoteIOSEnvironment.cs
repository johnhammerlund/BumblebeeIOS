using System;
using Bumblebee.Setup;
using OpenQA.Selenium;
using Regression.iOS.Tests;
using TestGallioProj.RemoteWebDriverBridge;
using org.uiautomation.ios;
using java.net;
using org.json;
using IOSCapabilities = Regression.iOS.Tests.IOSCapabilities;

namespace BumblebeeIOS
{
    public class RemoteIOSEnvironment : IDriverEnvironment
    {
        private string _address;
        private string _bundleName;

        public RemoteIOSEnvironment(string address, string bundleName)
        {
            _address = address;
            _bundleName = bundleName;
        }

        public IWebDriver CreateWebDriver()
        {
            var driver = new JavaDriver(_address, IOSCapabilities.Iphone(_bundleName));
            
            return driver;
        }
    }
}
