using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobsway.Business.Entity
{
    public class Error
    {

        public Exception Exception
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

    }
}
