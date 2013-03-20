using Bumblebee.Implementation;
using Bumblebee.Interfaces;
using Bumblebee.Setup;
using BumblebeeIOS.Implementation;
using OpenQA.Selenium;

namespace BumblebeeIOS
{
    public abstract class IOSBlock : Block
    {
        protected IOSBlock(Session session) : base(session)
        {
            //session.Driver.SwitchTo().DefaultContent();
        }

        public override IPerformsDragAndDrop GetDragAndDropPerformer()
        {
            return new IOSDragAndDrop(Session.Driver);
        }
    }
}
