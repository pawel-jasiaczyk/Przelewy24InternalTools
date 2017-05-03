using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Przelewy24InrernalTools.Crc
{
    public enum VariationName
    {
        plain,  // when parameter is correctly entered
        spaceBefore,
        spaceAfter,
        spaceBoth,
        omnited,    // when paramether was totaly omnited
        empty   // when paramether is an empty string ""
    }
}