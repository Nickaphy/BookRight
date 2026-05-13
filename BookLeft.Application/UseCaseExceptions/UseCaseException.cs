using System;
using System.Collections.Generic;
using System.Text;

namespace BookRight.Application.UseCaseExceptions
{
    public class UseCaseException : Exception
    {
        public UseCaseException(string message) : base(message) { }

    }
}
