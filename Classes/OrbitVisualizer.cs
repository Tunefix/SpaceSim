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
		public double scale = 1.5e-5;

		double offsetX = 5000000;
		double offsetY = 0;
		readonly Brush center = new SolidBrush(Color.FromArgb(0, 0, 128));
		readonly Brush craft = new SolidBrush(Color.FromArgb(255, 255, 255));
		readonly Pen earthPen = new Pen(Color.FromArgb(0, 0, 255), 1f);
		readonly Pen orbitPen = new Pen(Color.FromArgb(192, 0, 0), 1f);
		readonly Pen orbitPenG = new Pen(Color.FromArgb(0, 192, 0), 1f);

		int iterations = 60;

		bool mouseDown = false;
		Point dragStart;

		Dictionary<string, SpaceItem> list;

		public OrbitVisualizer(Dictionary<string, SpaceItem> list)
		{
			this.DoubleBuffered = true;
			this.BackColor = Color.FromArgb(0, 0, 0);
			this.list = list;
		}

		public double GetScale()
		{
			return scale;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBilinear;
			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;


			// DRAW SPACEITEMS
			foreach(KeyValuePair<string, SpaceItem> kvp in list)
			{
				SpaceItem s = kvp.Value;
				float cX = (float)((this.Width / 2f) + (s.position.Item1 * scale) - (s.radius * scale) + (offsetX * scale));
				float cY = (float)((this.Height / 2f) - (s.position.Item2 * scale) - (s.radius * scale) + (offsetY * scale));
				float size = (float)(2 * (s.radius * scale));
				g.FillEllipse(center, cX, cY, size, size);

				if(s.type == SpaceItem.SpaceItemType.SPACECRAFT || s.type == SpaceItem.SpaceItemType.MOON)
				{
					List<Tuple<double, double, double>> future = OrbitHelper.IteratePosition(s, list, iterations);
					for(int i = 0; i < future.Count - 1; i++)
					{
						g.DrawLine(orbitPen,
							(float)((this.Width / 2f) + (future[i].Item1 * scale)),
							(float)((this.Height / 2f) + (future[i].Item2 * -scale)),
							(float)((this.Width / 2f) + (future[i+1].Item1 * scale)),
							(float)((this.Height / 2f) + (future[i+1].Item2 * -scale))
							);
					}

					List<Tuple<double, double, double>> points = s.orbitPoints.ToList();
					PointF[] pointsXY = new PointF[points.Count];
					for(int i = 0; i < points.Count; i++)
					{
						pointsXY[i] = new PointF(
							(float)((this.Width / 2f) + (points[i].Item1 * scale) + (offsetX * scale)),
							(float)((this.Height / 2f) + (points[i].Item2 * -scale) + (offsetY * scale))
						);
					}
					if(pointsXY.Count() >= 2)
					{
						g.DrawLines(orbitPenG, pointsXY);
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
			foreach(KeyValuePair<string, SpaceItem> kvp in list)
			{
				SpaceItem s = kvp.Value;
				if(s.type == SpaceItem.SpaceItemType.SPACECRAFT)
				{
					float size = 2f;
					float cX = (float)((this.Width / 2f) + (s.position.Item1 * scale) - (size / 2f) + (offsetX * scale));
					float cY = (float)((this.Height / 2f) - (s.position.Item2 * scale) - (size / 2f) + (offsetY * scale));
					
					g.FillEllipse(craft, cX, cY, size, size);
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

		protected override void OnMouseWheel(MouseEventArgs e)
		{
			int clicks = e.Delta / SystemInformation.MouseWheelScrollDelta;
			
			double factor = 2;

			if(clicks > 0)
			{
				scale = scale * factor * clicks;
			}
			else
			{
				scale = scale / (factor * -clicks);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			mouseDown = true;
			dragStart = e.Location;
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			mouseDown = false;
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			if(mouseDown)
			{
				int dX = e.X - dragStart.X;
				int dY = e.Y - dragStart.Y;

				offsetX += dX / scale;
				offsetY += dY / scale;

				dragStart.X = e.X;
				dragStart.Y = e.Y;
			}
		}
	}
}
