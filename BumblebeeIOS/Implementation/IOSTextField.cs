using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bumblebee.Implementation;
using Bumblebee.Interfaces;
using OpenQA.Selenium;

namespace BumblebeeIOS.Implementation
{
    public class IOSTextField<TResult> : TextField<TResult> where TResult : IBlock
    {
        public IOSTextField(IBlock parent, By @by) : base(parent, @by)
        {
        }

        public IOSTextField(IBlock parent, IWebElement element) : base(parent, element)
        {
        }

        public override TResult EnterText(string text)
        {
            Tag.Click();

            Tag.Clear();

            ((IJavaScriptExecutor)ParentBlock.Session.Driver).ExecuteScript("UIATarget.localTarget().frontMostApp().keyboard().typeString(\"" + text + "\");");

            return Session.CurrentBlock<TResult>(ParentBlock.Tag);
        }

        public override TResult AppendText(string text)
        {
            Tag.Click();

            ((IJavaScriptExecutor)ParentBlock.Session.Driver).ExecuteScript("UIATarget.localTarget().frontMostApp().keyboard().typeString(\"" + text + "\");");

            return Session.CurrentBlock<TResult>(ParentBlock.Tag);
        }
    }
}
