using System;
using System.Collections.Generic;

namespace SpaceSim
{
    class SpaceItem
    {
        /***
         * This is a masterclass of all "stuff in space",
         * it can be planets, moons, spacecraft, astroid,
         * stranded astronauts, sattellites and all other
         * stuff.
         * 
         * THE VECTORS ARE DEFINED THUSLY:
		 * Item1: X-axis points out from the center of the earth and through the equator at 0°
		 * Item2: Y-axis points out from the center of the earth and through the equator at 90° E
		 * Item3: z-axis points out from the center of the earth and through the north pole
         ***/
        public string name;

        // The CENTER_OF_UNIVERSE is a special type of item, you should only designate one of these.
        // The CENTER_OF_UNIVERSE will always be at location 0,0,0 and if it moves (because REASONS),
        // the universe moves with it, so it remains at 0,0,0
        public enum SpaceItemType {ROCK, CENTER_OF_UNIVERSE, PLANET, SPACECRAFT};
        public SpaceItemType type = SpaceItemType.ROCK; // a sane default
        double GravConst = 6.6740831e-11; // N * m²/kg²     G in equations

        public double mass; // in kg
        public double radius; // in meters

        // THE STATE VECTORS
        public Tuple<double, double, double> position; // in m/s <X, Y, Z>
        public Tuple<double, double, double> velocity; // in m/s <X, Y, Z>

        // Some objects can make their own force, with like rockets
        public Tuple<double, double, double> selfForce; // in m/s <X, Y, Z>


        // Position history
        List<Tuple<double, double, double>> orbitPoints = new List<Tuple<double, double, double>>();

        public SpaceItem(string name, SpaceItemType type, double mass, double radius)
		{
			this.name = name;
            this.type = type;
            this.mass = mass;
            this.radius = radius;
            this.position = new Tuple<double, double, double>(0,0,0);
            this.velocity = new Tuple<double, double, double>(0,0,0);
            this.selfForce = new Tuple<double, double, double>(0,0,0);
		}

        public void updateVelocity(double deltaTime, List<SpaceItem> items)
        {
            foreach(SpaceItem s in items)
            {
                if(s != this)
                {
                    double dist = Helper.VectorLength(Helper.VectorSubtract(s.position, this.position));
                    Tuple<double, double, double> direction = Helper.VectorNormalize(Helper.VectorSubtract(s.position, this.position));
                    Tuple<double, double, double> force = Helper.VectorMultiply(direction, GravConst * ((this.mass * s.mass) / (dist * dist)));
                    Tuple<double, double, double> acc = Helper.VectorDivide(force, this.mass);
                    velocity = Helper.VectorAdd(velocity, Helper.VectorMultiply(acc, deltaTime));
                }
            }
        }

        public void updatePosition(double deltaTime)
        {
            position = Helper.VectorAdd(position, Helper.VectorMultiply(velocity, deltaTime));
            orbitPoints.Add(position);

            if(orbitPoints.Count > 10000)
            {
                orbitPoints.RemoveAt(0);
            }
        }

    }
}