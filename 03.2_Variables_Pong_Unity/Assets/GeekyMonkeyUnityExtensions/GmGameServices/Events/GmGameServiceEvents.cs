using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekyMonkey
{
    public abstract class GmGameServicesEvent : GameEvent
    {
        public bool Success { get; set; }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public string ResponseText { get; set; }
    }

    public abstract class GmGameServicesEvent<T> : GmGameServicesEvent
    {
        public T Request { get; set; }
    }
}
