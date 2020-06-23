using System;

namespace SpaceSim
{
    class ElectricalDistributor : ElectricalComponent
    {
        public double inputVoltage; // V
        public double inputDrain; // W
        public double inputCurrent; // A
        public double outputVoltage; // V
        public double outputDrain; // W
        public double outputCurrent; // A
    }
}