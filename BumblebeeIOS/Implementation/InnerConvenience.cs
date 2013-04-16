using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using OpenQA.Selenium;
using BumblebeeIOS.Extensions;

namespace BumblebeeIOS.Implementation
{
    class InnerConvenience
    {
        public static Point GetElementLocation(IWebElement element)
        {
            Point location = GetElementOrigin(element);
            Size size = GetElementSize(element);

            return new Point(location.X + size.Width / 2, location.Y + size.Height / 2);
        }

        public static Point GetElementOrigin(IWebElement element)
        {
            var serializer = new JavaScriptSerializer();

            var dict = serializer.Deserialize<Dictionary<string, object>>(serializer.Serialize(element.GetAttribute<Dictionary<string, object>>("rect")));

            int x = int.Parse(((Dictionary<string, object>)dict["origin"])["x"].ToString());
            int y = int.Parse(((Dictionary<string, object>)dict["origin"])["y"].ToString());

            return new Point(x, y);
        }

        public static Size GetElementSize(IWebElement element)
        {
            var serializer = new JavaScriptSerializer();

            var dict = serializer.Deserialize<Dictionary<string, object>>(serializer.Serialize(element.GetAttribute<Dictionary<string, object>>("rect")));

            return new Size(int.Parse(((Dictionary<string, object>)dict["size"])["width"].ToString()),
                            int.Parse(((Dictionary<string, object>)dict["size"])["height"].ToString()));
        }

        public static void ClickAtLocation(IWebDriver driver, int xPoint, int yPoint)
        {
            IWebElement UIAWindow = driver.FindElement(ByIOS.ClassName("UIAWindow"));
            Size windowSize = GetElementSize(UIAWindow);

            double xPts = (double)xPoint / windowSize.Width;
            double yPts = (double)yPoint / (windowSize.Height + 20);

            ((IJavaScriptExecutor) driver).ExecuteScript("UIATarget.localTarget().frontMostApp().tapWithOptions({'tapOffset':{'x':" +
                                                         xPts + ",'y':" + yPts + "}});");
        }
    }
}
