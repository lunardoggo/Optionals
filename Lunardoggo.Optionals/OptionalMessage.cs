using System;

namespace LunarDoggo.Optionals
{
    internal class OptionalMessage<T> : IOptional<T>
    {
        private readonly string message;

        internal OptionalMessage(string message)
        {
            this.message = message;
        }

        public bool HasValue { get => false; }

        public T Value => throw new NotSupportedException(Messages.OptionalNoValue);

        public bool HasMessage { get => true; }

        public string Message { get => this.message; }

        public bool HasException { get => false; }

        public Exception Exception => throw new NotSupportedException(Messages.OptionalNoException);

        public IOptional<S> FlatMap<S>(Func<T, IOptional<S>> mapper)
        {
            return this.FlatMapAny(mapper);
        }

        public IOptional<S> Map<S>(Func<T, S> mapper)
        {
            return this.MapAny(mapper);
        }

        public IOptional<S> SafeFlatMap<S, V>(Func<T, IOptional<S>> mapper) where V : Exception
        {
            return this.FlatMapAny(mapper);
        }

        public IOptional<S> SafeMap<S, V>(Func<T, S> mapper) where V : Exception
        {
            return this.MapAny(mapper);
        }

        private IOptional<S> FlatMapAny<S>(Func<T, IOptional<S>> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(Messages.MappingMapperNull);
            }

            return Optional.OfMessage<S>(this.message);
        }

        private IOptional<S> MapAny<S>(Func<T, S> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(Messages.MappingMapperNull);
            }

            return Optional.OfMessage<S>(this.message);
        }

        public IOptional<T> Apply(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public IOptional<T> SafeApply<S>(Action<T> action) where S : Exception
        {
            throw new NotImplementedException();
        }

        public string ToString(Func<T, string> valueToString)
        {
            throw new NotImplementedException();
        }
    }
}
