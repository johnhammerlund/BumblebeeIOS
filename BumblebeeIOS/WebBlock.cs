using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumblebee.Implementation;
using Bumblebee.Setup;
using OpenQA.Selenium;

namespace BumblebeeIOS
{
    public abstract class WebBlock : Block
    {
        protected WebBlock(Session session) : base(session)
        {
            // TODO Switch to web context

            // If we want we can put this here, otherwise we can leave it up to the
            // user to extend IOSBlock and set the default tag themselves
            Tag = Session.Driver.FindElement(By.TagName("body"));
        }
    }
}
