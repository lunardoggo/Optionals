namespace LunarDoggo.Optionals.Tests
{
    public class OptionalMessageTests
    {
        [Fact]
        public void TestProperties()
        {
            const string message = "Some message";
            IOptional<int> optional = Optional.OfMessage<int>(message);

            Assertions.AssertOptionalMessage(optional, message);
        }

        [Fact]
        public void TestMapping()
        {
            const string message = "Some message";
            IOptional<string> optional = Optional.OfMessage<string>(message);
            IOptional<int> mapped = optional.Map(_value => Int32.Parse(_value));

            Assertions.AssertOptionalMessage(mapped, message);
            Assert.Throws<ArgumentNullException>(() => optional.Map<string>(null));
            Assert.Throws<ArgumentNullException>(() => optional.SafeMap<string, Exception>(null));
        }

        [Fact]
        public void TestFlatMapping()
        {
            const string message = "Some message";
            IOptional<int> optional = Optional.OfMessage<int>(message);
            IOptional<int> mapped = optional.Map(_value => _value % 10);

            Assertions.AssertOptionalMessage(mapped, message);
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
            IOptional<int> optional = Optional.OfMessage<int>(message);

            Assert.Throws<ArgumentNullException>(() => optional.ToString(null));
            Assert.Equal(message, optional.ToString(_value => BitConverter.ToString(new byte[] { (byte)(_value % 10) })));
        }

        [Fact]
        public void TestAlternativeValues()
        {
            IOptional<int> optional = Optional.OfMessage<int>("Some message");

            Assertions.AssertOptionalMessage(optional);
            Assert.Equal(321, optional.OrElse(() => 321));
            Assert.Equal(321, optional.OrElse(321));
        }
    }
}
