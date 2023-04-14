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
    }
}
