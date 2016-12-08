//
// Copyright - James Finlay
// 

using FishTank.Screens;
using System;
using System.Linq;
using System.Reflection;

namespace FishTank.Utilities.Events
{
    public class NavigationEventArgs : EventArgs
    {
        public Type Target { get; set; }

        public NavigationEventArgs(Type target)
        {
            if (!target.GetInterfaces().Contains(typeof(IScreen)))
            {
                throw new ArgumentException(string.Format("{0} does not inherit from {1}", nameof(target), nameof(IScreen)));
            }

            Target = target;
        }
    }
}
