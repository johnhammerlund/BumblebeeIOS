using System;
using Bumblebee.Setup;
using OpenQA.Selenium;
using Regression.iOS.Tests;
using TestGallioProj.RemoteWebDriverBridge;

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
            return new JavaDriver(_address, IOSCapabilities.Iphone(_bundleName));
        }
    }
}
