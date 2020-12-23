using System;
using System.Collections.Generic;
using System.Text;

namespace CabInvoiceGeneratorProject
{
    class CabInvoiceException : Exception
    {
        public enum ExceptionType
        {
            INVALID_RIDE_TYPE,
            INVALID_DISTANCE,
            INVALID_TIME,
            INVALID_RIDES,
            NULL_RIDES,
            INVALID_USER_ID
        }

        ExceptionType type;
        public CabInvoiceException(ExceptionType type, string message) : base(message)
        {
            this.type = type;
        }
    }
}
