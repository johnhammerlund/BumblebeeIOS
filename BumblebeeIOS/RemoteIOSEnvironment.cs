using System;
using Bumblebee.Setup;
using IosDriverJavaAdapter;
using OpenQA.Selenium;
using java.net;
using org.json;
using IOSCapabilities = IosDriverJavaAdapter.IOSCapabilities;

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
