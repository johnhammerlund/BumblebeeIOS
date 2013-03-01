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
            session.Driver.SwitchTo().Window("Web");
        }
    }
}
