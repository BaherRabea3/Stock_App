using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockApp.Core.Exceptions
{
    public class FinhubException : Exception
    {
        public FinhubException() { }

        public FinhubException(string message) : base(message)
        {

        }
        public FinhubException(string message , Exception? innerException) : base(message , innerException)
        {
            
        }
    }
}
