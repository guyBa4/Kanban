using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.BusinessLayer;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    class Password
    {
        // Fields
        private readonly string password;
        private readonly int MIN_PASS_LENGTH = 4;
        private readonly int MAX_PASS_LENGTH = 20;

        //constructor
        internal Password(string pass)
        {
            if (ValidatePassword(pass))
            {
                this.password = pass;
            }
            else
            {
                throw new ArgumentException("Password is illegal");
            }
        }

        internal Password(string pass, bool b)
        {
             this.password = pass;   
        }
        // Methods

        /// <summary>
        /// Check if the given password equals to this password
        /// </summary>
        /// <param name="pass">The password we should match with</param>
        /// <returns>True if the password is equal to an input password, False if not</returns>
        internal bool Equals(Password pass)
        {
            return password.Equals(pass.ToString());
        }

        /// <summary>
        /// Check if the password answer the requirements of password structure
        /// </summary>
        /// <param name="pass">A password we should check</param>
        /// <returns>True if the password answer the requirements of password structure, False if not</returns>
        private bool ValidatePassword(string pass)
        {
            if (pass == null)
            {
                return false;
            }

            if (pass.Length < MIN_PASS_LENGTH | pass.Length > MAX_PASS_LENGTH)
            {
                return false;
            }

            bool isLegal = false;
            bool hasCapital = false;
            bool hasLower = false;
            bool hasDigit = false;


            for (int i = 0; i < pass.Length & !isLegal; i++)
            {
                if (hasCapital == false && Char.IsUpper(pass[i]))
                {
                    hasCapital = true;
                }
                if (hasLower == false && Char.IsLower(pass[i]))
                {
                    hasLower = true;
                }
                if (hasDigit == false && Char.IsDigit(pass[i]))
                {
                    hasDigit = true;
                }
                if (hasCapital & hasLower & hasDigit)
                {
                    isLegal = true;
                }
            }
            if (isLegal)
            {
                string[] notAllowed = { "123456", "123456789", "qwerty", "password", "1111111", "12345678", "abc123", "	1234567", "	password1", "12345",
                "1234567890", "123123","000000", "Iloveyou","1234","1q2w3e4r5t","Qwertyuiop","123","Monkey","Dragon"};
                for (int i = 0; i < notAllowed.Length; i = i + 1)
                {
                    if (pass.Equals(notAllowed[i]))
                    {
                        return false;
                    }
                }

            }


            return isLegal;
        }

        // Returns the password as a string
        public override string ToString()
        {
            return password;
        }
    }
}
