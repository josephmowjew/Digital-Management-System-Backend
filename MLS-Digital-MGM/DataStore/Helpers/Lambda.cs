using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStore.Helpers
{
    public class Lambda
    {
        public static string Active = "Active";
        public static string Deleted = "Deleted";
        public static string Disabled = "Disabled";
        public static string Pending = "Pending";


        private static Random _random = new Random(); // declaring it as static and initiating it once

        public static int RandomNumber()
        {
            // generating a random number
            return _random.Next(100000, 1000000);
        }
    }

    
}
