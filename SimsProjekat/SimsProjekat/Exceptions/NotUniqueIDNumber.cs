using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.Exceptions
{
    class NotUniqueIDNumber : System.Exception
    {
        public NotUniqueIDNumber()
        {

        }

        public NotUniqueIDNumber(string message) : base(message)
        {

        }

        public NotUniqueIDNumber(string message, System.Exception inner) : base(message, inner)
        {

        }
    }
}
