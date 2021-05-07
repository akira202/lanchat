using System;

namespace Lanchat.Core.Api
{
    /// <summary>
    ///     Inherit this class for create custom API handler.
    /// </summary>
    /// <typeparam name="T">Type of handled model</typeparam>
    public abstract class ApiHandler<T> : IApiHandler
    {
        /// <inheritdoc />
        public Type HandledType { get; } = typeof(T);

        /// <inheritdoc />
        public bool Privileged { get; protected init; }

        /// <inheritdoc />
        public bool Disabled { get; protected set; }

        /// <inheritdoc />
        public void Handle(object data)
        {
            Handle((T) data);
        }

        /// <summary>
        ///     Handle object converted to required type.
        /// </summary>
        /// <param name="data">Converted object.</param>
        protected abstract void Handle(T data);
    }

    /// <summary>
    ///     Use <see cref="ApiHandler{T}" /> instead this.
    /// </summary>
    public interface IApiHandler
    {
        /// <summary>
        ///     Type of handled model.
        /// </summary>
        public Type HandledType { get; }

        /// <summary>
        ///     If handler is privileged it will process data even if node is unready.
        /// </summary>
        public bool Privileged { get; }

        /// <summary>
        ///     Disable handler.
        /// </summary>
        public bool Disabled { get; }

        /// <summary>
        ///     Object handler.
        /// </summary>
        /// <param name="data">API model object.</param>
        public void Handle(object data);
    }
}