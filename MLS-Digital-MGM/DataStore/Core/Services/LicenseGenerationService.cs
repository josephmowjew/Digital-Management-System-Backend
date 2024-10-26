using System;
using System.Threading.Tasks;
using DataStore.Core.Services.Interfaces;
using DataStore.Core.Models;
using DataStore.Persistence.Interfaces;

// Service for generating licenses for members based on the year of operation
public class LicenseGenerationService : ILicenseGenerationService
{
    private readonly IRepositoryManager _repositoryManager; // Repository manager for data access
    private readonly IUnitOfWork _unitOfWork; // Unit of work for transaction management
    private readonly IEmailService _emailService; // Service for sending emails

    // Constructor to initialize dependencies
    public LicenseGenerationService(
        IRepositoryManager repositoryManager,
        IUnitOfWork unitOfWork,
        IEmailService emailService)
    {
        _repositoryManager = repositoryManager;
        _unitOfWork = unitOfWork;
        _emailService = emailService;
    }

    // Generates licenses for all unlicensed members for a given year of operation
    public async Task<(int success, int failed)> GenerateLicensesForYear(int? yearOfOperationId = null)
    {
        var successCount = 0; // Counter for successfully generated licenses
        var failedCount = 0; // Counter for failed license generation
        
        // If no year provided, get current active year
        var yearOfOperation = yearOfOperationId.HasValue 
            ? await _repositoryManager.YearOfOperationRepository.GetByIdAsync(yearOfOperationId.Value)
            : await _repositoryManager.YearOfOperationRepository.GetCurrentYearOfOperation();

        if (yearOfOperation == null) throw new ArgumentException("No valid year of operation found");

        // Get all unlicensed members
        var members = await _repositoryManager.MemberRepository.GetUnlicensedMembersAsync();
        
        // Iterate through each member to generate licenses
        foreach (var member in members)
        {
            try 
            {
                // Check if the member already has a license for the current year of operation
                var existingLicense = await _repositoryManager.LicenseRepository
                    .GetAsync(l => l.MemberId == member.Id && l.YearOfOperationId == yearOfOperationId);

                if (existingLicense != null) continue; // Skip if license already exists

                // Generate a new license number
                var licenseNumber = await GenerateLicenseNumber(yearOfOperation);
                
                // Create a new license object
                var license = new License
                {
                    MemberId = member.Id,
                    YearOfOperationId = yearOfOperationId.Value,
                    LicenseNumber = licenseNumber,
                    ExpiryDate = yearOfOperation.EndDate,
                    Status = "Active"
                };

                // Add the new license to the repository and commit the transaction
                await _repositoryManager.LicenseRepository.AddAsync(license);
                await _unitOfWork.CommitAsync();

                // Send email notification to the member
                await _emailService.SendMailWithKeyVarReturn(
                    member.User.Email,
                    "License Generated",
                    $"Your license for {yearOfOperation.StartDate.Year}-{yearOfOperation.EndDate.Year} has been generated. License Number: {licenseNumber}",
                    false
                );

                successCount++; // Increment success count
            }
            catch
            {
                failedCount++; // Increment failed count on exception
            }
        }

        return (successCount, failedCount); // Return the counts of success and failure
    }

    // Generates a unique license number based on the last issued license
    private async Task<string> GenerateLicenseNumber(YearOfOperation yearOfOperation)
    {
        var lastLicense = await _repositoryManager.LicenseRepository.GetLastLicenseNumber(yearOfOperation.Id);
        
        // If no licenses exist, start with the first license number
        if (lastLicense == null)
        {
            return $"{yearOfOperation.StartDate.Year}{yearOfOperation.EndDate.Year}MLS0001";
        }

        // Extract the last number and increment it for the new license
        var lastNumber = int.Parse(lastLicense.LicenseNumber.Substring(lastLicense.LicenseNumber.Length - 4));
        return $"{yearOfOperation.StartDate.Year}{yearOfOperation.EndDate.Year}MLS{(lastNumber + 1).ToString("D4")}";
    }
}
