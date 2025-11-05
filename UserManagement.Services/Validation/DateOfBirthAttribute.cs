using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Services.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateOfBirthAttribute : ValidationAttribute
    {
        public int MinimumAge { get; }
        public int MaximumAge { get; }

        public DateOfBirthAttribute(int minimumAge = 13, int maximumAge = 110)
        {
            MinimumAge = minimumAge;
            MaximumAge = maximumAge;
            ErrorMessage = $"Date of Birth must result in an age between {minimumAge} and {maximumAge}, and cannot be in the future.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is not DateOnly dob)
                return ValidationResult.Success; // ignore nulls — [Required] can handle that

            var today = DateOnly.FromDateTime(DateTime.Today);
            if (dob > today)
                return new ValidationResult("Date of Birth cannot be in the future.");


            var age = today.Year - dob.Year;          //Approx age
            var birthdayThisYear = dob.AddYears(age); //Check if birthday has occurred this year
            if (birthdayThisYear > today)
                age--;                                //If not subtract 1 from approx



            if (age < MinimumAge)
                return new ValidationResult($"User must be at least {MinimumAge} years old.");

            if (age > MaximumAge)
                return new ValidationResult($"Age cannot exceed {MaximumAge} years.");

            return ValidationResult.Success;
        }
    }
}
