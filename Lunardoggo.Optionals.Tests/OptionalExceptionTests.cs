namespace LunarDoggo.Optionals.Tests
{
    public class OptionalExceptionTests
    {
        [Fact]
        public void TestProperties()
        {
            Exception exception = new ArgumentException("Some exception");
            IOptional<int> optional = Optional.OfException<int>(exception);

            Assertions.AssertOptionalException(optional, exception.GetType());

            const string message = "Some message";
            IOptional<int> withMessage = Optional.OfException<int>(exception, message);
            Assertions.AssertOptionalException(withMessage, exception.GetType(), message);
        }

        [Fact]
        public void TestMapping()
        {
            Exception exception = new ArgumentException("Some exception");
            IOptional<string> optional = Optional.OfException<string>(exception);
            IOptional<int> mapped = optional.Map(_value => Int32.Parse(_value));

            Assertions.AssertOptionalException(mapped, exception.GetType());
            Assert.Throws<ArgumentNullException>(() => optional.Map<string>(null));
            Assert.Throws<ArgumentNullException>(() => optional.SafeMap<string, Exception>(null));
        }

        [Fact]
        public void TestFlatMapping()
        {
            Exception exception = new ArgumentException("Some exception");
            IOptional<int> optional = Optional.OfException<int>(exception);
            IOptional<int> mapped = optional.Map(_value => _value % 10);

            Assertions.AssertOptionalException(mapped, exception.GetType());
            Assert.Throws<ArgumentNullException>(() => optional.FlatMap<string>(null));
            Assert.Throws<ArgumentNullException>(() => optional.SafeFlatMap<string, Exception>(null));
        }

        [Fact]
        public void TestApply()
        {
            const string message = "Some message";
            IOptional<IntReference> optional = Optional.OfMessage<IntReference>(message);

            Assert.Throws<ArgumentNullException>(() => optional.Apply(null));

            Assertions.AssertOptionalMessage(optional, message);

            optional.Apply(_ref => _ref.Value = 123);

            Assertions.AssertOptionalMessage(optional, message);
        }

        [Fact]
        public void TestToString()
        {
            const string message = "Some message";
            IOptional<int> optional = Optional.OfException<int>(new ArgumentException(message));

            Assert.Throws<ArgumentNullException>(() => optional.ToString(null));
            Assert.Equal(message, optional.ToString(_value => BitConverter.ToString(new byte[] { (byte)(_value % 10) })));

            optional = Optional.OfException<int>(new ArgumentException("Some argument"), message);

            Assert.Throws<ArgumentNullException>(() => optional.ToString(null));
            Assert.Equal(message, optional.ToString(_value => BitConverter.ToString(new byte[] { (byte)(_value % 10) })));
        }
    }
}
