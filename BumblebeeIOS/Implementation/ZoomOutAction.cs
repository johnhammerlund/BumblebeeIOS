using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumblebee.Interfaces;
using OpenQA.Selenium;

namespace BumblebeeIOS.Implementation
{
    class ZoomOutAction<TParent> : PinchAction<TParent> where TParent : IBlock
    {
        public ZoomOutAction(TParent parent, Func<TParent, IHasBackingElement> getStartElement)
            : base(parent, getStartElement)
        {
        }

        public ZoomOutAction(TParent parent, int centerY, int centerX) : base(parent, centerY, centerX)
        {
        }

        protected override void PerformPinch(int offsetX, int offsetY)
        {
            if (offsetX < CenterX || offsetY < CenterY)
                throw new InvalidOperationException("Invalid gesture bounds.");

            ((IJavaScriptExecutor)Parent.Session.Driver).ExecuteScript("UIATarget().localTarget()." +
                                                                        "pinchOpenFromToForDuration({" +
                                                                        "'x': " + CenterX + ", " +
                                                                        "'y': " + CenterY + "}, {" +
                                                                        "'x': " + offsetX + ", 'y': " + offsetY + "}, 1);");
        }
    }
}
