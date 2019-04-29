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
		readonly Pen forbitPen = new Pen(Color.FromArgb(0, 192, 0), 1f);

		List<Tuple<double, double>> orbitPointsXY = new List<Tuple<double, double>>();

		public OrbitVisualizer()
		{
			this.DoubleBuffered = true;
			this.BackColor = Color.FromArgb(0, 0, 0);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.SmoothingMode = SmoothingMode.AntiAlias;
			g.InterpolationMode = InterpolationMode.HighQualityBilinear;
			g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

			
			// DRAW EARTH
			g.FillEllipse(center, (float)((this.Width / 2f) - (earthRadius * scale)), (float)((this.Height / 2f) - (earthRadius * scale)), (float)(2 * (earthRadius * scale)), (float)(2 * (earthRadius * scale)));

			// DRAW ORBIT
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
			}

			// DRAW SPACECRAFT-DOT
			int a = orbitPointsXY.Count - 1;
			g.FillEllipse(craft, (float)(this.Width / 2f) + (float)(orbitPointsXY[a].Item1 * scale) - 1.0f, (float)(this.Height / 2f) + (float)(orbitPointsXY[a].Item2 * -scale) - 1.0f, 2, 2);
		}

		public void AddOrbitPoint(Tuple<double, double, double> point)
		{
			orbitPointsXY.Add(new Tuple<double, double>(point.Item1, point.Item2));

			// Limit point to last 30 min at 1x speed
			if (orbitPointsXY.Count > 108000) orbitPointsXY.RemoveAt(0);

			this.Invalidate();
		}
	}
}
