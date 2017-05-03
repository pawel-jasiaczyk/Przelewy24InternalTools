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

        #endregion

        #region Constructors

        public ParameterVariation (Parameter parameter, VariationName variation)
            : this (parameter.Name, parameter.Value, variation)
        { }

        public ParameterVariation (string name, string value, VariationName variation)
        {
            this.ParameterName = name;
            this.ParameterValue = value;
            this.Variation = variation;

            switch (variation)
            {
                case (VariationName.omnited):
                {
                    this.VariationValue = null;
                    break;
                }
                case (VariationName.plain):
                {
                    this.VariationValue = value;
                    break;
                }
                case (VariationName.spaceAfter):
                {
                    this.VariationValue = value + " ";
                    break;
                }
                case (VariationName.spaceBefore):
                {
                    this.VariationValue = " " + value;
                    break;
                }
                case (VariationName.spaceBoth):
                {
                    this.VariationValue = " " + value + " ";
                    break;
                }
                case (VariationName.empty):
                {
                    this.VariationValue = "";
                    break;
                }
                default:
                {
                    throw new ApplicationException("Error during creation Parametar Variation - it was used unsupported enum value: "
                        + variation.ToString());
                }
            }
        }

        #endregion

        #region Static Methods

        public static ParameterVariation[] GetAllVariations (Parameter parameter)
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

        public override string ToString()
        {
            string result = string.Format("ParameterName = {0}, ParameterValue = {1}, Variation = {2}, VariationValuee = {3}",
                this.ParameterName, this.ParameterValue, this.Variation.ToString(), this.VariationValue);
            return result;
        }
    }
}
