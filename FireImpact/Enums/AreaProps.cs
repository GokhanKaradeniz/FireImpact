using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FireImpact.Enums
{

    public struct AreaProps
    {

        public enum Type
        {
            Flash,
            HighEffect,
            LowEffect
        }


        public enum Priority
        {
            VeryHigh = 0,
            High = 1,
            Medium = 2,
            Low = 3,
            VeryLow = 4
        }

        public enum Range
        {
            Short = 100,
            Medium = 150,
            Long = 250

        }

    }

}
