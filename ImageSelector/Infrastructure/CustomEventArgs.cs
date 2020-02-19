using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageSelector.Infrastructure
{
    public class CustomEventArgs<T> : EventArgs
    {
        public CustomEventArgs(T data)
        {
            this.Data = data;
        }

        public T Data { get; set; }
    }
}
