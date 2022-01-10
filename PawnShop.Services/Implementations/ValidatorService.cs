using PawnShop.Core.Extensions;
using PawnShop.Services.Interfaces;
using System;
using System.Security;
using System.Text.RegularExpressions;

namespace PawnShop.Services.Implementations
{
    public class ValidatorService : IValidatorService
    {
        public bool ValidateIdCardNumber(string idCardNumber)
        {

            if (string.IsNullOrEmpty(idCardNumber))
                return false;

            var tab = new byte[9] { 7, 3, 1, 9, 7, 3, 1, 7, 3 };
            var sum = 0;

            idCardNumber = idCardNumber.Trim().Replace(" ", "");


            bool bResult;
            if (idCardNumber.Length == 9)
            {
                byte b;

                for (int i = 0; i < 3; i++)
                {
                    b = Convert.ToByte(idCardNumber[i]);
                    if (b < 65 || b > 90) return false;
                }
                for (int i = 3; i < 9; i++)
                {
                    b = Convert.ToByte(idCardNumber[i]);
                    if (b < 48 || b > 57) return false;
                }

                for (int i = 0; i < 9; i++)
                {
                    if (i < 3)
                    {
                        sum += (Convert.ToByte(idCardNumber[i]) - 55) * tab[i];
                    }
                    else
                    {
                        sum += Convert.ToInt32(idCardNumber[i].ToString()) * tab[i];
                    }
                }

                bResult = (sum % 10) == 0;
            }
            else
            {
                return false;
            }

            return bResult;
        }

        public bool IsValidPassword(SecureString secureString)
        {
            if (secureString is null)
                throw new ArgumentNullException(nameof(secureString));

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{10,}");
            var hasSpecialCharacter = new Regex("^[a-zA-Z0-9 ]+$");

            var pw = secureString.ConvertToString();

            if (pw is null)
                return false;

            var result = hasNumber.IsMatch(pw) && hasUpperChar.IsMatch(pw) && hasMinimum8Chars.IsMatch(pw) && !hasSpecialCharacter.IsMatch(pw);

            pw = null;
            return result;
        }
    }
}