using System;

namespace LunarDoggo.Optionals
{
    public interface IOptional<T>
    {
        /// <summary>
        /// Returns whether this <see cref="IOptional{T}"/> contains a value
        /// </summary>
        bool HasValue { get; }
        /// <summary>
        /// If this <see cref="IOptional{T}"/> contains a value the value is returned, otherwise a <see cref="NotSupportedException"/>
        /// is thrown, make sure to check <see cref="IOptional{T}.HasValue"/> first
        /// </summary>
        /// <exception cref="NotSupportedException"/>
        T Value { get; }

        /// <summary>
        /// Returns whether this <see cref="IOptional{T}"/> contains a message
        /// </summary>
        bool HasMessage { get; }
        /// <summary>
        /// If this <see cref="IOptional{T}"/> contains a message the message is returned, otherwise a <see cref="NotSupportedException"/>
        /// is thrown, make sure to check <see cref="IOptional{T}.HasMessage"/> first
        /// </summary>
        /// <exception cref="NotSupportedException"/>
        string Message { get; }

        /// <summary>
        /// Returns whether this <see cref="IOptional{T}"/> contains an <see cref="System.Exception"/>
        /// </summary>
        bool HasException { get; }
        /// <summary>
        /// If this <see cref="IOptional{T}"/> contains an <see cref="System.Exception"/>, the <see cref="System.Exception"/> is returned,
        /// otherwise a <see cref="NotSupportedException"/> is thrown, make sure to check <see cref="IOptional{T}.HasException"/> first
        /// </summary>
        /// <exception cref="NotSupportedException"/>
        Exception Exception { get; }

        /// <summary>
        /// Maps this <see cref="IOptional{T}"/> to an <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;. Additionally Exceptions of type <typeparamref name="V"/>
        /// are caught in the process
        /// <br/><br/>
        /// In contrast to <see cref="IOptional{T}.SafeMap{S, V}(Func{T, S})"/> the provided mapper must return an object of type <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt; instead of an object of type <typeparamref name="S"/>
        /// </summary>
        /// <typeparam name="S">Target Type</typeparam>
        /// <typeparam name="V">Type of <see cref="System.Exception"/> to be caught</typeparam>
        /// <param name="mapper">function that maps the current value into an <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;</param>
        /// <returns>Mapped <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;</returns>
        IOptional<S> SafeFlatMap<S, V>(Func<T, IOptional<S>> mapper) where V : Exception;

        /// <summary>
        /// Maps this <see cref="IOptional{T}"/> to an <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;
        /// <br/><br/>
        /// In contrast to <see cref="IOptional{T}.Map{S}(Func{T, S}))"/> the provided mapper must return an object of type <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt; instead of an object of type <typeparamref name="S"/>
        /// </summary>
        /// <typeparam name="S">Target Type</typeparam>
        /// <param name="mapper">function that maps the current value into an <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt; instead of an object of type <typeparamref name="S"/></param>
        /// <returns>Mapped <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt; instead of an object of type <typeparamref name="S"/></returns>
        IOptional<S> FlatMap<S>(Func<T, IOptional<S>> mapper);

        /// <summary>
        /// Maps this <see cref="IOptional{T}"/> into an <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;. Additionally Exceptions of type <typeparamref name="V"/>
        /// are caught in the process
        /// </summary>
        /// <typeparam name="S">Target type</typeparam>
        /// <typeparam name="V">Type of <see cref="System.Exception"/> to be caught</typeparam>
        /// <param name="mapper">function that maps the current value into an object of type <typeparamref name="S"/></param>
        /// <returns>Mapped <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;</returns>
        IOptional<S> SafeMap<S, V>(Func<T, S> mapper) where V : Exception;

        /// <summary>
        /// Maps this <see cref="IOptional{T}"/> into an <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;
        /// </summary>
        /// <typeparam name="S">Target type</typeparam>
        /// <param name="mapper">function that maps the current value into an object of type <typeparamref name="S"/></param>
        /// <returns>Mapped <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;</returns>
        IOptional<S> Map<S>(Func<T, S> mapper);

        /// <summary>
        /// Executes the provided <see cref="Action{T}"/> on the contained value. Additionally Exceptions of type <typeparamref name="S"/> are
        /// caught in the process
        /// </summary>
        /// <typeparam name="S">Type of <see cref="Exception"/> to be caught</typeparam>
        /// <param name="action">Action to apply to the contained value</param>
        /// <returns>Reference to the same <see cref="IOptional{T}"/> on which <see cref="SafeApply{S}(Action{T})"/> was called</returns>
        IOptional<T> SafeApply<S>(Action<T> action) where S : Exception;

        /// <summary>
        /// Executes the provided <see cref="Action{T}"/> on the contained value
        /// </summary>
        /// <param name="action">Action to apply to the contained value</param>
        /// <returns>Reference to the same <see cref="IOptional{T}"/> on which <see cref="Apply(Action{T})"/> was called</returns>
        IOptional<T> Apply(Action<T> action);

        /// <summary>
        /// Converts the contained value into a string and returns the result. If no value is contained in this <see cref="IOptional{T}"/>,
        /// the contained message will be returned instead
        /// </summary>
        /// <param name="valueToString">Function that converts the contained value into a string</param>
        /// <returns>converted string</returns>
        string ToString(Func<T, string> valueToString);
    }
}
