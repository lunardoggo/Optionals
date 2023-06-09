﻿namespace LunarDoggo.Optionals.Tests
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

            Assertions.AssertOptionalMessage(optional.SafeMap<string, AggregateException>(_str => _str));
            Assertions.AssertOptionalMessage(optional.SafeMap(_str => _str));
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

            Assertions.AssertOptionalMessage(optional.SafeFlatMap<int, AggregateException>(_i => Optional.OfValue(_i)));
            Assertions.AssertOptionalMessage(optional.SafeFlatMap(_i => Optional.OfValue(_i)));
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
            Assert.Throws<ArgumentException>(() => optional.OrElseThrow(new ArgumentException("Exception")));
        }

        [Fact]
        public void TestIfHasMethods()
        {
            const int targetValue = 123;
            IOptional<int> optional = Optional.OfMessage<int>("Some message");
            int value = -1;

            Assert.Throws<ArgumentNullException>(() => optional.IfHasException(null));
            Assert.Throws<ArgumentNullException>(() => optional.IfHasMessage(null));
            Assert.Throws<ArgumentNullException>(() => optional.IfHasValue(null));

            Assertions.AssertOptionalMessage(optional);

            optional.IfHasValue(_value => value = _value);
            Assert.Equal(-1, value);

            optional.IfHasException(_ex => value = targetValue);
            Assert.Equal(-1, value);

            optional.IfHasMessage(_msg => value = targetValue);
            Assert.Equal(targetValue, value);
        }
    }
}
