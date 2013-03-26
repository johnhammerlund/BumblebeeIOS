using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bumblebee.Interfaces;

namespace BumblebeeIOS.Implementation
{
    public abstract class PinchAction<TParent> where TParent : IBlock
    {
        protected int CenterX;
        protected int CenterY;
        protected TParent Parent;

        protected PinchAction(TParent parent, Func<TParent, IHasBackingElement> getStartElement)
        {
            Point location = InnerConvenience.GetElementLocation(getStartElement.Invoke(parent).Tag);
            CenterX = location.X;
            CenterY = location.Y;
        }

        protected PinchAction(TParent parent, int centerY, int centerX)
        {
            CenterX = centerX;
            CenterY = centerY;
        }

        public TParent ToOffset(int points)
        {
            return ToOffset<TParent>(points);
        }

        public TCustomResult ToOffset<TCustomResult>(int points) where TCustomResult : IBlock
        {
            double offset = points / Math.Sqrt(2);

            int x = (int)(CenterX - offset);
            int y = (int)(CenterY - offset);

            PerformPinch(x, y);

            return Parent.Session.CurrentBlock<TCustomResult>();
        }
        public TParent ToElementBounds(Func<TParent, IHasBackingElement> getEndElement)
        {
            return ToElementBounds<TParent>(getEndElement);
        }

        public TCustomResult ToElementBounds<TCustomResult>(Func<TParent, IHasBackingElement> getEndElement)
            where TCustomResult : IBlock
        {
            Point location = InnerConvenience.GetElementOrigin(getEndElement.Invoke(Parent).Tag);

            PerformPinch(location.X, location.Y);
            return Parent.Session.CurrentBlock<TCustomResult>();
        }

        protected abstract void PerformPinch(int offsetX, int offsetY);
    }
}
