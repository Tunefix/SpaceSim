using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceSim
{
	class OrbitVisualizer : Control
	{
		readonly double scale = 1.5e-5;
		readonly double earthRadius = 6371000; // m

		readonly Brush center = new SolidBrush(Color.FromArgb(0, 0, 128));
		readonly Brush craft = new SolidBrush(Color.FromArgb(255, 255, 255));
		readonly Pen earthPen = new Pen(Color.FromArgb(0, 0, 255), 1f);
		readonly Pen orbitPen = new Pen(Color.FromArgb(192, 0, 0), 1f);
		readonly Pen orbitPenG = new Pen(Color.FromArgb(0, 192, 0), 1f);

		double iterations = 60 * 60 * 60;

		List<SpaceItem> list;

		public OrbitVisualizer(List<SpaceItem> list)
		{
			this.DoubleBuffered = true;
			this.BackColor = Color.FromArgb(0, 0, 0);
			this.list = list;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBilinear;
			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;


			// DRAW SPACEITEMS
			foreach(SpaceItem s in list)
			{
				g.FillEllipse(center, (float)((this.Width / 2f) - (s.radius * scale)), (float)((this.Height / 2f) - (s.radius * scale)), (float)(2 * (s.radius * scale)), (float)(2 * (s.radius * scale)));

				if(s.type == SpaceItem.SpaceItemType.SPACECRAFT)
				{
					List<Tuple<double, double, double>> future = OrbitHelper.IteratePosition(s, list, 100000);
					for(int i = 0; i < future.Count - 1; i++)
					{
						g.DrawLine(orbitPen,
							(float)((this.Width / 2f) + (future[i].Item1 * scale)),
							(float)((this.Height / 2f) + (future[i].Item2 * -scale)),
							(float)((this.Width / 2f) + (future[i+1].Item1 * scale)),
							(float)((this.Height / 2f) + (future[i+1].Item2 * -scale))
							);
					}

					for(int i = 0; i < s.orbitPoints.Count - 1; i++)
					{
						g.DrawLine(orbitPenG,
							(float)((this.Width / 2f) + (s.orbitPoints[i].Item1 * scale)),
							(float)((this.Height / 2f) + (s.orbitPoints[i].Item2 * -scale)),
							(float)((this.Width / 2f) + (s.orbitPoints[i+1].Item1 * scale)),
							(float)((this.Height / 2f) + (s.orbitPoints[i+1].Item2 * -scale))
							);
					}
				}
			}
			
			// DRAW EARTH
			//g.FillEllipse(center, (float)((this.Width / 2f) - (earthRadius * scale)), (float)((this.Height / 2f) - (earthRadius * scale)), (float)(2 * (earthRadius * scale)), (float)(2 * (earthRadius * scale)));

			// DRAW PAST POSITIONS
			/**
			if(orbitPointsXY.Count > 1)
			{
				for(int i = 0; i < orbitPointsXY.Count - 1; i++)
				{
					g.DrawLine(orbitPen,
						(float)((this.Width / 2f) + (orbitPointsXY[i].Item1 * scale)),
						(float)((this.Height / 2f) + (orbitPointsXY[i].Item2 * -scale)),
						(float)((this.Width / 2f) + (orbitPointsXY[i+1].Item1 * scale)),
						(float)((this.Height / 2f) + (orbitPointsXY[i+1].Item2 * -scale))
						);
				}
			}**/


			// DRAW SPACECRAFT-DOTS
			foreach(SpaceItem s in list)
			{
				if(s.type == SpaceItem.SpaceItemType.SPACECRAFT)
				{
					g.FillEllipse(craft, (float)(this.Width / 2f) + (float)(s.position.Item1 * scale) - 1.0f, (float)(this.Height / 2f) + (float)(s.position.Item2 * -scale) - 1.0f, 2, 2);
				}
			}
			
		}

		public void AddOrbitPoint(Tuple<double, double, double> point)
		{
			/**
			orbitPointsXY.Add(new Tuple<double, double>(point.Item1, point.Item2));

			// Limit point to last 30 min at 1x speed
			if (orbitPointsXY.Count > 108000) orbitPointsXY.RemoveAt(0);

			this.Invalidate();
			**/
		}

		public void UpdateOrbitData(double eccentricity, double semiMajorAxis, double inclination, double longitudeOfAscendingNode, double argumentOfPeriapsis, double trueAnomaly)
		{
			/**
			this.eccentricity = eccentricity;
			this.semiMajorAxis = semiMajorAxis;
			this.inclination = inclination;
			this.longitudeOfAscendingNode = longitudeOfAscendingNode;
			this.argumentOfPeriapsis = argumentOfPeriapsis;
			this.trueAnomaly = trueAnomaly;
			**/
		}
	}
}
