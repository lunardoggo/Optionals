using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using static System.Collections.Specialized.BitVector32;

namespace LunarDoggo.Optionals
{
    public static class Optional
    {
        /// <summary>
        /// Creates a new <see cref="IOptional{T}"/> containing the provided value
        /// </summary>
        /// <typeparam name="T">Type of the stored value</typeparam>
        /// <param name="value">Value to be stored</param>
        /// <returns><see cref="IOptional{T}"/> containing the provided value</returns>
        public static IOptional<T> OfValue<T>(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(Messages.ValueNull);
            }

            return new OptionalValue<T>(value);
        }

        /// <summary>
        /// Creates a new <see cref="IOptional{T}"/> containing the provided message
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IOptional{T}"/></typeparam>
        /// <param name="message">Message to be stored</param>
        /// <returns><see cref="IOptional{T}"/> containing the provided value</returns>
        public static IOptional<T> OfMessage<T>(string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException(Messages.MessageNullOrEmpty);
            }

            return new OptionalMessage<T>(message);
        }

        /// <summary>
        /// Creates a new <see cref="IOptional{T}"/> containing the provided <see cref="Exception"/>, <see cref="Exception.Message"/> is
        /// used for the message contained in the <see cref="IOptional{T}"/>
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IOptional{T}"/></typeparam>
        /// <param name="exception"><see cref="Exception"/> to be stored</param>
        /// <returns><see cref="IOptional{T}"/> containing the provided <see cref="Exception"/></returns>
        public static IOptional<T> OfException<T>(Exception exception)
        {
            if (exception == null)
            {
                throw new ArgumentNullException(Messages.ExceptionNull);
            }

            return Optional.OfException<T>(exception, exception.Message);
        }

        /// <summary>
        /// Creates a new <see cref="IOptional{T}"/> containing the provided <see cref="Exception"/> and custom message
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IOptional{T}"/></typeparam>
        /// <param name="exception"><see cref="Exception"/> to be stored</param>
        /// <param name="customMessage">message to be stored</param>
        /// <returns><see cref="IOptional{T}"/> containing the provided <see cref="Exception"/> and message</returns>
        public static IOptional<T> OfException<T>(Exception exception, string customMessage)
        {
            if (exception == null || String.IsNullOrEmpty(customMessage))
            {
                throw new ArgumentNullException(Messages.ExceptionAndMessageNullOrEmpty);
            }

            return new OptionalException<T>(exception, customMessage);
        }

        /// <summary>
        /// Converts the provided optionals of type <typeparamref name="T"/> into a single optional of type <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <typeparam name="S">original optional type</typeparam>
        /// <typeparam name="T">type to map the optionals to</typeparam>
        /// <param name="optionals">optionals to be mapped</param>
        /// <returns>Single optional of type <see cref="IEnumerable{T}"</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static IOptional<IEnumerable<T>> OfOptionals<S, T>(IEnumerable<S> optionals) where S : IOptional<T>
        {
            if (optionals == null)
            {
                throw new ArgumentNullException(Messages.FromCollectionNullOrEmpty);
            }
            if (!optionals.Any())
            {
                throw new ArgumentException(Messages.FromCollectionNullOrEmpty);
            }

            S[] withException = optionals.Where(_optional => _optional.HasException).ToArray();
            if (withException.Length > 0)
            {
                return Optional.OfException<IEnumerable<T>>(new AggregatedException(withException.Select(_optional => _optional.Exception)), Optional.GetAggregatedMessage<S, T>(withException));
            }

            S[] withMessage = optionals.Where(_optional => _optional.HasMessage).ToArray();
            if (withMessage.Length > 0)
            {
                return Optional.OfMessage<IEnumerable<T>>(Optional.GetAggregatedMessage<S, T>(withMessage));
            }

            return Optional.OfValue(optionals.Select(_optional => _optional.Value));
        }

        private static string GetAggregatedMessage<S, T>(IEnumerable<S> optionals) where S : IOptional<T>
        {
            return String.Join(Environment.NewLine, optionals.Select(_optional => _optional.Message));
        }


        /// <summary>
        /// Returns an <see cref="IOptional{T}">IOptional&lt;IEnumerable&lt;T&gt;&gt;</see> containing only the values of the original
        /// optional that match the provided filter
        /// </summary>
        /// <param name="optional">optional to be filtered</param>
        /// <param name="filter">filter function</param>
        /// <returns><see cref="IOptional{T}">IOptional&lt;IEnumerable&lt;T&gt;&gt;</see> containing only the values of the original optional that match the provided filter</returns>
        public static IOptional<IEnumerable<T>> Filter<T>(this IOptional<IEnumerable<T>> optional, Func<T, bool> filter)
        {
            if (optional == null)
            {
                throw new ArgumentNullException(Messages.ExtensionMethodTargetNull);
            }
            if (filter == null)
            {
                throw new ArgumentNullException(Messages.FilterFunctionNull);
            }

            return optional.Map(_value => (IEnumerable<T>)_value.Where(_item => filter.Invoke(_item)).ToArray());
        }

        /// <summary>
        /// Applies the provided action to all values contained in this <see cref="IOptional{S}"/>
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="optional"></param>
        /// <param name="action"></param>
        /// <returns>the optional ForEach was called on</returns>
        public static IOptional<S> ForEach<S, T>(this IOptional<S> optional, Action<T> action) where S : IEnumerable<T>
        {
            if (optional == null)
            {
                throw new ArgumentNullException(Messages.ExtensionMethodTargetNull);
            }
            if(action == null)
            {
                throw new ArgumentNullException(Messages.ApplyActionNull);
            }

            return optional.Map(_value =>
            {
                foreach (T item in _value)
                {
                    action.Invoke(item);
                }
                return _value;
            });
        }


        /// <summary>
        /// Converts the collection contained in this <see cref="IOptional{T}"/> from a collection of type <typeparamref name="T"/> to a collection of type <typeparamref name="S"/>
        /// using the provided converter function
        /// </summary>
        /// <typeparam name="S">target collection type</typeparam>
        /// <typeparam name="T">original collection type</typeparam>
        /// <typeparam name="V">item type</typeparam>
        /// <param name="optional">optional to be converted</param>
        /// <returns>optional containing the converted collection</returns>
        public static IOptional<S> Convert<T, S, V>(this IOptional<T> optional, Func<T, S> converter) where S : IEnumerable<V> where T : IEnumerable<V>
        {
            if (optional == null)
            {
                throw new ArgumentNullException(Messages.ExtensionMethodTargetNull);
            }
            if (converter == null)
            {
                throw new ArgumentNullException(Messages.ConverterFunctionNull);
            }

            return optional.Map(_value => converter(_value));
        }

        /// <summary>
        /// Casts the collection contained in this <see cref="IOptional{T}"/> from a collection of type <typeparamref name="T"/> to a collection of type <typeparamref name="S"/>
        /// </summary>
        /// <typeparam name="S">target collection type</typeparam>
        /// <typeparam name="T">original collection type</typeparam>
        /// <typeparam name="V">item type</typeparam>
        /// <param name="optional">optional to be converted</param>
        /// <returns>optional containing the casted collection</returns>
        public static IOptional<S> Cast<T, S, V>(this IOptional<T> optional) where S : IEnumerable<V> where T : IEnumerable<V>
        {
            if (optional == null)
            {
                throw new ArgumentNullException(Messages.ExtensionMethodTargetNull);
            }

            try
            {
                return optional.Map(_value => (S)(IEnumerable<V>)_value);
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException(Messages.CollectionNotCastable);
            }
        }
    }
}
