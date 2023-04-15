namespace LunarDoggo.Optionals.Tests
{
    public class OptionalTests
    {
        [Fact]
        public void TestOptionalCreation()
        {
            Assert.Throws<ArgumentNullException>(() => Optional.OfValue<object>(null));
            Assert.Throws<ArgumentNullException>(() => Optional.OfMessage<object>(null));
            Assert.Throws<ArgumentNullException>(() => Optional.OfException<object>(null));
            Assert.Throws<ArgumentNullException>(() => Optional.OfException<object>(null, null));
            Assert.Throws<ArgumentNullException>(() => Optional.OfException<object>(new Exception(), null));
            Assert.Throws<ArgumentNullException>(() => Optional.OfException<object>(null, "Test"));

            const string message = "Test message";
            Exception exception = new NotImplementedException("Not implemented");

            Assertions.AssertOptionalValue(Optional.OfValue(123), 123);
            Assertions.AssertOptionalMessage(Optional.OfMessage<int>(message), message);
            Assertions.AssertOptionalException(Optional.OfException<int>(exception), typeof(NotImplementedException), exception.Message);
            Assertions.AssertOptionalException(Optional.OfException<int>(exception, message), typeof(NotImplementedException), message);
        }

        [Fact]
        public void TestCreateOfOptionals()
        {
            IOptional<int>[] optionals = new IOptional<int>[]
            {
                Optional.OfValue(123),
                Optional.OfValue(321),
                Optional.OfValue(111),
                Optional.OfMessage<int>("Some message 1"),
                Optional.OfMessage<int>("Some message 2"),
                Optional.OfException<int>(new ArgumentException("Some argument")),
                Optional.OfException<int>(new NullReferenceException("Something null"), "Some message")
            };

            Assert.Throws<ArgumentNullException>(() => Optional.OfOptionals<IOptional<int>, int>(null));
            Assert.Throws<ArgumentException>(() => Optional.OfOptionals<IOptional<int>, int>(new IOptional<int>[0]));

            Assertions.AssertOptionalException(Optional.OfOptionals<IOptional<int>, int>(optionals.Where(_optional => _optional.HasException).ToArray()));
            Assertions.AssertOptionalMessage(Optional.OfOptionals<IOptional<int>, int>(optionals.Where(_optional => _optional.HasMessage && !_optional.HasException).ToArray()));
            Assertions.AssertOptionalValue(Optional.OfOptionals<IOptional<int>, int>(optionals.Where(_optional => _optional.HasValue).ToArray()));

            IOptional<IEnumerable<int>> withExceptions = Optional.OfOptionals<IOptional<int>, int>(optionals);
            Assertions.AssertOptionalException<IEnumerable<int>>(withExceptions);
            Assert.NotNull(withExceptions.Exception);
            Assert.IsType<AggregatedException>(withExceptions.Exception);
            Assert.Equal(2, ((AggregatedException)withExceptions.Exception).Exceptions.Length);
            Assert.Equal($"Some argument{Environment.NewLine}Some message", withExceptions.Message);

            IOptional<IEnumerable<int>> withMessages = Optional.OfOptionals<IOptional<int>, int>(optionals.Where(_optional => !_optional.HasException).ToArray());
            Assertions.AssertOptionalMessage<IEnumerable<int>>(withMessages);
            Assert.Equal($"Some message 1{Environment.NewLine}Some message 2", withMessages.Message);

            IOptional<IEnumerable<int>> withValues = Optional.OfOptionals<IOptional<int>, int>(optionals.Where(_optional => _optional.HasValue).ToArray());
            Assertions.AssertOptionalValue(withValues);
            Assert.NotEmpty(withValues.Value);
            Assert.Equal(3, withValues.Value.Count());
            Assert.Contains(123, withValues.Value);
            Assert.Contains(321, withValues.Value);
            Assert.Contains(111, withValues.Value);
        }

        [Fact]
        public void TestFilter()
        {
            IOptional<IEnumerable<int>> ints = Optional.OfValue(new int[] { 1, 2, 30 } as IEnumerable<int>);

            Assert.Throws<ArgumentNullException>(() => Optional.Filter<int>(null, null));
            Assert.Throws<ArgumentNullException>(() => Optional.Filter<int>(null, _val => _val > 10));
            Assert.Throws<ArgumentNullException>(() => ints.Filter(null));

            IOptional<IEnumerable<int>> filtered = ints.Filter(_val => _val > 10);
            Assertions.AssertOptionalValue(filtered);
            Assert.Single(filtered.Value);
            Assert.Equal(30, filtered.Value.Single());
        }

        [Fact]
        public void TestForEach()
        {
            IOptional<IntReference[]> ints = Optional.OfValue(new IntReference[] { new IntReference(1), new IntReference(2), new IntReference(3) });

            Assert.Throws<ArgumentNullException>(() => Optional.ForEach<IntReference[], IntReference>(null, null));
            Assert.Throws<ArgumentNullException>(() => Optional.ForEach<IntReference[], IntReference>(null, _ref => _ref.Value = 10));
            Assert.Throws<ArgumentNullException>(() => ints.ForEach<IntReference[], IntReference>(null));

            Assert.DoesNotContain(ints.Value, _ref => _ref.Value == 10);

            ints.ForEach<IntReference[], IntReference>(_ref => _ref.Value = 10);

            Assertions.AssertOptionalValue(ints);
            Assert.DoesNotContain(ints.Value, _ref => _ref.Value != 10);
        }

        [Fact]
        public void TestConvert()
        {
            IOptional<int[]> ints = Optional.OfValue(new int[] { 1, 2, 3 });

            Assert.Throws<ArgumentNullException>(() => Optional.Convert<int[], List<int>, int>(null));

            IOptional<List<int>> converted = ints.Convert<int[], List<int>, int>();
            
            Assertions.AssertOptionalValue(converted);
            Assert.Equal(3, converted.Value.Count);
            Assert.Contains(converted.Value, _item => _item == 1);
            Assert.Contains(converted.Value, _item => _item == 2);
            Assert.Contains(converted.Value, _item => _item == 3);
        }
    }
}
