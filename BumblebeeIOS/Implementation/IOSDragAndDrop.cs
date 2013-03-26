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
            Point dragLocation = InnerConvenience.GetElementLocation(drag);
            Point dropLocation = InnerConvenience.GetElementLocation(drop);

            DragAndDrop(dragLocation.X, dragLocation.Y,
                        (dragLocation.X - dropLocation.X), (dragLocation.Y - dropLocation.Y));
        }

        public void DragAndDrop(IWebElement drag, int xDrop, int yDrop)
        {
            Point dragLocation = InnerConvenience.GetElementLocation(drag);

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

    }
}
