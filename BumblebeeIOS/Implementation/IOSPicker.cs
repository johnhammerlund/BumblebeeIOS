using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

    /// <summary>
    /// A normal IOSPickerWheel that uses UIAutomation to select a given value.
    /// </summary>
    /// <typeparam name="TResult">Standard Page Object return type</typeparam>
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

        protected virtual IOption<TResult> GetOption(IBlock parent, IWebElement tag, string value)
        {
            return new PickerOption<TResult>(parent, tag, value);
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

                return values.Select(val => GetOption(this, Tag, val.ToString()));
            }
        }
    }

    /// <summary>
    /// Good for year pickers, constructs options using only a segment of the existing (often >10000) options and uses
    /// UIAutomation to select a given value.
    /// </summary>
    /// <typeparam name="TResult">Standard Page Object return type</typeparam>
    public class OrderedIOSPickerWheel<TResult> : IOSPickerWheel<TResult> where TResult : IBlock
    {
        public OrderedIOSPickerWheel(IBlock parent, By @by) : base(parent, @by)
        {
        }

        public OrderedIOSPickerWheel(IBlock parent, IWebElement tag)
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

                IEnumerable<int> arr = values.Select(val => int.Parse(val.ToString()));
                int min = arr.First();
                int max = arr.Last();

                if (max - min > 60)
                {
                    int currentVal =
                        int.Parse((string)((IJavaScriptExecutor)Session.Driver).ExecuteScript("return arguments[0].value()", Tag));
                    max = currentVal + 30;
                    min = currentVal - 30;
                }

                for (int i = min; i < max; i++)
                {
                    yield return GetOption(this, Tag, i.ToString());
                }
            }
        }
    }

    /// <summary>
    /// Uses wheel traversal/comparison for finds, use only in special cases when Instruments does not properly populate the picker.
    /// </summary>
    /// <typeparam name="TResult">Standard Page Object return type</typeparam>
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

        protected override IOption<TResult> GetOption(IBlock parent, IWebElement tag, string value)
        {
            return new SafePickerOption<TResult>(this, tag, value);
        }
    }

    /// <summary>
    /// Uses wheel traversal/comparison for finds. USE AT YOUR OWN RISK as traversals may become infinite.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public class SafeOrderedIOSPickerWheel<TResult> : OrderedIOSPickerWheel<TResult> where TResult : IBlock
    {
        public SafeOrderedIOSPickerWheel(IBlock parent, By @by) : base(parent, @by)
        {
        }

        public SafeOrderedIOSPickerWheel(IBlock parent, IWebElement tag)
            : base(parent, tag)
        {
        }

        protected override IOption<TResult> GetOption(IBlock parent, IWebElement tag, string value)
        {
            return new SafeOrderedPickerOption<TResult>(parent, tag, value);
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

        public override string Text
        {
            get{return Option;}
        }

        protected IWebElement PickerTag { get; set; }

        protected string Option { get; set; }

        public virtual TResult Click()
        {
            return Click<TResult>();
        }

        public virtual TCustomResult Click<TCustomResult>() where TCustomResult : IBlock
        {
            var wasClicked = (bool)((IJavaScriptExecutor)Session.Driver).ExecuteScript
                    (@"try{
                                arguments[0].selectValue(arguments[1]);
                                return true;
                          } catch (e) {
                                return false;
                          }", PickerTag, Option);

            if(!wasClicked) throw new NoSuchElementException("Could not find option - " + Option);

            return Session.CurrentBlock<TCustomResult>();
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
            CenterOfWheel = InnerConvenience.GetElementLocation(PickerTag);
        }

        private void Initialize()
        {
            CurrentValue =
                ((string)((IJavaScriptExecutor) Session.Driver).ExecuteScript("return arguments[0].value", PickerTag)) ?? ""; //XXXX. Y of Z

            NumValues =
                (long)((IJavaScriptExecutor) Session.Driver).ExecuteScript("return arguments[0].values().length",
                                                                         PickerTag);
            Position =
                (long)((IJavaScriptExecutor) Session.Driver).ExecuteScript(
                        "for(var i=0;i<arguments[0].values().length;i++) if(arguments[0].values()[i]==arguments[1]) return i; return 0;", PickerTag, CurrentValue);

            OptionPosition = (long)((IJavaScriptExecutor)Session.Driver).ExecuteScript(
                        "for(var i=0;i<arguments[0].values().length;i++) if(arguments[0].values()[i].indexOf(arguments[1])==0) return i; return 0;", PickerTag, Option);
        }

        private string CurrentValue { get; set; }

        private long NumValues { get; set; }

        private long Position { get; set; }

        private long OptionPosition { get; set; }

        protected Point CenterOfWheel { get; set; }

        private bool TraverseUp()
        {
            CurrentValue =
                    ((string)((IJavaScriptExecutor)Session.Driver).ExecuteScript("return arguments[0].value()", PickerTag)) ?? "";

            if (CurrentValue.IndexOf(Option, StringComparison.Ordinal) == 0)
                return true;

            if (Position > 1)
            {
                Position--;

                InnerConvenience.ClickAtLocation(Session.Driver, CenterOfWheel.X, CenterOfWheel.Y - 50);
                Thread.Sleep(500);

                return TraverseUp();
            }

            return false;
        }

        private bool TraverseDown()
        {
            CurrentValue =
                    (string)(((IJavaScriptExecutor)Session.Driver).ExecuteScript("return arguments[0].value()", PickerTag)) ?? "";

            if (CurrentValue.IndexOf(Option, StringComparison.Ordinal) == 0)
                return true;

            if (Position < NumValues)
            {
                Position++;

                InnerConvenience.ClickAtLocation(Session.Driver, CenterOfWheel.X, CenterOfWheel.Y + 50);
                Thread.Sleep(500);

                return TraverseDown();
            }

            return false;
        }
        
        public override TCustomResult Click<TCustomResult>()
        {
            Initialize();

            int distance = (int)OptionPosition - (int)Position;

            if (distance > 0)
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

            return Session.CurrentBlock<TCustomResult>();
        }
    }

    public class SafeOrderedPickerOption<TResult> : SafePickerOption<TResult> where TResult : IBlock
    {
        public SafeOrderedPickerOption(IBlock parent, By @by) : base(parent, @by)
        {
        }

        public SafeOrderedPickerOption(IBlock parent, IWebElement tag) : base(parent, tag)
        {
        }

        public SafeOrderedPickerOption(IBlock parent, IWebElement tag, string option) : base(parent, tag, option)
        {
        }

        public override TCustomResult Click<TCustomResult>()
        {
            var currentValue = (string)(((IJavaScriptExecutor)Session.Driver).ExecuteScript("return arguments[0].value()", PickerTag)) ?? "";

            int comparison = String.Compare(Option, currentValue, StringComparison.Ordinal);

            if (comparison > 0)
            {
                InnerConvenience.ClickAtLocation(Session.Driver, CenterOfWheel.X, CenterOfWheel.Y + 50);
                Thread.Sleep(500);

                return Click<TCustomResult>();
            }
            if (comparison < 0)
            {
                InnerConvenience.ClickAtLocation(Session.Driver, CenterOfWheel.X, CenterOfWheel.Y - 50);
                Thread.Sleep(500);

                return Click<TCustomResult>();
            }

            if (Option.Equals(currentValue))
                return Session.CurrentBlock<TCustomResult>();

            throw new NoSuchElementException("Could not find option - " + Option);
        }
    }
}
