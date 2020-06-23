using System;
using System.Collections.Generic;

namespace SpaceSim
{
    class ElectricalComponent
    {
        public double drain; // W
        public double voltage; // V
        public double current; // A
        public string name;
        public ElectricalComponent Provider;      
    }
}