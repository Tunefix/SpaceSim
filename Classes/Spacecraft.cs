using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceSim
{
	class Spacecraft : SpaceItem
	{
		
		// SPACECRAFT DATA
		public double Cd; // Drag coefficient
		public double A; // Reference area (m²)


		public Spacecraft(string name, double mass, double radius) : base (name, SpaceItemType.SPACECRAFT, mass, radius)
		{
			this.type = SpaceItemType.SPACECRAFT;
			// JUST PUT IT IN SOME ORBIT FOR NOW
			position = new Tuple<double, double, double>(6571000, 0, 1);
			velocity = new Tuple<double, double, double>(0, 7790, 100);
			this.mass = mass;
			Cd = 0.5;
			A = 11.95;
		}

/**
		public void Update(double deltaTime)
		{
			// UPDATE VELOCITY VECTOR

			// Acceleration from Earth (Gravity)
			radius = Math.Sqrt(Math.Pow(stateVectorL.Item1, 2) + Math.Pow(stateVectorL.Item2, 2) + Math.Pow(stateVectorL.Item3, 2));
			altitude = radius - EarthRadius;

			double Acceleration = GravConst * (EarthWeight / Math.Pow(radius, 2));

			double AccZ = (stateVectorL.Item3 / radius) * Acceleration * -1.0;
			double AccY = (stateVectorL.Item2 / radius) * Acceleration * -1.0;
			double AccX = (stateVectorL.Item1 / radius) * Acceleration * -1.0;
			Tuple<double, double, double> AccV = new Tuple<double, double, double>(AccX, AccY, AccZ);

			// Scale Earth acceleration
			AccV = Helper.VectorMultiply(AccV, deltaTime / 1000.0);

			// Add Earth acceleration to velocity vector
			stateVectorV = Helper.VectorAdd(stateVectorV, AccV);

			// Store the velocity vector with only gravity added
			stateVectorVG = stateVectorV;



			// Scale Thrust
			Tuple<double, double, double> scaledThrust = Helper.VectorMultiply(thrust, deltaTime / 1000.0);

			// Add Thrust
			stateVectorV = Helper.VectorAdd(stateVectorV, scaledThrust);


			// AERODYNAMIC DRAG
			double dAlt = Helper.DensityAtAltitude(altitude);
			double v2 = Math.Pow(Helper.VectorLength(stateVectorV), 2);
			double AtmDrag = 0.5 * ((dAlt * Cd * A) / mass) * v2;
			AtmDrag = AtmDrag * (deltaTime / 1000.0);
			stateVectorV = Helper.VectorMultiply(stateVectorV, 1 - (AtmDrag / Helper.VectorLength(stateVectorV)));


			// UPDATE POSITION VECTOR
			Tuple<double, double, double> scaledV = Helper.VectorMultiply(stateVectorV, deltaTime / 1000.0);
			stateVectorL = Helper.VectorAdd(stateVectorL, scaledV);

			// Orbital Momentum Vector
			h = Helper.VectorCrossProduct(stateVectorL, stateVectorV);

			
			// CALCULATE GEE
			gee = Math.Abs((Helper.VectorLength(stateVectorV) - Helper.VectorLength(stateVectorVG)) / (deltaTime / 1000.0));
			gee = gee / 9.80665;

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
		**/
	}
}
