using System;
using System.Collections.Generic;
using Bumblebee.Setup;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

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
            //var test = IOSCapabilities.Iphone(_bundleName);
            var json = new Dictionary<string, object>
                           {
                               {"simulator", false},
                               {"CFBundleName", _bundleName},
                               {"locale", "en_GB"},
                               {"variation", "Regular"},
                               {"timeHack", false},
                               {"device", "iphone"},
                               {"CFBundleVersion", "1.0"},
                               {"language", "en"}
                           };
            var raw = new DesiredCapabilities(json);
            return new RemoteWebDriver(new Uri(_address), raw);
        }
    }
}
