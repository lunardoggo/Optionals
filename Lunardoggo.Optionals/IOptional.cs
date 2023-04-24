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
        /// <exception cref="ArgumentNullException"></exception>
        IOptional<S> SafeFlatMap<S, V>(Func<T, IOptional<S>> mapper) where V : Exception;

        /// <summary>
        /// Maps this <see cref="IOptional{T}"/> to an <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;
        /// <br/><br/>
        /// In contrast to <see cref="IOptional{T}.Map{S}(Func{T, S}))"/> the provided mapper must return an object of type <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt; instead of an object of type <typeparamref name="S"/>
        /// </summary>
        /// <typeparam name="S">Target Type</typeparam>
        /// <param name="mapper">function that maps the current value into an <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt; instead of an object of type <typeparamref name="S"/></param>
        /// <returns>Mapped <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt; instead of an object of type <typeparamref name="S"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        IOptional<S> FlatMap<S>(Func<T, IOptional<S>> mapper);

        /// <summary>
        /// Maps this <see cref="IOptional{T}"/> into an <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;. Additionally Exceptions of type <typeparamref name="V"/>
        /// are caught in the process
        /// </summary>
        /// <typeparam name="S">Target type</typeparam>
        /// <typeparam name="V">Type of <see cref="System.Exception"/> to be caught</typeparam>
        /// <param name="mapper">function that maps the current value into an object of type <typeparamref name="S"/></param>
        /// <returns>Mapped <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;</returns>
        /// <exception cref="ArgumentNullException"></exception>
        IOptional<S> SafeMap<S, V>(Func<T, S> mapper) where V : Exception;

        /// <summary>
        /// Maps this <see cref="IOptional{T}"/> into an <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;
        /// </summary>
        /// <typeparam name="S">Target type</typeparam>
        /// <param name="mapper">function that maps the current value into an object of type <typeparamref name="S"/></param>
        /// <returns>Mapped <see cref="IOptional{T}">IOptional</see>&lt;<typeparamref name="S"/>&gt;</returns>
        /// <exception cref="ArgumentNullException"></exception>
        IOptional<S> Map<S>(Func<T, S> mapper);

        /// <summary>
        /// Executes the provided <see cref="Action{T}"/> on the contained value
        /// <br/><br/>
        /// Functionally this method behaves the same way as <see cref="IfHasValue(Action{Exception})"/>, but semantically
        /// this method should be used if you want to make alterations to the contained value; this of course only works
        /// if the type of the contained value is a reference type
        /// </summary>
        /// <param name="action">Action to apply to the contained value</param>
        /// <returns>Reference to the same <see cref="IOptional{T}"/> on which <see cref="Apply(Action{T})"/> was called</returns>
        /// <exception cref="ArgumentNullException"></exception>
        IOptional<T> Apply(Action<T> action);

        /// <summary>
        /// Executes the provided <see cref="Action"/>&lt;<see cref="System.Exception"/>&gt; if this <see cref="IOptional{T}"/>
        /// contains an exception
        /// </summary>
        /// <param name="action">Action to be executed if this optional contains an exception</param>
        /// <returns>Reference to this optional</returns>
        /// <exception cref="ArgumentNullException"></exception>
        IOptional<T> IfHasException(Action<Exception> action);

        /// <summary>
        /// Executes the provided <see cref="Action"/>&lt;<see cref="String"/>&gt; if this <see cref="IOptional{T}"/>
        /// contains a message
        /// </summary>
        /// <param name="action">Action to be executed if this optional contains a message</param>
        /// <returns>Reference to this optional</returns>
        /// <exception cref="ArgumentNullException"></exception>
        IOptional<T> IfHasMessage(Action<string> action);

        /// <summary>
        /// Executes the provided <see cref="Action{T}"/> if this <see cref="IOptional{T}"/> contains a value
        /// <br/><br/>
        /// Functionally this method behaves the same way as <see cref="Apply(Action{T})"/>, but semantically
        /// this method should be used if you want to call an <see cref="Action{T}"/> that does not alter the 
        /// value contained in this <see cref="IOptional{T}"/>
        /// </summary>
        /// <param name="action">Action to be executed if this optional contains a value</param>
        /// <returns>Reference to this optional</returns>
        /// <exception cref="ArgumentNullException"></exception>
        IOptional<T> IfHasValue(Action<T> action);

        /// <summary>
        /// Converts the contained value into a string and returns the result. If no value is contained in this <see cref="IOptional{T}"/>,
        /// the contained message will be returned instead
        /// </summary>
        /// <param name="valueToString">Function that converts the contained value into a string</param>
        /// <returns>converted string</returns>
        /// <exception cref="ArgumentNullException"></exception>
        string ToString(Func<T, string> valueToString);

        /// <summary>
        /// If this <see cref="IOptional{T}"/> contains a value, the contained value is returned,
        /// otherwise the return value of <paramref name="valueGetter"/> is returned
        /// </summary>
        /// <param name="valueGetter">Function to generate the alternative return value</param>
        /// <returns>the contained value if present, otherwise the return value of <paramref name="valueGetter"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        T OrElse(Func<T> valueGetter);

        /// <summary>
        /// If this <see cref="IOptional{T}"/> contains a value, the contained value is returned,
        /// otherwise the provided <paramref name="value"/> is returned
        /// </summary>
        /// <param name="value">alternative return value</param>
        /// <returns>the contained value if present, otherwise the provided <paramref name="value"/></returns>
        T OrElse(T value);
    }
}
