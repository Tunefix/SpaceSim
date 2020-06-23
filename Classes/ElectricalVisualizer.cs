using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceSim
{
    class ElectricalVisualizer : Control
    {
        ElectricalComponent component;

        Brush bkgColor = new SolidBrush(Color.FromArgb(255, 16, 16, 16));
        Brush foreColor = new SolidBrush(Color.FromArgb(200, 255, 255, 255));
        Font font = new Font("Arial", 12.0f, FontStyle.Regular);

        public void SetComponent(ElectricalComponent component)
        {
            this.component = component;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Background
            g.FillRectangle(bkgColor, 0, 0, Width, Height);

            // Name, for now
            g.DrawString(component.name, font, foreColor, 10, 10);
        }
    }
}