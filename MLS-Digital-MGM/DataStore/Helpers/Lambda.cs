using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Persistence.Interfaces;

namespace DataStore.Helpers
{
    public class Lambda
    {
        public static string Active = "Active";
        public static string Deleted = "Deleted";
        public static string Disabled = "Disabled";
        public static string Pending = "Pending";
        public static string Rejected = "Rejected";
        public static string Approved = "Approved";
        public static string Denied = "Denied";
        public static string Incomplete = "Incomplete";
        public static string Draft = "Draft";
        public static string Submit = "Submit";

        public static string ProBonoApplication = "Pro Bono Application";
        public static string LicenseApplication = "License Application";
        public static string LicenseApplicationFolderName = "LicenseApplication";
        public static string CPDTrainingFolderName ="CPDTrainings";


        private static Random _random = new Random(); // declaring it as static and initiating it once

        public static string Accepted = "Accepted";

        public static string UnderReview = "Under Review";

        //CPD Units section
        public static string Registered = "Registered";
        public static string Cancelled = "Cancelled";
        public static string Attended = "Attended";
        public static string Finance = "Finance Officer";

        //Penalty sections
        public static string Issued = "Issued";
        public static string Paid = "Paid";
        public static string PartiallyPaid = "Partially Paid";
        public static string Resolved = "Resolved";

        public static int RandomNumber()
        {
            // generating a random number
            return _random.Next(100000, 1000000);
        }

        public static string GetCurrentUserRole(IRepositoryManager repositoryManager, string userId)
        {
            if (string.IsNullOrEmpty(userId) || repositoryManager == null) 
            {
                throw new ArgumentException("userId or repositoryManager cannot be null or empty");
            }

            var userRole = repositoryManager.UserRepository.GetUserRoleByUserId(userId);

            if (userRole == null) 
            {
                return string.Empty;
            }

            string currentRole = repositoryManager.UserRepository.GetRoleName(userRole.RoleId);

            if (string.IsNullOrEmpty(currentRole))
            {
                return string.Empty;
            }

            return currentRole;
        }

    }

    
}
