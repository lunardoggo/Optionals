using System.Collections.Generic;
using System;

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
            if(value == null)
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

        public static IOptional<IEnumerable<T>> OfOptionals<S, T>(IEnumerable<S> optionals) where S : IOptional<T>
        {
            throw new NotImplementedException();
        }
    }
}
