namespace Lunardoggo.Optionals.Tests
{
    public static class Assertions
    {
        public static void AssertOptionalValue<T>(IOptional<T> optional, T value)
        {
            Assert.True(optional.HasValue);
            Assert.False(optional.HasMessage);
            Assert.False(optional.HasException);

            Assert.Throws<NotSupportedException>(() => { Exception tmp = optional.Exception; });
            Assert.Throws<NotSupportedException>(() => { string tmp = optional.Message; });
            Assert.Equal(value, optional.Value);
        }

        public static void AssertOptionalMessage<T>(IOptional<T> optional, string message)
        {
            Assert.False(optional.HasValue);
            Assert.True(optional.HasMessage);
            Assert.False(optional.HasException);

            Assert.Throws<NotSupportedException>(() => { Exception tmp = optional.Exception; });
            Assert.Throws<NotSupportedException>(() => { T tmp = optional.Value; });
            Assert.Equal(message, optional.Message);
        }

        public static void AssertOptionalException<T>(IOptional<T> optional, Type exceptionType)
        {
            Assert.False(optional.HasValue);
            Assert.True(optional.HasMessage);
            Assert.True(optional.HasException);

            Assert.Throws<NotSupportedException>(() => { T tmp = optional.Value; });
            Assert.NotNull(optional.Exception);
            Assert.Equal(exceptionType, optional.Exception.GetType());
            Assert.NotNull(optional.Message);
            Assert.NotEmpty(optional.Message);
        }

        public static void AssertOptionalException<T>(IOptional<T> optional, Type exceptionType, string message)
        {
            Assert.False(optional.HasValue);
            Assert.True(optional.HasMessage);
            Assert.True(optional.HasException);

            Assert.Throws<NotSupportedException>(() => { T tmp = optional.Value; });
            Assert.NotNull(optional.Exception);
            Assert.Equal(exceptionType, optional.Exception.GetType());
            Assert.Equal(message, optional.Message);
        }
    }
}
