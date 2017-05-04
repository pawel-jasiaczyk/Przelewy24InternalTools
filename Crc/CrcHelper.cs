using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Przelewy24;

namespace Przelewy24InrernalTools.Crc
{
    public class CrcHelper
    {
        public string TestedCrc { get; set; }
        
        public string P24_merchant_id { get; set; }
        public string P24_session_id { get; set; }
        public string P24_amount { get; set; }
        public string P24_currency { get; set; }
        public string CrcKey { get; set; }
        public string TransactionId { get; set; }

        public ParameterVariation[] Errors { get; private set; }

        private ParameterVariation[][] variations = new ParameterVariation[1][];
        private List<ParameterVariation> errors = new List<ParameterVariation>();

        public ParameterVariation[] ValuesUsedForTestedCrc { get { return errors.ToArray(); } }

        public CrcHelper()
        {
            Errors = new ParameterVariation[0];
        }

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

            helper.FindBugInRegisterSign32();
            return helper.ValuesUsedForTestedCrc;
        }

        public bool FindBugInRegisterSign32()
        {
            bool result = false;
            // List<ParameterVariation> errors = new List<ParameterVariation> ();
            List<ParameterVariation> parameters = new List<ParameterVariation> ();
            
            parameters.Add (new ParameterVariation ("p24_session_id", this.P24_session_id));
            parameters.Add (new ParameterVariation ("p24_merchant_id", this.P24_merchant_id));
            parameters.Add (new ParameterVariation ("p24_amount", this.P24_amount));
            parameters.Add (new ParameterVariation ("p24_currency", this.P24_currency));
            parameters.Add (new ParameterVariation ("crckey", this.CrcKey));

            this.variations = new ParameterVariation[5][];
            for(int i = 0; i < 5; i++)
            {
                variations[i] = ParameterVariation.GetAllVariations(parameters[i].Parameter);
            }

            string test = "";
            //string[] test = new string[5];
            RecuringTestSequence(0, test);

            return result;
        }

        private ParameterVariation RecuringTestSequence(int position, string s)
        {
            // Special - do not use any '|' character
            // not supported yet
            // it should be included in possiotion == variations.Length - 1
            if (variations.Length == 1)
            {
                // test
                return null;
            }
            // When this is not last paramether
            if (position < variations.Length - 1)
            {
                string temp = s;
                if (position != 0)  // for first one paramether we do not add an separator '|'
                    temp += '|';
                for(int i = 0; i < this.variations[position].Length; i++)   // loop over all variations
                {
                    if (this.variations[position][i].Variation == VariationName.omnited)    // if paramether was omnited, just run for source string, not modified
                    {
                        if(RecuringTestSequence(position + 1, s) != null)   // check next paramether
                        {
                            this.errors.Insert(0, this.variations[position][i]);    // if found correct chain, put value into list an leave
                            return this.variations[position][i];
                        }
                    }
                    else    // if paramether has value, wasn't omnited
                    {
                        string test = temp + this.variations[position][i].VariationValue;   // add variation to chain
                        if (RecuringTestSequence(position + 1, test) != null)   // check next paramether
                        {
                            this.errors.Insert(0, this.variations[position][i]);    // if found correcf chain, put value into list and leave
                            return this.variations[position][i];
                        }
                    }
                }
                return null;    // if this path was blind, return null
            }
            // if this paramether was last in chain
            else if(position == variations.Length - 1)
            {
                // loop over all variations
                for (int i  = 0; i < this.variations[position].Length; i++)
                {
                    if (this.variations[position][i].Variation == VariationName.omnited)    // if it was omnited, just ckech MD5 for previous paramethers
                    {
                        if (this.TestedCrc.ToLower() == Przelewy24.Przelewy24.CalculateMD5Hash(s).ToLower())    // MS MD5 returns uppercase value, a lot of other tool lowercase
                        {
                            this.errors.Add(this.variations[position][i]);  // if found correct chain, add variation to list and return it
                            return this.variations[position][i];
                        }
                    }
                    else
                    {
                        string test = s + '|' + this.variations[position][i].VariationValue;
                        if (this.TestedCrc.ToLower() == Przelewy24.Przelewy24.CalculateMD5Hash(test).ToLower())
                        {
                            this.errors.Add(this.variations[position][i]);
                            return this.variations[position][i];
                        }
                    }
                }
                return null;
            }
            else
            {
                throw new IndexOutOfRangeException("Error in recuring reference to \"variations\"");
            }
        }
    }
}
