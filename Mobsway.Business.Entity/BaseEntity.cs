using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mobsway.Business.Entity
{
    public abstract class BaseEntity
    {

        private List<Error> errors = null;
        protected List<Error> Errors
        {
            get
            {
                if (errors == null)
                    errors = new List<Error>();

                return errors;
            }
        }

    }
}
