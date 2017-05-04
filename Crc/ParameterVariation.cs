using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Przelewy24;

namespace Przelewy24InrernalTools.Crc
{
    public class ParameterVariation
    {
        #region Properties

        public string ParameterName { get; private set; }
        public VariationName Variation { get; private set; }
        public string ParameterValue { get; private set; }
        public string VariationValue { get; private set; }
        public bool DoTest { get; set; }
        public Przelewy24.Parameter Parameter { get; private set; }

        #endregion

        #region Constructors

        public ParameterVariation(Przelewy24.Parameter parameter) 
            : this(parameter, VariationName.plain)
        { }

        public ParameterVariation(string name, string value) 
            : this(name, value, VariationName.plain)
        { }
        
        public ParameterVariation (Przelewy24.Parameter parameter, VariationName variation)
        {
            this.Parameter = parameter;
            this.ParameterName = parameter.Name;
            this.ParameterValue = parameter.Value;
            this.Variation = variation;
            this.VariationValue = GetSpecifiedVariationValue(variation);
        }

        public ParameterVariation (string name, string value, VariationName variation)
            : this(new Przelewy24.Parameter(name, value), variation)
        {
        }

        #endregion

        #region Public Methods

        public string GetSpecifiedVariationValue(VariationName variation)
        {
            string toReturn = "";
            switch (variation)
            {
                case (VariationName.omnited):
                {
                    toReturn = null;
                    break;
                }
                case (VariationName.plain):
                {
                    toReturn = this.ParameterValue;
                    break;
                }
                case (VariationName.spaceAfter):
                {
                    toReturn = this.ParameterValue + " ";
                    break;
                }
                case (VariationName.spaceBefore):
                {
                    toReturn = " " + this.ParameterValue;
                    break;
                }
                case (VariationName.spaceBoth):
                {
                    toReturn = " " + this.ParameterValue + " ";
                    break;
                }
                case (VariationName.empty):
                {
                    toReturn = "";
                    break;
                }
                default:
                {
                    throw new ApplicationException("Error during creation Parametar Variation - it was used unsupported enum value: "
                        + variation.ToString());
                }
            }
            return toReturn;
        }

        #endregion

        #region Static Methods

        public static ParameterVariation[] GetAllVariations (Przelewy24.Parameter parameter)
        {
            return GetAllVariations (parameter.Name, parameter.Value);
        }

        public static ParameterVariation[] GetAllVariations (string name, string value)
        {
            List<ParameterVariation> variations = new List<ParameterVariation> ();

            var variationNames = Enum.GetValues(typeof(VariationName));
            foreach(var variationName in variationNames)
            {
                variations.Add(new ParameterVariation(name, value, (VariationName)variationName));
            }


            //for (int i = 0; i < 6; i++)
            //{
            //    variations.Add (new ParameterVariation (name, value, (VariationName)i));
            //}

            return variations.ToArray ();
        }

        #endregion

        #region override

        public override string ToString()
        {
            string result = string.Format("ParameterName = {0}, ParameterValue = {1}, Variation = {2}, VariationValuee = {3}",
                this.ParameterName, this.ParameterValue, this.Variation.ToString(), this.VariationValue);
            return result;
        }

        #endregion
    }
}
