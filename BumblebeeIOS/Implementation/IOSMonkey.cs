using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumblebee.Implementation;
using Bumblebee.Interfaces;
using Bumblebee.Setup;
using BumblebeeIOS.Extensions;
using OpenQA.Selenium;

namespace BumblebeeIOS.Implementation
{
    public class IOSMonkey : Monkey
    {
        public IOSMonkey()
        {
            Logs = new List<string>();
            Probability = 0;
        }

        public override void VerifyState()
        {
            Block.VerifyMonkeyState();
        }
        
        public override void Invoke(IBlock block)
        {
            Block = block;
            PerformRandomAction();
            
            try
            {
                VerifyState();
            } catch
            {
                Logs.Add("Monkey Verification failed at " + Block.GetType());
            }
        }

        public override void PerformRandomAction()
        {
            double random = new Random().NextDouble();

            if (random < Probability)
            {
                switch ((int)(random * 10) % 2)
                {
                    case 0:
                        LockPhone();
                        Logs.Add("Phone locked at " + Block.GetType());
                        break;
                    case 1:
                        HideApp();
                        Logs.Add("App hidden at " + Block.GetType());
                        break;
                }
            }
            else
            {
                Console.WriteLine("Monkey ignored at " + Block.GetType());
            }
        }

        protected void LockPhone()
        {
            ((IJavaScriptExecutor)Block.Session.Driver).ExecuteScript("UIATarget.localTarget().deactivateAppForDuration(2);");
        }

        protected void HideApp()
        {
            ((IJavaScriptExecutor)Block.Session.Driver).ExecuteScript("UIATarget.localTarget().lockForDuration(2);");
        }
    }
}
