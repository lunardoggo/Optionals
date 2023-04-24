using System;

namespace LunarDoggo.Optionals
{
    internal class OptionalException<T> : IOptional<T>
    {
        private readonly Exception exception;
        private readonly string message;

        internal OptionalException(Exception exception, string message)
        {
            this.exception = exception;
            this.message = message;
        }

        public bool HasValue { get => false; }

        public T Value => throw new NotSupportedException(Messages.OptionalNoValue);

        public bool HasMessage { get => true; }

        public string Message { get => this.message; }

        public bool HasException { get => true; }

        public Exception Exception { get =>  this.exception; }

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
            if(mapper == null)
            {
                throw new ArgumentNullException(Messages.MappingMapperNull);
            }

            return Optional.OfException<S>(this.exception, this.message);
        }

        private IOptional<S> MapAny<S>(Func<T, S> mapper)
        {
            if(mapper == null)
            {
                throw new ArgumentNullException(Messages.MappingMapperNull);
            }

            return Optional.OfException<S>(this.exception, this.message);
        }

        public IOptional<T> Apply(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(Messages.ApplyActionNull);
            }
            return this;
        }

        public string ToString(Func<T, string> valueToString)
        {
            if (valueToString == null)
            {
                throw new ArgumentNullException(Messages.ToStringMapperNull);
            }
            return this.message;
        }

        public T OrElse(Func<T> valueGetter)
        {
            if (valueGetter == null)
            {
                throw new ArgumentNullException(Messages.AlternativeValueFunctionNull);
            }
            return valueGetter.Invoke();
        }

        public T OrElse(T value)
        {
            return value;
        }

        public IOptional<T> IfHasException(Action<Exception> action)
        {
            throw new NotImplementedException();
        }

        public IOptional<T> IfHasMessage(Action<string> action)
        {
            throw new NotImplementedException();
        }

        public IOptional<T> IfHasValue(Action<T> action)
        {
            throw new NotImplementedException();
        }
    }
}
