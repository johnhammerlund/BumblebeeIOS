using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumblebee.Interfaces;
using Bumblebee.Setup;
using BumblebeeIOS.Implementation;
using BumblebeeIOS.UIAObjects;
using OpenQA.Selenium;

namespace BumblebeeIOS.Extensions
{
    public static class CoreActions
    {
        public static TResult HideApp<TResult>(this IBlock result, int seconds) where TResult : IBlock
        {
            ((IJavaScriptExecutor)result.Session.Driver).ExecuteScript("UIATarget.localTarget().deactivateAppForDuration(" + seconds + ");");
            return result.Session.CurrentBlock<TResult>();
        }

        public static TResult LockPhone<TResult>(this IBlock result, int seconds) where TResult : IBlock
        {
            ((IJavaScriptExecutor)result.Session.Driver).ExecuteScript("UIATarget.localTarget().lockForDuration(" + seconds + ");");
            return result.Session.CurrentBlock<TResult>();
        }

        public static TResult SetLocation<TResult>(this TResult result, double latitude, double longitude)
            where TResult : IBlock
        {
            ((IJavaScriptExecutor) result.Session.Driver).ExecuteScript("UIATarget.localTarget()." +
                                                                        "setLocation({" +
                                                                        "'latitude': " + latitude + ", " +
                                                                        "'longitude': " + longitude + "});");
            return result.Session.CurrentBlock<TResult>();
        }

        public static TResult SetLocation<TResult>(this TResult result, double latitude, double longitude, LocationOptions locationOptions) 
            where TResult : IBlock
        {
            ((IJavaScriptExecutor)result.Session.Driver).ExecuteScript("UIATarget.localTarget()." +
                                                                       "setLocationWithOptions({" +
                                                                       "'latitude': " + latitude + ", " +
                                                                       "'longitude': " + longitude + "}, "+
                                                                       "{'altitude': " + locationOptions.Altitude + "}, " +
                                                                       "'horizontalAccuracy': " + locationOptions.HorizontalAccuracy + ", " +
                                                                       "'verticalAccuracy: " + locationOptions.VerticalAccuracy + ", " +
                                                                       "'course': " + locationOptions.CourseHeading + ", " +
                                                                       "'speed': " + locationOptions.Speed + "});");
            return result.Session.CurrentBlock<TResult>();
        }

        public static TResult ResignFirstResponder<TResult>(this TResult result, By @by)
            where TResult : IBlock
        {
            result.Session.Driver.FindElement(@by).Click();

            return result.Session.CurrentBlock<TResult>();
        }

        public static TResult ResignFirstResponder<TResult>(this TResult result, string byIOSName = "done")
            where TResult : IBlock
        {
            return ResignFirstResponder(result, ByIOS.Name(byIOSName));
        }
    }
}
