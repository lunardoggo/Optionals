using System;
using System.Collections.Generic;
using System.Text;

namespace LunarDoggo.Optionals
{
    internal class Messages
    {
        public const string OptionalNoValue = "This Optional does not contain a value";
        public const string OptionalNoMessage = "This Optional does not contain a message";
        public const string OptionalNoException = "This Optional does not contain an Exception";

        public const string MappingMapperNull = "The provided mapping function must not be null";

        public const string ValueNull = "The Optional's value must not be null";
        public const string MessageNullOrEmpty = "The Optional's message must not be null or empty";
        public const string ExceptionNull = "The Optional's Exception must not be null";
        public const string ExceptionAndMessageNullOrEmpty = "The Optional's message and Exception must not be null or empty";
    }
}
