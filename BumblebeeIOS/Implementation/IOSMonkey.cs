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
        private DateTime _readyTime;

        private TimeSpan _interval;

        private bool _actionWasPerformed;

        private double _baseProbability;

        public IOSMonkey()
        {
            Logs = new List<string>();
            _baseProbability = 0;
            _readyTime = DateTime.Now;
            _interval = TimeSpan.FromSeconds(1);
        }

        public override void SetProbability(double probability)
        {
            Probability = _baseProbability = probability;
        }

        public override void VerifyState()
        {
            Block.VerifyMonkeyState();
        }
        
        public override void Invoke(IBlock block)
        {
            Block = block;

            if (DateTime.Now > _readyTime)
            {
                PerformRandomAction();

                if (_actionWasPerformed)
                {
                    try
                    {
                        VerifyState();
                    }
                    catch
                    {
                        Logs.Add("Monkey Verification failed at " + Block.GetType());
                        throw;
                    }
                    Probability = _baseProbability;
                    _readyTime = DateTime.Now + _interval;
                }
                else
                {
                    Probability += _baseProbability < 0.1 ? 0 : (1 - Probability) / 3;
                }
            }
        }

        public override void PerformRandomAction()
        {
            double random = new Random().NextDouble();

            if (random < _baseProbability)
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
                _actionWasPerformed = true;
                return;
            }
            Logs.Add("Monkey ignored at " + Block.GetType());
            _actionWasPerformed = false;
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
