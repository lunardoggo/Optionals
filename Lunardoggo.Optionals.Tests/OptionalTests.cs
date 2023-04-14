namespace Lunardoggo.Optionals.Tests
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
    }
}
