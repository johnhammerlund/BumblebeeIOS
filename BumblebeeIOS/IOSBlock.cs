using Bumblebee.Implementation;
using Bumblebee.Setup;
using OpenQA.Selenium;

namespace BumblebeeIOS
{
    public abstract class IOSBlock : Block
    {
        protected IOSBlock(Session session) : base(session)
        {
            // TODO switch to IOS

            // If we want we can put this here, otherwise we can leave it up to the
            // user to extend IOSBlock and set the default tag themselves
            Tag = Session.Driver.FindElement(By.ClassName("UIAWindow"));
        }
    }
}
