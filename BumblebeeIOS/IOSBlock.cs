using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
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

        public IOSPlatform PlatformType
        {
            get
            {
                string variation =
                    (string)((IJavaScriptExecutor) Session.Driver).ExecuteScript("return UIATarget.localTarget().model();");
                switch (variation)
                {
                    case "iPhone Simulator":
                        return IOSPlatform.IPhoneSimulator;
                    case "iphoneos":
                        return IOSPlatform.IPhone;
                    case "iPad Simulator":
                        return IOSPlatform.IPadSimulator;
                    case "ipados":
                        return IOSPlatform.IPad;
                    default:
                        throw new NotFoundException("You're likely using a new device that has not been integrated into the enum quite yet. " +
                                                    "Platform type: " + variation);
                }
            }
        }
    }
}
