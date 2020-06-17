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
        public enum SpaceItemType {ROCK, CENTER_OF_UNIVERSE, PLANET, MOON, SPACECRAFT};
        public SpaceItemType type = SpaceItemType.ROCK; // a sane default
        double GravConst = 6.6740831e-11; // N * m²/kg²     G in equations

        public double mass; // in kg
        public double radius; // in meters

        // THE STATE VECTORS
        public Tuple<double, double, double> position; // in m/s <X, Y, Z>
        public Tuple<double, double, double> velocity; // in m/s <X, Y, Z>

        // Some objects can make their own force, with like rockets
        public Tuple<double, double, double> selfForce; // in m/s <X, Y, Z>


        // Position history (1 point pr. second)
        public LinkedList<Tuple<double, double, double>> orbitPoints = new LinkedList<Tuple<double, double, double>>();
        public int maxOrbitPoints = 7200; // (120 minutes)

        // Temporary points (1 point every frame (1/60th sec))
        public LinkedList<Tuple<double, double, double>> orbitPoints_tmp = new LinkedList<Tuple<double, double, double>>();

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

        public void updateVelocity(double deltaTime, Dictionary<string, SpaceItem> items)
        {
            foreach(KeyValuePair<string, SpaceItem> kvp in items)
            {
                SpaceItem s = kvp.Value;
                if(s != this)
                {
                    double dist = Helper.VectorLength(Helper.VectorSubtract(s.position, this.position));
                    Tuple<double, double, double> direction = Helper.VectorNormalize(Helper.VectorSubtract(s.position, this.position));
                    Tuple<double, double, double> force = Helper.VectorMultiply(direction, GravConst * ((this.mass * s.mass) / (dist * dist)));
                    Tuple<double, double, double> acc = Helper.VectorDivide(force, this.mass);
                    // deltatime is in milliseconds
                    velocity = Helper.VectorAdd(velocity, Helper.VectorMultiply(acc, deltaTime / 1000.0));
                }
            }
        }

        public void updatePosition(double deltaTime)
        {
            // deltatime is in milliseconds
            position = Helper.VectorAdd(position, Helper.VectorMultiply(velocity, deltaTime / 1000.0));
            orbitPoints_tmp.AddLast(position);

            // If we have collected 60 orbitPoints (or more for some reason), average them into the proper collection
            if(orbitPoints_tmp.Count >= 60)
            {
                Tuple<double, double, double> avg = new Tuple<double, double, double>(0,0,0);
                int values = 0;
                foreach(Tuple<double, double, double> t in orbitPoints_tmp)
                {
                    if(values == 0)
                    {
                        // The special first one, just insert into average
                        avg = t;
                        values++;
                    }
                    else
                    {
                        // Calculate interative average
                        double i1 = avg.Item1 + ((t.Item1 - avg.Item1) / (values + 1));
                        double i2 = avg.Item2 + ((t.Item2 - avg.Item2) / (values + 1));
                        double i3 = avg.Item3 + ((t.Item3 - avg.Item3) / (values + 1));
                        avg = new Tuple<double, double, double>(i1, i2, i3);
                    }
                }
                orbitPoints.AddLast(avg);
                orbitPoints_tmp.Clear();
            }         

            while(orbitPoints.Count > maxOrbitPoints)
            {
                orbitPoints.RemoveFirst();
            }
        }

    }
}