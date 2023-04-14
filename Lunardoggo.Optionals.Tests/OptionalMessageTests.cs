namespace Lunardoggo.Optionals.Tests
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
    }
}
