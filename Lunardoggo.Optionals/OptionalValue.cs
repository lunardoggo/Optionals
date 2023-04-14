﻿using System;

namespace Lunardoggo.Optionals
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