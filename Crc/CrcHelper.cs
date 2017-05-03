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

        public CrcHelper()
        {
            Errors = new ParameterVariation[0];
        }

        //private string[] ParameterVariations(string parameter)
        //{
        //    List<string> variations = new List<string>();

        //    variations.Add(parameter);
        //    variations.Add(parameter + " ");
        //    variations.Add(" " + parameter);
        //    variations.Add("");
        //    variations.Add(null);

        //    return variations.ToArray();
        //}

        public bool FindBugInRegisterSign32()
        {
            bool result = false;
            List<ParameterVariation> errors = new List<ParameterVariation> ();
            List<Parameter> parameters = new List<Parameter> ();
            
            parameters.Add (new Parameter ("p24_session_id", this.P24_session_id));
            parameters.Add (new Parameter ("p24_merchant_id", this.P24_merchant_id));
            parameters.Add (new Parameter ("p24_amount", this.P24_amount);
            parameters.Add (new Parameter ("p24_currency", this.P24_currency));
            parameters.Add (new Parameter ("crckey", this.CrcKey));

            ParameterVariation[][] variations = new ParameterVariation[5][];
            for(int i = 0; i < 5; i++)
            {
                variations[i] = ParameterVariation.GetAllVariations(parameters[i]);
            }
            // ten fragment trzeba zapisać rekurencyjnie
            for(int i =0; i < variations.Length; i++)
            {
                for(int j = 0; j < variations[i].Length; j++)
                {
                    string variationHash = Przelewy24.Przelewy24.
                }
            }

            return result;
        }
    }
}
