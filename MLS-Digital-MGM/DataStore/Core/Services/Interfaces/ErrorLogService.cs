using DataStore.Core.Models;
using DataStore.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Core.Services.Interfaces
{
    public class ErrorLogService : IErrorLogService
    {
        private readonly IErrorLogRepository _errorLogRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ErrorLogService(IErrorLogRepository errorLogRepository, IUnitOfWork unitOfWork)
        {
            _errorLogRepository = errorLogRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task LogErrorAsync(Exception ex)
        {
            // Create a user-friendly error message
            string userFriendlyErrorMessage = "An error occurred while processing the request. Please try again later or contact support if the issue persists.";

            // Build a detailed error message for logging and debugging purposes
            string detailedErrorMessage = BuildDetailedErrorMessage(ex);

            // Save the error to the database
            var errorDetails = new ErrorLog
            {
                UserFriendlyMessage = userFriendlyErrorMessage,
                DetailedMessage = detailedErrorMessage,
                DateOccurred = DateTime.UtcNow
            };

            await _errorLogRepository.AddAsync(errorDetails);
            await _unitOfWork.CommitAsync();
        }

        private string BuildDetailedErrorMessage(Exception ex)
        {
            StringBuilder sb = new StringBuilder();

            // Add the exception message
            sb.AppendLine($"Error Message: {ex.Message}");

            // Add the stack trace
            sb.AppendLine($"Stack Trace: {ex.StackTrace}");

            // Add additional information about the exception, such as inner exceptions
            if (ex.InnerException != null)
            {
                sb.AppendLine("Inner Exception:");
                sb.AppendLine(BuildDetailedErrorMessage(ex.InnerException));
            }

            return sb.ToString();
        }
    }
}
