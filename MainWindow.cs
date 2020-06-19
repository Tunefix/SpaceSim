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
			this.SuspendLayout();
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Name = "MainWindow";
			this.Text = "SpaceSim";
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.ResumeLayout(false);
		}

		private void MainWindow_Load(object sender, EventArgs e)
		{
			// MAKE LAYOUT
			MakeLayout();

			// MAKE EARTH
			MakeEarth();

			// MAKE MOON
			MakeMoon();

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

				MET += (deltaTime / 1000.0) * gameSpeed;
				lbl_MET.Text = "MET: " + Helper.doubleToHHHMMSS(MET);

				lbl_delta.Text = "Δt: " + deltaTime.ToString();
				lbl_gameSpeed.Text = "GameSpeed: " + gameSpeed.ToString() + "x";

				preview.Invalidate();
			}
		}

		private void RunSim()
		{
			// THIS IS THE MAIN FUNCTION FOR RUNNING THE DIFFERENT PARTS OF THE SIMULATION

			// Handle inputs

			// Update Spaceitems
			foreach(KeyValuePair<string, SpaceItem> kvp in spaceitems)
			{
				kvp.Value.updateVelocity(deltaTime, spaceitems);
			}

			// UPDATE ALL ITEMS POSITIONS AND STORE POSITION OF CENTER OF UNIVERSE
			Tuple<double, double, double> coo = new Tuple<double, double, double>(0,0,0);
			foreach(KeyValuePair<string, SpaceItem> kvp in spaceitems)
			{
				kvp.Value.updatePosition(deltaTime);
				if(kvp.Value.type == SpaceItem.SpaceItemType.CENTER_OF_UNIVERSE)
				{
					coo = new Tuple<double, double, double>(kvp.Value.position.Item1, kvp.Value.position.Item2, kvp.Value.position.Item3);
				}
			}

			// MOVE ALL ITEMS BY THE INVERSE OF CENTER OF UNIVERSE'S POSITION
			// SO THAT COU STAYS AT 0,0,0
			foreach(KeyValuePair<string, SpaceItem> kvp in spaceitems)
			{
				kvp.Value.Move(-coo.Item1, -coo.Item2, -coo.Item3);
			}
		}

		private void UpdateOutputs()
		{
			/**
			foreach (Spacecraft s in spacecrafts)
			{
				preview.AddOrbitPoint(s.stateVectorL);
				preview.UpdateOrbitData(s.e, s.sma, s.i, s.omega, s.argp, s.nu);
			}
			**/

			// Update Outputs
			PrintDebug();
		}

		private void PrintDebug()
		{
			string debugStr = "";
			
			foreach(KeyValuePair<string, SpaceItem> kvp in spaceitems)
			{
				SpaceItem s = kvp.Value;
				debugStr += s.name + "\n";
				debugStr += "  Lx: " + s.position.Item1.ToString() + "\n";
				debugStr += "  Ly: " + s.position.Item2.ToString() + "\n";
				debugStr += "  Lz: " + s.position.Item3.ToString() + "\n";
				debugStr += "  Vx: " + s.velocity.Item1.ToString() + "\n";
				debugStr += "  Vy: " + s.velocity.Item2.ToString() + "\n";
				debugStr += "  Vz: " + s.velocity.Item3.ToString() + "\n";
				debugStr += "  Vt: " + Helper.VectorLength(s.velocity).ToString() + "\n";
				debugStr += "\n";
				debugStr += "  SL: " + (s.position.Item1 * preview.scale).ToString() + "\n";
				debugStr += "  Pc: " + s.orbitPoints.Count.ToString() + "\n";
				//debugStr += "  Rx: " + s.stateVectorR.Item1.ToString() + "\n";
				//debugStr += "  Ry: " + s.stateVectorR.Item2.ToString() + "\n";
				//debugStr += "  Rz: " + s.stateVectorR.Item3.ToString() + "\n";
				//debugStr += "  Tx: " + s.thrust.Item1.ToString() + "\n";
				//debugStr += "  Ty: " + s.thrust.Item2.ToString() + "\n";
				//debugStr += "  Tz: " + s.thrust.Item3.ToString() + "\n";
				/**debugStr += " gee: " + Math.Round(s.gee, 3).ToString() + "\n";
				debugStr += " sma: " + s.sma.ToString() + "\n";
				debugStr += "   p: " + s.p.ToString() + "\n";
				debugStr += "   i: " + s.i.ToString() + "\n";
				debugStr += "   O: " + s.omega.ToString() + "\n";
				debugStr += "argp: " + s.argp.ToString() + "\n";
				debugStr += "  nu: " + s.nu.ToString() + "\n";
				debugStr += " nu°: " + Helper.rad2deg(s.nu).ToString() + "\n";
				debugStr += "   e: " + s.e.ToString() + "\n";
				debugStr += "   d: " + Helper.DensityAtAltitude(s.altitude).ToString() + "\n";
				debugStr += " Per: " + Math.Round(s.Periapse).ToString() + "\n";
				debugStr += " Apo: " + Math.Round(s.Apoapse).ToString() + "\n";
				debugStr += " Hgt: " + Math.Round(s.altitude).ToString() + "\n";
				debugStr += "ApoX: " + Math.Round(s.aMin).ToString() + " - " + Math.Round(s.aMax).ToString() + "\n";
				debugStr += "PerX: " + Math.Round(s.pMin).ToString() + " - " + Math.Round(s.pMax).ToString() + "\n";
				debugStr += "AltX: " + Math.Round(s.hMin).ToString() + " - " + Math.Round(s.hMax).ToString() + "\n";
				/**/
			}

			debugStr += "\n";
			debugStr += "Scale: " + preview.GetScale().ToString() + "\n";
			debug.Text = debugStr;
		}
	}
}
