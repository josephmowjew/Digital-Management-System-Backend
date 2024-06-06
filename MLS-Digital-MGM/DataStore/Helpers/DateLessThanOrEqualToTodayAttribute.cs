using System.ComponentModel.DataAnnotations;
using System;

namespace DataStore.Helpers;


public class DateLessThanOrEqualToTodayAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is DateTime date)
        {
            return date.Date <= DateTime.Today;
        }

        return true; // Return true for null values
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} must be equal to or less than the current date.";
    }
}