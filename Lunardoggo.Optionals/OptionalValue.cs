using System;

namespace Lunardoggo.Optionals
{
    internal class OptionalValue<T> : IOptional<T>
    {
        internal OptionalValue(T value)
        {
        }

        public bool HasValue => throw new NotImplementedException();

        public T Value => throw new NotImplementedException();

        public bool HasMessage => throw new NotImplementedException();

        public string Message => throw new NotImplementedException();

        public bool HasException => throw new NotImplementedException();

        public Exception Exception => throw new NotImplementedException();

        public IOptional<T> Apply(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public IOptional<S> FlatMap<S>(Func<T, IOptional<S>> mapper)
        {
            throw new NotImplementedException();
        }

        public IOptional<S> Map<S>(Func<T, S> mapper)
        {
            throw new NotImplementedException();
        }

        public IOptional<T> SafeApply<S>(Action<T> action) where S : Exception
        {
            throw new NotImplementedException();
        }

        public IOptional<S> SafeFlatMap<S, V>(Func<T, IOptional<S>> mapper) where V : Exception
        {
            throw new NotImplementedException();
        }

        public IOptional<S> SafeMap<S, V>(Func<T, S> mapper) where V : Exception
        {
            throw new NotImplementedException();
        }

        public string ToString(Func<T, string> valueToString)
        {
            throw new NotImplementedException();
        }
    }
}
