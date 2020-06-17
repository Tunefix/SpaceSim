using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceSim
{
    static class OrbitHelper
	{
        static double GravConst = 6.6740831e-11; // N * m²/kg²     G in equations

        public static List<Tuple<double, double, double>> IteratePosition(SpaceItem moving_item, List<SpaceItem> list, int iterations)
		{
            Tuple<double, double, double> velocity = moving_item.velocity;
            Tuple<double, double, double> position = moving_item.position;
            List<Tuple<double,double,double>> positions = new List<Tuple<double, double, double>>();

            for(int i = 0; i < iterations; i++)
            {
                foreach(SpaceItem s in list)
                {
                    if(s != moving_item)
                    {
                        double dist = Helper.VectorLength(Helper.VectorSubtract(s.position, moving_item.position));
                        Tuple<double, double, double> direction = Helper.VectorNormalize(Helper.VectorSubtract(s.position, moving_item.position));
                        Tuple<double, double, double> force = Helper.VectorMultiply(direction, GravConst * ((moving_item.mass * s.mass) / (dist * dist)));
                        Tuple<double, double, double> acc = Helper.VectorDivide(force, moving_item.mass);
                        velocity = Helper.VectorAdd(velocity, Helper.VectorMultiply(acc, 0.01666666));
                    }
                }

                position = Helper.VectorAdd(position, Helper.VectorMultiply(velocity, 0.01666666));
                positions.Add(position);
            }

            

            return positions;
        }
    }
}