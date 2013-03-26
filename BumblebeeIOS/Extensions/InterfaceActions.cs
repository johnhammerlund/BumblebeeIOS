using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumblebee.Implementation;
using Bumblebee.Interfaces;
using BumblebeeIOS.Implementation;
using BumblebeeIOS.UIAObjects;
using OpenQA.Selenium;

namespace BumblebeeIOS.Extensions
{
    public static class InterfaceActions
    {
        public static PinchAction<TParent> PinchOpenFrom<TParent>(this TParent result,
                                                                  Func<TParent, IHasBackingElement> getStartElement)
            where TParent : IBlock
        {
            return new ZoomInAction<TParent>(result, getStartElement);
        }

        public static PinchAction<TParent> PinchOpenFrom<TParent>(this TParent result, int offsetX, int offsetY)
            where TParent : IBlock
        {
            return new ZoomInAction<TParent>(result, offsetX, offsetY);
        }

        public static PinchAction<TParent> PinchCloseFrom<TParent>(this TParent result,
                                                                     Func<TParent, IHasBackingElement> getStartElement)
            where TParent : IBlock
        {
            return new ZoomOutAction<TParent>(result, getStartElement);
        }

        public static PinchAction<TParent> PinchCloseFrom<TParent>(this TParent result, int offsetX, int offsetY)
            where TParent : IBlock
        {
            return new ZoomOutAction<TParent>(result, offsetX, offsetY);
        }

        public static TResult SetDeviceOrientation<TResult>(this TResult result, Orientation orientation) where TResult : IBlock
        {
            ((IJavaScriptExecutor)result.Session.Driver).ExecuteScript("UIATarget.localTarget().setDeviceOrientation('" + orientation + "');");
            return result.Session.CurrentBlock<TResult>();
        }

        public static TResult Shake<TResult>(this TResult result) where TResult : IBlock
        {
            ((IJavaScriptExecutor)result.Session.Driver).ExecuteScript("UIATarget.localTarget().shake()");
            return result.Session.CurrentBlock<TResult>();
        }

        public static TResult TapWithFingers<TResult>(this IClickable clickable, int numberOfFingers,
                                                                  int duration = 0)
            where TResult : IBlock
        {
            return TapWithFingers<TResult>(clickable, numberOfFingers, duration);
        }

        public static IBlock TapWithFingers<TResult>(this IClickable<TResult> clickable, int numberOfFingers, int duration = 0) 
            where TResult : IBlock
        {
            if (numberOfFingers < 1 || numberOfFingers > 5)
                throw new ArgumentException("numberOfFingers must be between 1 and 5 (duh!)");

            Point location = InnerConvenience.GetElementLocation(clickable.Tag);
            ((IJavaScriptExecutor)clickable.Session.Driver).ExecuteScript("UIATarget.localTarget().tapWithOptions({" +
                                                                           "'x': " + location.X + ", " +
                                                                           "'y': " + location.Y + "}, {" +
                                                                           "'tapCount': 1, " +
                                                                           "'touchCount': " + numberOfFingers + ", " +
                                                                           "'duration': " + duration + "});");
            return clickable.Session.CurrentBlock<TResult>();
        }
    }
}
