using System;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;

namespace SpaceSim
{
    class ElectricalSwitch : ElectricalDistributor
    {
        public List<ElectricalComponent> providers;
        public int activeProvider;

        public bool isFuse;
        public bool isOn;
        public bool hasMultipleProviders;

        public double fuseMaxAmps; // A

        public Point location;
        public Size size;

    }
}