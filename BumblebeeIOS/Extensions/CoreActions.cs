using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumblebee.Interfaces;
using Bumblebee.Setup;
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
    }
}
