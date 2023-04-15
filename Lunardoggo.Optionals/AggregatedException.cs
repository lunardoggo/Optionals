using System.Collections.Generic;
using System.Linq;
using System;

namespace LunarDoggo.Optionals
{
    public class AggregatedException : Exception
    {
        public AggregatedException(IEnumerable<Exception> exceptions) : base($"{exceptions.Count()} exception(s) have occurred")
        {
            this.Exceptions = exceptions.ToArray();
        }

        public Exception[] Exceptions { get; }
    }
}
