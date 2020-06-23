using System;

namespace SpaceSim
{
    class ElectricalProvider : ElectricalComponent
    {
        public double maxStorage; // mAh

        public double stored; // mAh

        public bool chargeable;
    }
}