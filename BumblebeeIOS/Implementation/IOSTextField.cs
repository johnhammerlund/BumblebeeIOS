using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public override TResult AppendText(string text)
        {
            var uiaScript = text.Aggregate("", 
                (current, c) => current + ("UIATarget.localTarget()frontMostApp().keyboard().buttons()[\"" + c + "\"].tap();"));


            ((IJavaScriptExecutor) ParentBlock.Session.Driver).ExecuteScript(uiaScript);

            return Session.CurrentBlock<TResult>(ParentBlock.Tag);
        }
    }
}
