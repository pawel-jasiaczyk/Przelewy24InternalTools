using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Przelewy24;

namespace Przelewy24InrernalTools.Crc
{
    class ParameterVariation
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

            for (int i = 0; i < 5; i++)
            {
                variations.Add (new ParameterVariation (name, value, (VariationName)i));
            }

            return variations.ToArray ();
        }

        #endregion
    }
}
