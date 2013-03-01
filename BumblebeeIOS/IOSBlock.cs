using Bumblebee.Implementation;
using Bumblebee.Setup;
using OpenQA.Selenium;

namespace BumblebeeIOS
{
    public abstract class IOSBlock : Block
    {
        protected IOSBlock(Session session) : base(session)
        {
            session.Driver.SwitchTo().DefaultContent();
        }
    }
}
