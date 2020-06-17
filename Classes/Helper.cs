﻿using System;
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

		public static Tuple<double, double, double> VectorNormalize(Tuple<double, double, double> v)
		{
			double maxVal = 0;
			if(Math.Abs(v.Item1) > maxVal) maxVal = Math.Abs(v.Item1);
			if(Math.Abs(v.Item2) > maxVal) maxVal = Math.Abs(v.Item2);
			if(Math.Abs(v.Item3) > maxVal) maxVal = Math.Abs(v.Item3);

			return new Tuple<double, double, double>(v.Item1 / maxVal, v.Item2 / maxVal, v.Item3 / maxVal);
		}

		public static double rad2deg(double radians)
		{
			return (radians * 180) / Math.PI;
		}

		public static double deg2rad(double degrees)
		{
			return (degrees / 180) * Math.PI;
		}

		public static double DensityAtAltitude(double meter)
		{
			/*
			double T = 0;
			double p = 0;
			if(meter < 11000) // Troposhpere
			{
				T = 15.04 - (0.00649 * meter);
				p = 101.29 * Math.Pow((T + 273.1) / 288.08, 5.256);
			}
			else if(meter < 25000) // Lower Stratosphere
			{
				T = -56.46;
				p = 22.65 * Math.Pow(Math.E, 1.73 - (0.000157 * meter));
			}
			else
			{
				T = -131.21 + (0.00299 * meter);
				p = 2.488 * Math.Pow((T + 273.1) / 216.6, -11.388);
			}

			double d = p / (0.2869 * (T + 273.1));
			return d;
			*/

			return 1.3 * Math.Exp(-(meter / 8500));
		}

		public static string doubleToHHHMMSS(double seconds)
		{
			int s = (int)Math.Floor(seconds);

			int m = (int)Math.Floor(s / 60.0);
			s -= m * 60;

			int h = (int)Math.Floor(m / 60.0);
			m -= h * 60;

			string hrs = "000";
			if(h >= 100)
			{
				hrs = h.ToString();
			}
			else if(h >= 10)
			{
				hrs = "0" + h.ToString();
			}
			else if(h > 0)
			{
				hrs = "00" + h.ToString();
			}

			string min = "00";
			if(m >= 10)
			{
				min = m.ToString();
			}
			else if (m > 0)
			{
				min = "0" + m.ToString();
			}

			string sec = "00";
			if(s >= 10)
			{
				sec = s.ToString();
			}
			else if (s > 0)
			{
				sec = "0" + s.ToString();
			}

			return hrs + ":" + min + ":" + sec;
		}
	}
}
