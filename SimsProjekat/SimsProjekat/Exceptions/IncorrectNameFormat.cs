using System;
using System.Collections.Generic;
using System.Text;

namespace SimsProjekat.Exceptions
{
    class IncorrectNameFormat : System.Exception
    {
        public IncorrectNameFormat()
        {

        }

        public IncorrectNameFormat(string message) : base(message)
        {

        }

        public IncorrectNameFormat(string message, System.Exception inner) : base(message, inner)
        {

        }
    }
}
