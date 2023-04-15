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

        [Fact]
        public void TestApply()
        {
            const int updatedValue = 111;
            const int value = 123;
            IntReference reference = new IntReference(value);
            IOptional<IntReference> optional = Optional.OfValue(reference);

            Assert.Throws<ArgumentNullException>(() => optional.Apply(null));

            Assertions.AssertOptionalValue(optional, reference);
            Assert.Equal(value, reference.Value);

            optional.Apply(_ref => _ref.Value = updatedValue);

            Assertions.AssertOptionalValue(optional, reference);
            Assert.Equal(updatedValue, reference.Value);
        }

        [Fact]
        public void TestToString()
        {
            const int value = 123;
            IOptional<int> optional = Optional.OfValue(value);

            Assert.Throws<ArgumentNullException>(() => optional.ToString(null));
            Assert.Equal("11", optional.ToString(_value => BitConverter.ToString(new byte[] { (byte)(_value % 10) })));
        }
    }
}
