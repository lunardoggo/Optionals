namespace LunarDoggo.Optionals
{
    internal class Messages
    {
        public const string OptionalNoValue = "This Optional does not contain a value";
        public const string OptionalNoMessage = "This Optional does not contain a message";
        public const string OptionalNoException = "This Optional does not contain an Exception";

        public const string ToStringMapperNull = "The provided to string function must not be null";
        public const string MappingMapperNull = "The provided mapping function must not be null";
        public const string ApplyActionNull = "The provided action must not be null";

        public const string ValueNull = "The Optional's value must not be null";
        public const string MessageNullOrEmpty = "The Optional's message must not be null or empty";
        public const string ExceptionNull = "The Optional's Exception must not be null";
        public const string ExceptionAndMessageNullOrEmpty = "The Optional's message and Exception must not be null or empty";

        public const string FromCollectionNullOrEmpty = "The provided optional collection must not be null or empty";

        public const string ExtensionMethodTargetNull = "The provided optional must not be null";
        public const string ConverterFunctionNull = "The provided converter function must not be null";
        public const string FilterFunctionNull = "The provided filter function must not be null";
        public const string CollectionNotCastable = "The collection contained in the optional cannot be cast to the target type, use Optional.Convert instead";

        public const string AlternativeValueFunctionNull = "The provided function for generating an alternative value must not be null";
    }
}
