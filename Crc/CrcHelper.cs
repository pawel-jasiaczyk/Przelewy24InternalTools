using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Przelewy24;

namespace Przelewy24InrernalTools.Crc
{
    public class CrcHelper : Przelewy24.Przelewy24
    {
        public string TestedCrc { get; set; }
        
        public string P24_merchant_id { get; set; }
        public string P24_session_id { get; set; }
        public string P24_amount { get; set; }
        public string P24_currency { get; set; }
        public string CrcKey { get; set; }
        public string TransactionId { get; set; }

        private List<ParameterVariation> parameters = new List<ParameterVariation>();
        public ParameterVariation[] ValuesUsedForTestedCrc { get { return parameters.ToArray(); } }

        public CrcHelper()
        { }

        public static ParameterVariation[] FindBugInRegisterSign32(string testedSign, string merchantId, string sessionId, string amount, string currency, string crc)
        {
            CrcHelper helper = new CrcHelper
            {
                TestedCrc = testedSign,
                P24_merchant_id = merchantId,
                P24_amount = amount,
                P24_currency = currency,
                P24_session_id = sessionId,
                CrcKey = crc
            };

            if (helper.FindBugInRegisterSign32())
                return helper.ValuesUsedForTestedCrc;
            else
                return new List<ParameterVariation>().ToArray();
        }

        public bool FindBugInRegisterSign32()
        {
            bool result = false;
            parameters = new List<ParameterVariation> ();
            
            parameters.Add (new ParameterVariation ("p24_session_id", this.P24_session_id));
            parameters.Add (new ParameterVariation ("p24_merchant_id", this.P24_merchant_id));
            parameters.Add (new ParameterVariation ("p24_amount", this.P24_amount));
            parameters.Add (new ParameterVariation ("p24_currency", this.P24_currency));
            parameters.Add (new ParameterVariation ("crckey", this.CrcKey));

            List<string> test = new List<string>();
            result = RecuringTestSequence(0, test);

            return result;
        }

        private bool RecuringTestSequence(int position, List<string> s)
        {
            // When this is not last paramether
            if (position < parameters.Count - 1)
            {
                s.Add(null);
                for(int i = 0; i < Enum.GetNames(typeof(VariationName)).Length; i++)   // loop over all variations
                {
                    s[position] = parameters[position].GetSpecifiedVariationValue((VariationName)i);
                    if (RecuringTestSequence(position + 1, s))   // check next paramether
                    {
                        parameters[position].Variation = (VariationName)i;
                        return true;
                    }
                }
                s.RemoveAt(position);
                return false;    // if this path was blind, return null
            }
            // if this paramether was last in chain
            else if(position == parameters.Count - 1)
            {
                s.Add(null);
                for (int i  = 0; i < Enum.GetNames(typeof(VariationName)).Length; i++)
                {
                    s[position] = parameters[position].GetSpecifiedVariationValue((VariationName)i);
                    if (this.TestedCrc.ToLower() == Przelewy24.Przelewy24.CalculateSign(s.ToArray(), true).ToLower())
                    {
                        parameters[position].Variation = (VariationName)i;
                        return true;
                    }
                }
                s.RemoveAt(position);
                return false;
            }
            else
            {
                throw new IndexOutOfRangeException("Error in recuring reference to \"variations\"");
            }
        }
    }
}
