using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Convert.Infrastruture.Caching;
using Convert.Service.Interface;
using Convert.Model;
using Convert.Infrastruture.Logging;

namespace Convert.Service
{
    public class ChequeWritingService : IChequeWritingService   
    {
        private const int maxPower = 4;
        private string[] numberNames;
        private readonly ILogger logger;
        private readonly ICache caching;
        public ChequeWritingService(ILogger logger, ICache caching)
        {
            this.logger = logger;
            this.caching = caching;
            InitNumberNames();
        }

        private void InitNumberNames(){

            numberNames = new string[110];
            numberNames[1] = "ONE";
            numberNames[2] = "TWO";
            numberNames[3] = "THREE";
            numberNames[4] = "FOUR";
            numberNames[5] = "FIVE";
            numberNames[6] = "SIX";
            numberNames[7] = "SEVEN";
            numberNames[8] = "EIGHT";
            numberNames[9] = "NINE";
            numberNames[10] = "TEN";
            numberNames[11] = "ELEVEN";
            numberNames[12] = "TWELVE";
            numberNames[13] = "THIRTEEN";
            numberNames[14] = "FOURTEEN";
            numberNames[15] = "FIFTEEN";
            numberNames[16] = "SIXTEEN";
            numberNames[17] = "SEVENTEEN";
            numberNames[18] = "EIGHTEEN";
            numberNames[19] = "NINETEEN";
            numberNames[20] = "TWENTY";
            numberNames[30] = "THIRTY";
            numberNames[40] = "FOURTY";
            numberNames[50] = "FIFTY";
            numberNames[60] = "SIXTY";
            numberNames[70] = "SEVENTY";
            numberNames[80] = "EIGHTY";
            numberNames[90] = "NINETY";
            numberNames[100] = "HUNDRED";

            numberNames[101] = "THOUSAND";
            numberNames[102] = "MILLION";
            numberNames[103] = "BILLION";
            numberNames[104] = "TRILLION";
        }

        public double ParseChequeModel(ChequeModel model)
        {
            if (string.IsNullOrEmpty(model.UserName)){
                throw new Exception("");
            }

            double amount = model.Amount;

            if (amount > Math.Pow(1000, maxPower+1))
                throw new Exception("The number is too big to convert.");
            if (amount < 0)
                throw new Exception("The cheque amount must be great than zero.");
            if (amount == 0)
                throw new Exception("The cheque amount can't not be zero");
            if (amount < 0.05)
                throw new Exception("The amount is too small.");

            return amount;
        }
        public ChequeModel Translate(ChequeModel model)
        {
            var amount = ParseChequeModel(model);
            amount = Math.Round(amount, 2);

            var sb = new StringBuilder();

            long intPart = System.Convert.ToInt64(Math.Floor(amount));
            int intFloatPart = System.Convert.ToInt16((amount - intPart)*100);
            sb.Append(TranslateIntegerPart(intPart));
            sb.Append("DOLLAR");
            if (intPart > 1) sb.Append("S");
            if (intFloatPart > 0)
            {
                sb.Append(" AND ");
                sb.Append(TranslateIntegerPart(intFloatPart));
                sb.Append(" CENT");
                if (intFloatPart > 1) sb.Append("S");
            }
            return new ChequeModel()
            {
                UserName = model.UserName,
                Amount = model.Amount,
                AmountString = sb.ToString()
            };
        }

        private string TranslateIntegerPart(long number)
        {
            var position = maxPower;
            var sb = new StringBuilder();
            while (number > 0)
            {
                long powNumber = System.Convert.ToInt64(Math.Pow(1000, position));
                long modNumber = (number % powNumber);
                if (modNumber != number)
                {
                    sb.Append(TranslateUnderThousand(number / powNumber));
                    if (position > 0)
                    {
                        sb.Append(numberNames[position + 100]);
                        if (number / powNumber > 1) sb.Append("S");
                        sb.Append(" AND ");
                    }
                    number = modNumber;
                }
                position--;
            }
            
            var result = sb.ToString();
            if (result.EndsWith("AND "))
            {
                result = result.Substring(0, result.Length - 4);
            }
            return result;
        }

        private string TranslateUnderThousand(long number)
        {
            var key = "ChequeWritingService" + number.ToString();
            var translated = caching.GetObject(key);
            if (translated == null)
            {
                var sb = new StringBuilder();
                if (number/100 > 0)
                {
                    sb.Append(numberNames[number/100]);
                    sb.Append(" ");
                    sb.Append(numberNames[100]);
                    if (number/100 > 1) sb.Append("S");
                    number = number%100;
                    sb.Append(" ");
                    sb.Append("AND "); //does it need "AND" here?
                }
                if (number > 20)
                {
                    sb.Append(numberNames[number - number%10]);
                    sb.Append("-");
                    sb.Append(numberNames[number%10]);
                }
                else
                {
                    sb.Append(numberNames[number]);
                }

                sb.Append(" ");
                var result = sb.ToString();
                caching.AddObject(key, new DateTime(2100, 1, 1), result);
                return result;
            }
            else
            {
                return translated.ToString();
            }
        }
    }
}

