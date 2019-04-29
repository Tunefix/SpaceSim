using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace SpaceSim
{
	public partial class MainWindow : Form
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void MainWindow_Load(object sender, EventArgs e)
		{
			// MAKE LAYOUT
			MakeLayout();

			// SPAWN SPACEFRAFT
			MakeSpacecraft();

			// Run Sim
			deltaTime = 16;
			MainLoop();
		}

		private async void MainLoop()
		{
			while (true)
			{
				start = DateTime.Now;

				for (int i = 0; i < gameSpeed; i++)
				{
					RunSim();
				}

				UpdateOutputs();

				end = DateTime.Now;
				duration = end - start;


				if (duration.Milliseconds < interval)
				{
					lbl_ups.Text = "UPS: " + (1000f / interval).ToString();
					deltaTime = interval;
					await Task.Delay(new TimeSpan((long)(interval * 10000) - duration.Ticks));
				}
				else if (duration.Milliseconds > (interval * 2))
				{
					lbl_ups.Text = "UPS: " + (1000f / duration.Milliseconds).ToString();
					deltaTime = duration.Ticks / 10000.0;
					Console.WriteLine(end.ToLongTimeString() + " SLOW OPERATION: Last step used " + duration.Milliseconds + "ms.");
				}
				else
				{
					lbl_ups.Text = "UPS: " + (1000f / duration.Milliseconds).ToString();
					deltaTime = duration.Ticks / 10000.0;
				}

				

				lbl_delta.Text = "Δt: " + deltaTime.ToString();
				lbl_gameSpeed.Text = "GameSpeed: " + gameSpeed.ToString() + "x";
			}
		}

		private void RunSim()
		{
			// THIS IS THE MAIN FUNCTION FOR RUNNING THE DIFFERENT PARTS OF THE SIMULATION

			// Handle inputs

			// Update Spacecraft
			foreach(Spacecraft s in spacecrafts)
			{
				s.Update(deltaTime);
			}
		}

		private void UpdateOutputs()
		{
			foreach (Spacecraft s in spacecrafts)
			{
				preview.AddOrbitPoint(s.stateVectorL);
			}

			// Update Outputs
			PrintDebug();
		}

		private void PrintDebug()
		{
			string debugStr = "";
			foreach(Spacecraft s in spacecrafts)
			{
				debugStr += s.name + "\n";
				debugStr += "  Lx: " + s.stateVectorL.Item1.ToString() + "\n";
				debugStr += "  Ly: " + s.stateVectorL.Item2.ToString() + "\n";
				debugStr += "  Lz: " + s.stateVectorL.Item3.ToString() + "\n";
				debugStr += "  Vx: " + s.stateVectorV.Item1.ToString() + "\n";
				debugStr += "  Vy: " + s.stateVectorV.Item2.ToString() + "\n";
				debugStr += "  Vz: " + s.stateVectorV.Item3.ToString() + "\n";
				debugStr += "  Rx: " + s.stateVectorR.Item1.ToString() + "\n";
				debugStr += "  Ry: " + s.stateVectorR.Item2.ToString() + "\n";
				debugStr += "  Rz: " + s.stateVectorR.Item3.ToString() + "\n";
				debugStr += "  Tx: " + s.thrust.Item1.ToString() + "\n";
				debugStr += "  Ty: " + s.thrust.Item2.ToString() + "\n";
				debugStr += "  Tz: " + s.thrust.Item3.ToString() + "\n";
				debugStr += " gee: " + Math.Round(s.gee, 3).ToString() + "\n";
				debugStr += " sma: " + s.sma.ToString() + "\n";
				debugStr += "   p: " + s.p.ToString() + "\n";
				debugStr += "   i: " + s.i.ToString() + "\n";
				debugStr += "   O: " + s.omega.ToString() + "\n";
				debugStr += "argp: " + s.argp.ToString() + "\n";
				debugStr += "  nu: " + s.nu.ToString() + "\n";
				debugStr += " nu°: " + Helper.rad2deg(s.nu).ToString() + "\n";
				debugStr += "   e: " + s.e.ToString() + "\n\n";
				debugStr += " Per: " + Math.Round(s.Periapse).ToString() + "\n";
				debugStr += " Apo: " + Math.Round(s.Apoapse).ToString() + "\n";
				debugStr += " Hgt: " + Math.Round(s.altitude).ToString() + "\n";
				debugStr += "ApoX: " + Math.Round(s.aMin).ToString() + " - " + Math.Round(s.aMax).ToString() + "\n";
				debugStr += "PerX: " + Math.Round(s.pMin).ToString() + " - " + Math.Round(s.pMax).ToString() + "\n";
				debugStr += "AltX: " + Math.Round(s.hMin).ToString() + " - " + Math.Round(s.hMax).ToString() + "\n";

			}

			debug.Text = debugStr;
		}
	}
}
