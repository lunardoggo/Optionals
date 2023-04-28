using System;

namespace LunarDoggo.Optionals
{
    internal class OptionalValue<T> : IOptional<T>
    {
        private readonly T value;

        internal OptionalValue(T value)
        {
            this.value = value;
        }

        public bool HasValue { get => true; }

        public T Value { get => this.value; }

        public bool HasMessage { get => false; }

        public string Message => throw new NotSupportedException(Messages.OptionalNoMessage);

        public bool HasException { get => false; }

        public Exception Exception => throw new NotSupportedException(Messages.OptionalNoException);

        public IOptional<S> FlatMap<S>(Func<T, IOptional<S>> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(Messages.MappingMapperNull);
            }
            return mapper.Invoke(this.value);
        }

        public IOptional<S> Map<S>(Func<T, S> mapper)
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(Messages.MappingMapperNull);
            }
            return Optional.OfValue(mapper.Invoke(this.value));
        }

        public IOptional<S> SafeFlatMap<S, V>(Func<T, IOptional<S>> mapper) where V : Exception
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(Messages.MappingMapperNull);
            }
            
            try
            {
                return mapper.Invoke(this.value);
            }
            catch(V ex)
            {
                return Optional.OfException<S>(ex);
            }
        }

        public IOptional<S> SafeMap<S, V>(Func<T, S> mapper) where V : Exception
        {
            if (mapper == null)
            {
                throw new ArgumentNullException(Messages.MappingMapperNull);
            }

            try
            {
                return Optional.OfValue(mapper.Invoke(this.value));
            }
            catch(V ex)
            {
                return Optional.OfException<S>(ex);
            }
        }

        public IOptional<T> Apply(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(Messages.ApplyActionNull);
            }
            action.Invoke(this.value);
            return this;
        }

        public string ToString(Func<T, string> valueToString)
        {
            if (valueToString == null)
            {
                throw new ArgumentNullException(Messages.ToStringMapperNull);
            }
            return valueToString.Invoke(this.value);
        }

        public T OrElse(Func<T> valueGetter)
        {
            if(valueGetter == null)
            {
                throw new ArgumentNullException(Messages.AlternativeValueFunctionNull);
            }
            return this.value;
        }

        public T OrElse(T value)
        {
            return this.value;
        }

        public IOptional<T> IfHasException(Action<Exception> action)
        {
            if(action == null)
            {
                throw new ArgumentNullException(Messages.IfHasExceptionActionNull);
            }

            return this;
        }

        public IOptional<T> IfHasMessage(Action<string> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(Messages.IfHasMessageActionNull);
            }

            return this;
        }

        public IOptional<T> IfHasValue(Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(Messages.IfHasValueActionNull);
            }

            action.Invoke(this.value);
            return this;
        }

        public IOptional<S> SafeFlatMap<S>(Func<T, IOptional<S>> mapper)
        {
            return this.SafeFlatMap<S, Exception>(mapper);
        }

        public IOptional<S> SafeMap<S>(Func<T, S> mapper)
        {
            return this.SafeMap<S, Exception>(mapper);
        }

        public T OrElseThrow(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(Messages.ThrowExceptionNull);
            }

            return this.value;
        }
    }
}
