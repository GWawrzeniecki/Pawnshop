using PawnShop.Services.Interfaces;
using System;

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
    }
}