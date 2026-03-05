/*
This coding question evaluates string manipulation and validation.

Requirements:

Create a method ValidatePassword(string password)
that returns true only if:

- Minimum 8 characters
- At least 1 uppercase letter
- At least 1 lowercase letter
- At least 1 digit
- At least 1 special character

Test multiple passwords in Main().
*/

using System;

namespace PasswordSanitizer
{
    public class PasswordValidator
    {
        public bool ValidatePassword(string password)
        {
            // TODO: Implement validation logic
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PasswordValidator validator = new PasswordValidator();

            Console.WriteLine(validator.ValidatePassword("Test123!"));
            Console.WriteLine(validator.ValidatePassword("weakpass"));

            Console.ReadLine();
        }
    }
}