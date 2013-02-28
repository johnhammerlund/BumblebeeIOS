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
    public class IOSBlock : Block
    {
        public IOSBlock(Session session) : base(session)
        {
            Tag = Session.Driver.FindElement(By.ClassName("UIAWindow"));
        }
    }
}
