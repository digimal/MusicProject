using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Dal.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    internal sealed class OrderAttribute : Attribute
    {
        private readonly int order_;
        public OrderAttribute([CallerLineNumber] int order = 0)
        {
            order_ = order;
        }

        public int Order { get { return order_; } }
    }
}
