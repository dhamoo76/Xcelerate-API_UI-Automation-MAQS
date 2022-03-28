using System.Collections.Generic;

namespace Tests.API.Utilities.Helpers
{
    public class ValidationResponse
    {
        public string Error { get; set; }
        public IDictionary<string, XValidationResult[]> Validations { get; set; }
        public IDictionary<string, XValidationResult[]> Details { get; set; }
    }

    public class XValidationResult
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public XValidationResult(string errorMessage, string errorCode)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Specific message to field validation error.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Specific code to field validation error.
        /// </summary>
        public string ErrorCode { get; set; }

    }
}
