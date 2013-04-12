using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumblebee.Implementation;
using Bumblebee.Interfaces;
using Bumblebee.Setup;
using OpenQA.Selenium;

namespace BumblebeeIOS.Implementation
{
    public class IOSPickerWheel<TResult> : Element, ISelectBox<TResult> where TResult : IBlock
    {
        public IOSPickerWheel(IBlock parent, By @by) : base(parent, @by)
        {
        }

        public IOSPickerWheel(IBlock parent, IWebElement tag)
            : base(parent, tag)
        {
        }


        public IEnumerable<IOption<TResult>> Options
        {
            get
            {
                var values = (IReadOnlyCollection<object>)((IJavaScriptExecutor) Session.Driver).ExecuteScript
                    (@"var wheel = arguments[0];
                       if (wheel.isValid()) {
                            return wheel.values();
                        } return null;", Tag);

                return values.Select(val => new PickerOption<TResult>(this, Tag, val.ToString()));
            }
        }
    }

    public class PickerOption<TResult> : Element, IOption<TResult> where TResult : IBlock
    {
        public PickerOption(IBlock parent, By @by) : base(parent, @by)
        {
        }

        public PickerOption(IBlock parent, IWebElement tag) : base(parent, tag)
        {
        }

        public PickerOption(IBlock parent, IWebElement tag, string option) : base(parent, tag)
        {
            PickerTag = parent.Tag;
            Option = option;
            Console.WriteLine("BLOOP");
        }

        private IWebElement PickerTag { get; set; }

        private string Option { get; set; }

        public TResult Click()
        {
            return Click<TResult>();
        }

        public TResult1 Click<TResult1>() where TResult1 : IBlock
        {
            Console.WriteLine(Option);

            var wasClicked = (bool)((IJavaScriptExecutor)Session.Driver).ExecuteScript
                    (@"var wheel = arguments[0];
                       var pickerItems = [];
                       if (wheel.isValid()) {

                            pickerItems = wheel.values();
                            for(var i=0; i<pickerItems.length; i++){
                                if(pickerItems[i] == arguments[1]){
                                    wheel.selectValue(arguments[1]);
                                    return true;
                                }
                            }       
                        } return false;", PickerTag, Option);

            if(!wasClicked) throw new NoSuchElementException("Could not find option - " + Option);

            return Session.CurrentBlock<TResult1>();
        }
    }
}
