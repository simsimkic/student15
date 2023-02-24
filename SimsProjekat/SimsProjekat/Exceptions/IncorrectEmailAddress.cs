using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.Exceptions
{
    class IncorrectEmailAddress : System.Exception
    {
        public IncorrectEmailAddress()
        {

        }

        public IncorrectEmailAddress(string message) : base(message)
        {

        }

        public IncorrectEmailAddress(string message, System.Exception inner) : base(message, inner)
        {

        }
    }
}
