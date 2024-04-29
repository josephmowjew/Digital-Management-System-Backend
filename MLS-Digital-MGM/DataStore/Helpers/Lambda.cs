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


        private static Random _random = new Random(); // declaring it as static and initiating it once

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
