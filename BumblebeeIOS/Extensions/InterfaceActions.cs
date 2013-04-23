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
        public static TParent DragToTop<TParent>(this TParent parent, Func<TParent, IElement> getDraggable)
            where TParent : IBlock
        {
            IElement element = getDraggable.Invoke(parent);

            Point location = InnerConvenience.GetElementLocation(element.Tag);
            new IOSDragAndDrop(element.Session.Driver).DragAndDrop(element.Tag, 0, -1 * location.Y + 1);

            return element.Session.CurrentBlock<TParent>();
        }

        public static TParent DragToBottom<TParent>(this TParent parent, Func<TParent, IElement> getDraggable)
            where TParent : IBlock
        {
            IElement element = getDraggable.Invoke(parent);
            IWebElement window = element.Session.Driver.FindElement(ByIOS.ClassName("UIAWindow"));

            Point location = InnerConvenience.GetElementLocation(element.Tag);
            Size windowSize = InnerConvenience.GetElementSize(window);

            new IOSDragAndDrop(element.Session.Driver).DragAndDrop(element.Tag, 0, windowSize.Height - location.Y - 1);

            return element.Session.CurrentBlock<TParent>();
        }

        public static TParent DragToRight<TParent>(this TParent parent, Func<TParent, IElement> getDraggable)
            where TParent : IBlock
        {
            IElement element = getDraggable.Invoke(parent);
            IWebElement window = element.Session.Driver.FindElement(ByIOS.ClassName("UIAWindow"));

            Point location = InnerConvenience.GetElementLocation(element.Tag);
            Size windowSize = InnerConvenience.GetElementSize(window);

            new IOSDragAndDrop(element.Session.Driver).DragAndDrop(element.Tag, windowSize.Width - location.X, 0);

            return element.Session.CurrentBlock<TParent>();
        }

        public static TParent DragToLeft<TParent>(this TParent parent, Func<TParent, IElement> getDraggable)
            where TParent : IBlock
        {
            IElement element = getDraggable.Invoke(parent);

            Point location = InnerConvenience.GetElementLocation(element.Tag);
            new IOSDragAndDrop(element.Session.Driver).DragAndDrop(element.Tag, -1 * location.X + 1, 0);

            return element.Session.CurrentBlock<TParent>();
        }

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

        public static TResult TapWithFingers<TResult>(this IClickable<TResult> clickable, int numberOfFingers, int duration = 0) 
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
