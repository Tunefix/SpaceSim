﻿using System;
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

		// Can make their own force, with like rockets
        public Tuple<double, double, double> selfForce; // in m/s <X, Y, Z>


		public Spacecraft(string name, double mass, double radius) : base (name, SpaceItemType.SPACECRAFT, mass, radius)
		{
			this.type = SpaceItemType.SPACECRAFT;
			// JUST PUT IT IN SOME ORBIT FOR NOW
			this.position = new Tuple<double, double, double>(-6571000, -6571000, 0);
			this.velocity = new Tuple<double, double, double>(7000, -7000, 0);
			this.selfForce = new Tuple<double, double, double>(0,0,0);
			this.mass = mass;
			Cd = 0.5;
			A = 11.95;
		}

		public override void updateVelocity(double deltaTime, Dictionary<string, SpaceItem> items)
		{
			base.updateVelocity(deltaTime, items);

			// Scale Thrust
			Tuple<double, double, double> scaledThrust = Helper.VectorMultiply(selfForce, deltaTime / 1000.0);

			// Add Thrust
			velocity = Helper.VectorAdd(velocity, scaledThrust);


			// AERODYNAMIC DRAG
			/**
			double dAlt = Helper.DensityAtAltitude(altitude);
			double v2 = Math.Pow(Helper.VectorLength(stateVectorV), 2);
			double AtmDrag = 0.5 * ((dAlt * Cd * A) / mass) * v2;
			AtmDrag = AtmDrag * (deltaTime / 1000.0);
			stateVectorV = Helper.VectorMultiply(stateVectorV, 1 - (AtmDrag / Helper.VectorLength(stateVectorV)));
			**/
		}


/**
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
