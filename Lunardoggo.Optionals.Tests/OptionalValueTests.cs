namespace LunarDoggo.Optionals.Tests
{
    public class OptionalValueTests
    {
        [Fact]
        public void TestProperties()
        {
            const string value = "Test value";
            IOptional<string> optional = Optional.OfValue(value);

            Assertions.AssertOptionalValue(optional, value);
        }

        [Fact]
        public void TestMapping()
        {
            IOptional<string> optional = Optional.OfValue("123");
            IOptional<int> mapped = optional.Map(_value => Int32.Parse(_value));

            Assertions.AssertOptionalValue(mapped, 123);
            Assert.Throws<FormatException>(() => optional.Map(_value => _value + "!").Map(_value => Int32.Parse(_value)));
            Assert.Throws<ArgumentNullException>(() => optional.Map<string>(null));
            Assert.Throws<ArgumentNullException>(() => optional.SafeMap<string, Exception>(null));

            IOptional<int> mappedException = optional.Map(_value => _value + "!").SafeMap<int, FormatException>(_value => Int32.Parse(_value));
            Assertions.AssertOptionalException(mappedException, typeof(FormatException));
        }

        [Fact]
        public void TestFlatMapping()
        {
            IOptional<int> optional = Optional.OfValue(123);
            IOptional<int> mapped = optional.Map(_value => _value % 10);

            Assertions.AssertOptionalValue(mapped, 3);
            Assert.Throws<DivideByZeroException>(() => optional.FlatMap(_value => Optional.OfValue(_value / 0)));
            Assert.Throws<ArgumentNullException>(() => optional.FlatMap<string>(null));
            Assert.Throws<ArgumentNullException>(() => optional.SafeFlatMap<string, Exception>(null));

            IOptional<int> mappedException = optional.SafeFlatMap<int, DivideByZeroException>(_value => Optional.OfValue(_value / 0));
            Assertions.AssertOptionalException(mappedException, typeof(DivideByZeroException));
        }
    }
}
