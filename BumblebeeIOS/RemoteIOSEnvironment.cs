using System;
using System.Collections.Generic;
using Bumblebee.Setup;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace BumblebeeIOS
{
    public class RemoteIOSEnvironment : IDriverEnvironment
    {
        private string _address;
        private Dictionary<string, object> _json; 


        /// <summary>
        /// Constructs a Bumblebee iOS test environment with minimum requirements.
        /// </summary>
        /// <param name="address">Grid node session address</param>
        /// <param name="bundleName">Name of the iOS Application</param>
        /// <param name="isIphone">True if using iPhone, false if using iPad</param>
        public RemoteIOSEnvironment(string address, string bundleName, bool isIphone = true) 
            : this(address, bundleName, "1.0")
        {
        }

        /// <summary>
        /// Constructs a Bumblebee iOS test environment with basic requirements.
        /// </summary>
        /// <param name="address">Grid node session address</param>
        /// <param name="bundleName">Name of the iOS Application</param>
        /// <param name="version">Specific version of the iOS Application</param>
        /// <param name="isIphone">True if using iPhone, false if using iPad</param>
        public RemoteIOSEnvironment(string address, string bundleName, string version, bool isIphone = true)
        {
            _address = address;

            _json = new Dictionary<string, object>
                           {
                               {"simulator", false},
                               {"CFBundleName", bundleName},
                               {"locale", "en_GB"},
                               {"variation", "Regular"},
                               {"timeHack", false},
                               {"device", isIphone ? "iphone" : "ipad"},
                               {"CFBundleVersion", version},
                               {"language", "en"}
                           };
        }

        /// <summary>
        /// Constructs a Bumblebee iOS test environment using user-defined DesiredCapabilities
        /// <para>See http://ios-driver.github.com/ios-driver-beta/native.html#params </para>
        /// </summary>
        /// <param name="jsonMap">JSON required to fulfill Capabilities for the grid protocol.</param>
        public RemoteIOSEnvironment(string address, Dictionary<string, object> jsonMap)
        {
            _address = address;
            _json = jsonMap;
        }


        public IWebDriver CreateWebDriver()
        {
            
            return new RemoteWebDriver(new Uri(_address), new DesiredCapabilities(_json));
        }

    }
}
