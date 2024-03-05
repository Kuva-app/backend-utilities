using System;
using System.Threading.Tasks;

namespace Utilities.General.Http
{
    /// <summary>
    /// Http Error
    /// </summary>
    public class HttpError
    {

        /// <summary>
        /// The track error
        /// </summary>
        private readonly Action<Exception> _trackError;
        /// <summary>
        /// The exception
        /// </summary>
        private readonly Exception _exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpError" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="trackError">The track error.</param>
        public HttpError(Exception exception, Action<Exception> trackError = null)
        {
            _exception = exception;
            _trackError = trackError ?? ((e) => System.Diagnostics.Debug.WriteLine(e));
        }

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message => _exception.Message;
        /// <summary>
        /// Gets the stacktrace.
        /// </summary>
        /// <value>
        /// The stacktrace.
        /// </value>
        public string Stacktrace => _exception.StackTrace;

        /// <summary>
        /// Logs the error.
        /// </summary>
        public Task LogErrorAsync()
        {
            return Task.Run(() =>
            {
                _trackError?.Invoke(_exception);
            });
        }
    }
}
