using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
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

        public override IPerformsDragAndDrop GetDragAndDropPerformer()
        {
            return new IOSDragAndDrop(Session.Driver);
        }

        public virtual IEnumerable<IOption<TResult>> Options
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

    public class SafeIOSPickerWheel<TResult> : IOSPickerWheel<TResult> where TResult : IBlock
    {
        public SafeIOSPickerWheel(IBlock parent, By @by)
            : base(parent, @by)
        {
        }

        public SafeIOSPickerWheel(IBlock parent, IWebElement tag)
            : base(parent, tag)
        {
        }

        public override IEnumerable<IOption<TResult>> Options
        {
            get
            {
                var values = (IReadOnlyCollection<object>)((IJavaScriptExecutor)Session.Driver).ExecuteScript
                (@"var wheel = arguments[0];
                       if (wheel.isValid()) {
                            return wheel.values();
                        } return null;", Tag);

                return values.Select(val => new SafePickerOption<TResult>(this, Tag, val.ToString()));
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
        }

        protected IWebElement PickerTag { get; set; }

        protected string Option { get; set; }

        public virtual TResult Click()
        {
            return Click<TResult>();
        }

        public virtual TResult1 Click<TResult1>() where TResult1 : IBlock
        {
            var wasClicked = (bool)((IJavaScriptExecutor)Session.Driver).ExecuteScript
                    (@"var wheel = arguments[0];
                       var pickerItems = [];
                       if (wheel.isValid()) {

                            pickerItems = wheel.values();
                            for(var i=0; i<pickerItems.length; i++){
                                if(pickerItems[i].toString() == arguments[1].toString()){
                                    UIATarget.localTarget().delay(2);
                                    wheel.selectValue(pickerItems[i].toString());
                                    return true;
                                }
                            }       
                        } return false;", PickerTag, Option);

            if(!wasClicked) throw new NoSuchElementException("Could not find option - " + Option);

            return Session.CurrentBlock<TResult1>();
        }
    }

    public class SafePickerOption<TResult> : PickerOption<TResult> where TResult : IBlock
    {
        public SafePickerOption(IBlock parent, By @by) : base(parent, @by)
        {
        }

        public SafePickerOption(IBlock parent, IWebElement tag) : base(parent, tag)
        {
        }

        public SafePickerOption(IBlock parent, IWebElement tag, string option) : base(parent, tag, option)
        {
        }

        private void Initialize()
        {
            CurrentValue = PickerTag.GetAttribute("value"); //XXXX. Y of Z
            string YofZ = CurrentValue.Substring(CurrentValue.LastIndexOf(". ", StringComparison.Ordinal) + 1);
            string[] YandZ = YofZ.Split(new []{" of "}, StringSplitOptions.None);

            NumValues = int.Parse(YandZ[1]);
            Position = int.Parse(YandZ[0]);
        }

        private string CurrentValue { get; set; }

        private int NumValues { get; set; }

        private int Position { get; set; }

        private bool TraverseUp()
        {
            do
            {
                CurrentValue = PickerTag.GetAttribute("value");

                Console.WriteLine("Value: [" + CurrentValue.Substring(0, CurrentValue.LastIndexOf(".", StringComparison.Ordinal)) + "] Option: [" + Option + "]");

                if (CurrentValue.Substring(0, CurrentValue.LastIndexOf(".", StringComparison.Ordinal)).Equals(Option))
                    return true;

                new IOSDragAndDrop(Session.Driver).DragAndDrop(PickerTag, 0, 50);
                Thread.Sleep(500);

                Position--;
            } while (Position > 1);

            Position++;

            return false;
        }

        private bool TraverseDown()
        {
            do
            {
                CurrentValue = PickerTag.GetAttribute("value");

                Console.WriteLine("Value: [" + CurrentValue.Substring(0, CurrentValue.LastIndexOf(".", StringComparison.Ordinal)) + "] Option: [" + Option + "]");

                if (CurrentValue.Substring(0, CurrentValue.LastIndexOf(".", StringComparison.Ordinal)).Equals(Option))
                    return true;

                new IOSDragAndDrop(Session.Driver).DragAndDrop(PickerTag, 0, -50);
                Thread.Sleep(500);

                Position++;
            } while (Position < NumValues);

            Position--;

            return false;
        }

        public override TResult Click()
        {
            return Click<TResult>();
        }

        public override TResult1 Click<TResult1>()
        {
            Initialize();

            if (Position > NumValues/2)
            {
                if(!TraverseUp())
                    if(!TraverseDown())
                        throw new NoSuchElementException("Could not find option - " + Option);
            }
            else
            {
                if(!TraverseDown())
                    if(!TraverseUp())
                        throw new NoSuchElementException("Could not find option - " + Option);
            }

            return Session.CurrentBlock<TResult1>();
        }
    }
}
