using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceSim
{
	class Spacecraft
	{
		/**
		 * THE VECTORS ARE DEFINED THUSLY:
		 * Item1: X-axis points out from the center of the earth and through the equator at 0°
		 * Item2: Y-axis points out from the center of the earth and through the equator at 90° E
		 * Item3: z-axis points out from the center of the earth and through the north pole
		 * 
		 * The units for Location is meters
		 * The units for Velocity is m/s
		 * The units for Rotation is degrees
		 **/
		
		public Tuple<double, double, double> stateVectorL; // Location/Position
		public Tuple<double, double, double> stateVectorV; // Velocity/Speed
		public Tuple<double, double, double> stateVectorR; // Rotation

		// ADD CLASSIC ORBITAL ELEMENTS HERE
		public double a; // Periapse
		public double p; // Apoapse
		public double sma; // Semi-Major Axis

		public double Periapse;
		public double Apoapse;

		public Tuple<double, double, double> h = new Tuple<double, double, double>(0, 0, 0); // Orbital Momentum Vector
		public Tuple<double, double, double> h0; // Previous Orbital Momentum Vector

		public Tuple<double, double, double> l = new Tuple<double, double, double>(0, 0, 0); // Momentum Vector
		public Tuple<double, double, double> l0; // Previous Momentum Vector

		public double gee;

		public double i;
		public double omega;
		public double argp;
		public double nu;

		public double e;

		public double radius;
		public double altitude;

		public string name;
		public double mass; // Kg

		public double aMax;
		public double aMin;
		public double pMax;
		public double pMin;
		public double hMax;
		public double hMin;
		private bool firstRun = true;

		double EarthWeight = 5.974e24; // kg
		double GravConst = 6.6740831e-11; // N * m²/kg²     G in equations

		double EarthRadius = 6371000; // m


		public Tuple<double, double, double> thrust = new Tuple<double, double, double>(0,0,0);
		public List<Tuple<double, double, double>> FutureOrbitPoints = new List<Tuple<double, double, double>>();


		public Spacecraft(string name)
		{
			this.name = name;
			// JUST PUT IT IN SOME ORBIT FOR NOW
			stateVectorL = new Tuple<double, double, double>(6571000, 0, 1);
			stateVectorV = new Tuple<double, double, double>(0, 7790, 100);
			stateVectorR = new Tuple<double, double, double>(0, 0, 0);
			mass = 26000;
		}

		public void Update(double deltaTime)
		{
			// UPDATE VELOCITY VECTOR

			// Acceleration from Earth (Gravity)
			radius = Math.Sqrt(Math.Pow(stateVectorL.Item1, 2) + Math.Pow(stateVectorL.Item2, 2) + Math.Pow(stateVectorL.Item3, 2));

			double Acceleration = GravConst * (EarthWeight / Math.Pow(radius, 2));

			double AccZ = (stateVectorL.Item3 / radius) * Acceleration * -1.0;
			double AccY = (stateVectorL.Item2 / radius) * Acceleration * -1.0;
			double AccX = (stateVectorL.Item1 / radius) * Acceleration * -1.0;
			Tuple<double, double, double> AccV = new Tuple<double, double, double>(AccX, AccY, AccZ);

			// Scale Earth acceleration
			AccV = Helper.VectorMultiply(AccV, deltaTime / 1000.0);

			// Add Earth acceleration to velocity vector
			stateVectorV = Helper.VectorAdd(stateVectorV, AccV);



			// Scale Thrust
			Tuple<double, double, double> scaledThrust = Helper.VectorMultiply(thrust, deltaTime / 1000.0);

			// Add Thrust
			stateVectorV = Helper.VectorAdd(stateVectorV, scaledThrust);


			// UPDATE POSITION VECTOR
			Tuple<double, double, double> scaledV = Helper.VectorMultiply(stateVectorV, deltaTime / 1000.0);
			stateVectorL = Helper.VectorAdd(stateVectorL, scaledV);

			// Orbital Momentum Vector
			h0 = h;
			h = Helper.VectorCrossProduct(stateVectorL, stateVectorV);

			// Momentum Vector
			l0 = l;
			l = Helper.VectorCrossProduct(stateVectorL, Helper.VectorMultiply(stateVectorV, mass));

			// CALCULATE GEE
			gee = Math.Abs((Helper.VectorLength(l0) - Helper.VectorLength(l)) / (deltaTime / 1000.0));

			DeriveOrbitalElements();
			MinMax();

		}

		public void AddTrust(Tuple<double, double, double> t)
		{
			thrust = Helper.VectorAdd(thrust, t);
		}

		public void AddTrust(double x, double y, double z)
		{
			thrust = Helper.VectorAdd(thrust, new Tuple<double, double, double>(x, y, z));
		}

		private void DeriveOrbitalElements()
		{
			

			Tuple<double, double, double> nhat = Helper.VectorCrossProduct(new Tuple<double, double, double>(0, 0, 1), h);

			double mu = GravConst * EarthWeight;

			radius = Helper.VectorLength(stateVectorL);
			altitude = radius - EarthRadius;

			//Tuple<double, double, double> evec = Helper.VectorMultiply(Helper.VectorSubtract(
			//	Helper.VectorMultiply(stateVectorL, Math.Pow(Helper.VectorLength(stateVectorV), 2) - (Helper.VectorLength(stateVectorL) / mu)),
			//	Helper.VectorMultiply(stateVectorV, Helper.VectorDotProduct(stateVectorL, stateVectorV))
			//	), 1 / mu);

			Tuple<double, double, double> evec = 
				Helper.VectorSubtract(
					Helper.VectorDivide(Helper.VectorCrossProduct(stateVectorV, h), mu),
					Helper.VectorDivide(stateVectorL, Helper.VectorLength(stateVectorL))
				);

			e = Helper.VectorLength(evec);

			double energy = (Math.Pow(Helper.VectorLength(stateVectorV), 2) / 2) - (GravConst / Helper.VectorLength(stateVectorL));

			// SMA: SEMI-MAJOR AXIS
			// P: PERIAPSE
			// A: APOAPSE
			if (Math.Abs(e - 1.0) > 0)
			{
				sma = 1.0 / ((2 / radius) - (Math.Pow(Helper.VectorLength(stateVectorV), 2) / mu));
				p = sma * (1 - e);
				Periapse = p - EarthRadius;
				Apoapse = a - EarthRadius;
				a = sma * (1 + e);
			}
			else
			{
				p = Math.Pow(Helper.VectorLength(h), 2) / GravConst;
				Periapse = p - EarthRadius;
				a = double.PositiveInfinity;
				Apoapse = a;
				sma = double.PositiveInfinity;
			}

			// INCLINATION
			i = Math.Acos(h.Item3 / Helper.VectorLength(h));


			// OMEGA (LONGITUDE OF THE DESCENDING NODE)
			omega = Math.Acos(nhat.Item1 / Helper.VectorLength(nhat));

			if (nhat.Item2 < 0)
			{
				omega = 2 * Math.PI - omega;
			}

			// ARGUMANT OF THE PERIAPSE
			argp = Math.Acos(Helper.VectorDotProduct(nhat, evec) / (Helper.VectorLength(nhat) * e));

			if(evec.Item3 < 0)
			{
				argp = 2 * Math.PI - argp;
			}


			nu = Math.Acos(Helper.VectorDotProduct(evec, stateVectorL) / (e * Helper.VectorLength(stateVectorL)));

			if (Helper.VectorDotProduct(stateVectorL, stateVectorV) < 0)
			{
				nu = 2 * Math.PI - nu;
			}
		}

		private void MinMax()
		{
			if (firstRun)
			{
				firstRun = false;
				aMax = Apoapse;
				aMin = Apoapse;
				pMax = Periapse;
				pMin = Periapse;
				hMax = altitude;
				hMin = altitude;
			}
			else
			{
				if (Apoapse > aMax) aMax = Apoapse;
				if (Apoapse < aMin) aMin = Apoapse;

				if (Periapse > pMax) pMax = Periapse;
				if (Periapse < pMin) pMin = Periapse;

				if (altitude > hMax) hMax = altitude;
				if (altitude < hMin) hMin = altitude;
			}
		}
	}
}
