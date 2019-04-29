using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceSim
{
	static class Helper
	{
		public static Tuple<double, double, double> VectorAdd(Tuple<double, double, double> v1, Tuple<double, double, double> v2)
		{
			return new Tuple<double, double, double>(v1.Item1 + v2.Item1, v1.Item2 + v2.Item2, v1.Item3 + v2.Item3);
		}

		public static Tuple<double, double, double> VectorSubtract(Tuple<double, double, double> v1, Tuple<double, double, double> v2)
		{
			return new Tuple<double, double, double>(v1.Item1 - v2.Item1, v1.Item2 - v2.Item2, v1.Item3 - v2.Item3);
		}

		public static Tuple<double, double, double> VectorMultiply (Tuple<double, double, double> v1, double d)
		{
			return new Tuple<double, double, double>(v1.Item1 * d, v1.Item2 * d, v1.Item3 * d);
		}

		public static Tuple<double, double, double> VectorDivide(Tuple<double, double, double> v1, double d)
		{
			return new Tuple<double, double, double>(v1.Item1 / d, v1.Item2 / d, v1.Item3 / d);
		}

		public static Tuple<double, double, double> VectorCrossProduct(Tuple<double, double, double> v1, Tuple<double, double, double> v2)
		{
			return new Tuple<double, double, double>(
				(v1.Item2 * v2.Item3) - (v2.Item2 * v1.Item3),
				(v2.Item1 * v1.Item3) - (v1.Item1 * v2.Item3),
				(v1.Item1 * v2.Item2) - (v2.Item1 * v1.Item2)
				);
		}

		public static double VectorLength(Tuple<double, double, double> v)
		{
			return Math.Sqrt(Math.Pow(v.Item1, 2) + Math.Pow(v.Item2, 2) + Math.Pow(v.Item3, 2));
		}

		public static double VectorDotProduct(Tuple<double, double, double> v1, Tuple<double, double, double> v2)
		{
			return (v1.Item1 * v2.Item1) + (v1.Item2 * v2.Item2) + (v1.Item3 * v2.Item3);
		}

		public static double rad2deg(double radians)
		{
			return (radians * 180) / Math.PI;
		}

		public static double deg2rad(double degrees)
		{
			return (degrees / 180) * Math.PI;
		}
	}
}
