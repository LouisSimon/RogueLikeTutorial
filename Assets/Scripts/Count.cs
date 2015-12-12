using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts
{
    [Serializable]
    public class Count
    {
        public int Minimum { get; private set; }
        public int Maximum { get; private set; }

        public Count(int minimum, int maximum)
        {
            this.Minimum = minimum;
            this.Maximum = maximum;
        }
    }
}
