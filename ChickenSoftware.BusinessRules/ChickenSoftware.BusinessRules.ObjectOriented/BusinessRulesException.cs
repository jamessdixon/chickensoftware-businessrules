using System;
namespace ChickenSoftware.BusinessRules.ObjectOriented
{
    [System.Serializable]
    public class BuisnessRulesException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:BuisException"/> class
        /// </summary>
        public BuisnessRulesException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BuisException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        public BuisnessRulesException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:BuisException"/> class
        /// </summary>
        /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
        /// <param name="inner">The exception that is the cause of the current exception. </param>
        public BuisnessRulesException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}
