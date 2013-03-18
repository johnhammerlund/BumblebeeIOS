using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Bumblebee.Interfaces;
using OpenQA.Selenium;

namespace BumblebeeIOS.Implementation
{
    class IOSDragAndDrop : IPerformsDragAndDrop
    {
        public IWebDriver Driver { get; private set; }

        public IOSDragAndDrop(IWebDriver driver)
        {
            Driver = driver;
        }

        public void DragAndDrop(IWebElement drag, IWebElement drop)
        {
            DragAndDrop(GetElementLocation(drag).X, GetElementLocation(drag).Y,
                        GetElementLocation(drop).X, GetElementLocation(drop).Y);
        }

        public void DragAndDrop(IWebElement drag, int xDrop, int yDrop)
        {
            DragAndDrop(GetElementLocation(drag).X, GetElementLocation(drag).Y,
                        xDrop, yDrop);
        }

        public void DragAndDrop(int xDrag, int yDrag, int xDrop, int yDrop)
        {
            ((IJavaScriptExecutor)Driver).ExecuteScript("UIATarget.localTarget().dragFromToForDuration({'x':" + xDrag +
                                                        ", 'y':" + yDrag + 
                                                        "},{'x':" + xDrop + 
                                                        ",'y':" + yDrop + "},1);");
        }

        private Point GetElementLocation(IWebElement element)
        {
            var serializer = new JavaScriptSerializer();

            dynamic dict = serializer.DeserializeObject(element.GetAttribute("rect").Replace('=', ':'));

            int x = int.Parse(dict["origin"]["x"].ToString());
            int y = int.Parse(dict["origin"]["y"].ToString());
            int height = int.Parse(dict["size"]["height"].ToString());
            int width = int.Parse(dict["size"]["width"].ToString());

            return new Point(x + width / 2, y + height / 2);
        }
    }
}
