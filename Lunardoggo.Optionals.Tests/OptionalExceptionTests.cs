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

            Assertions.AssertOptionalException(optional.SafeMap<string, AggregateException>(_str => _str));
            Assertions.AssertOptionalException(optional.SafeMap(_str => _str));
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

            Assertions.AssertOptionalException(optional.SafeFlatMap<int, AggregateException>(_i => Optional.OfValue(_i)));
            Assertions.AssertOptionalException(optional.SafeFlatMap(_i => Optional.OfValue(_i)));
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

        [Fact]
        public void TestAlternativeValues()
        {
            IOptional<int> optional = Optional.OfException<int>(new Exception("Some exception"));

            Assertions.AssertOptionalException(optional);
            Assert.Equal(321, optional.OrElse(() => 321));
            Assert.Equal(321, optional.OrElse(321));
            Assert.Throws<ArgumentException>(() => optional.OrElseThrow(new ArgumentException("Exception")));
        }

        [Fact]
        public void TestIfHasMethods()
        {
            this.RunTestIfHasMethods(Optional.OfException<int>(new Exception("Some exception")));
            this.RunTestIfHasMethods(Optional.OfException<int>(new Exception("Some exception"), "Some message"));
        }

        private void RunTestIfHasMethods(IOptional<int> optional)
        {
            const int targetValue = 123;
            int value = -1;

            Assert.Throws<ArgumentNullException>(() => optional.IfHasException(null));
            Assert.Throws<ArgumentNullException>(() => optional.IfHasMessage(null));
            Assert.Throws<ArgumentNullException>(() => optional.IfHasValue(null));

            Assertions.AssertOptionalException(optional);

            optional.IfHasValue(_value => value = _value);
            Assert.Equal(-1, value);

            optional.IfHasMessage(_msg => value = targetValue);
            Assert.Equal(targetValue, value);

            optional.IfHasMessage(_msg => value = -1);
            Assert.Equal(-1, value);

            optional.IfHasException(_ex => value = targetValue);
            Assert.Equal(targetValue, value);
        }
    }
}
