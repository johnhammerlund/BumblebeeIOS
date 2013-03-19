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
            Point dragLocation = GetElementLocation(drag);
            Point dropLocation = GetElementLocation(drop);

            DragAndDrop(dragLocation.X, dragLocation.Y,
                        (dragLocation.X - dropLocation.X), (dragLocation.Y - dropLocation.Y));
        }

        public void DragAndDrop(IWebElement drag, int xDrop, int yDrop)
        {
            Point dragLocation = GetElementLocation(drag);

            DragAndDrop(dragLocation.X, dragLocation.Y,
                        xDrop, yDrop);
        }

        public void DragAndDrop(int xDrag, int yDrag, int xDrop, int yDrop)
        {
            Console.WriteLine("UIATarget.localTarget().dragFromToForDuration({'x':" + xDrag +
                                                        ", 'y':" + yDrag +
                                                        "},{'x':" + (xDrop + xDrag) +
                                                        ",'y':" + (yDrop + yDrag) + "},1);\n\n");

            ((IJavaScriptExecutor)Driver).ExecuteScript("UIATarget.localTarget().dragFromToForDuration({'x':" + xDrag +
                                                        ", 'y':" + yDrag + 
                                                        "},{'x':" + (xDrop + xDrag) + 
                                                        ",'y':" + (yDrop + yDrag) + "},1);");
        }

        private Point GetElementLocation(IWebElement element)
        {
            var serializer = new JavaScriptSerializer();


            var dict =
                (Dictionary<string, object>)serializer.DeserializeObject(element.GetAttribute("rect").Replace('=', ':'));

            int x = int.Parse(((Dictionary<string, object>)dict["origin"])["x"].ToString());
            int y = int.Parse(((Dictionary<string, object>)dict["origin"])["y"].ToString());
            int height = int.Parse(((Dictionary<string, object>)dict["size"])["height"].ToString());
            int width = int.Parse(((Dictionary<string, object>)dict["size"])["width"].ToString());

            return new Point(x + width / 2, y + height / 2);
        }
    }
}
