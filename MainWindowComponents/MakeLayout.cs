using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceSim
{
	public partial class MainWindow : Form
	{
		private void MakeLayout()
		{
			lbl_ups = new Label();
			lbl_ups.Location = new System.Drawing.Point(10, 10);
			lbl_ups.Size = new System.Drawing.Size(400, 20);
			lbl_ups.Text = "UPS: ";
			this.Controls.Add(lbl_ups);

			lbl_delta = new Label();
			lbl_delta.Location = new System.Drawing.Point(10, 30);
			lbl_delta.Size = new System.Drawing.Size(400, 20);
			lbl_delta.Text = "Δt: ";
			this.Controls.Add(lbl_delta);

			lbl_gameSpeed = new Label();
			lbl_gameSpeed.Location = new System.Drawing.Point(10, 50);
			lbl_gameSpeed.Size = new System.Drawing.Size(400, 20);
			lbl_gameSpeed.Text = "Δt: ";
			this.Controls.Add(lbl_gameSpeed);

			gameSpeedUp = new Button();
			gameSpeedUp.Location = new System.Drawing.Point(10, 70);
			gameSpeedUp.Size = new System.Drawing.Size(400, 20);
			gameSpeedUp.Text = "GameSpeed Up";
			gameSpeedUp.Click += IncreaseGameSpeed;
			this.Controls.Add(gameSpeedUp);

			gameSpeedDown = new Button();
			gameSpeedDown.Location = new System.Drawing.Point(10, 90);
			gameSpeedDown.Size = new System.Drawing.Size(400, 20);
			gameSpeedDown.Text = "GameSpeed Down";
			gameSpeedDown.Click += DecreaseGameSpeed;
			this.Controls.Add(gameSpeedDown);

			debug = new Label();
			debug.Location = new System.Drawing.Point(420, 10);
			debug.Size = new System.Drawing.Size(200, 400);
			this.Controls.Add(debug);

			preview = new OrbitVisualizer(spaceitems);
			preview.Location = new System.Drawing.Point(10, 110);
			preview.Size = new System.Drawing.Size(400, 400);
			this.Controls.Add(preview);

			ThrustX = new Button();
			ThrustX.Location = new System.Drawing.Point(410, 410);
			ThrustX.Size = new System.Drawing.Size(80, 20);
			ThrustX.Text = "Thrust +X";
			//ThrustX.MouseDown += TrustXOn;
			//ThrustX.MouseUp += TrustXOff;
			this.Controls.Add(ThrustX);

			ThrustNegX = new Button();
			ThrustNegX.Location = new System.Drawing.Point(410, 430);
			ThrustNegX.Size = new System.Drawing.Size(80, 20);
			ThrustNegX.Text = "Thrust -X";
			//ThrustNegX.MouseDown += TrustnegXOn;
			//ThrustNegX.MouseUp += TrustnegXOff;
			this.Controls.Add(ThrustNegX);
		}

		private void IncreaseGameSpeed(object sender, EventArgs e)
		{
			switch(gameSpeed)
			{
				case 1: gameSpeed = 2; break;
				case 2: gameSpeed = 3; break;
				case 3: gameSpeed = 4; break;
				case 4: gameSpeed = 5; break;
				case 5: gameSpeed = 10; break;
				case 10: gameSpeed = 20; break;
				case 20: gameSpeed = 50; break;
				case 50: gameSpeed = 100; break;
				case 100: gameSpeed = 1000; break;
				case 1000: gameSpeed = 1000; break;
			}
		}

		private void DecreaseGameSpeed(object sender, EventArgs e)
		{
			switch (gameSpeed)
			{
				case 1: gameSpeed = 1; break;
				case 2: gameSpeed = 1; break;
				case 3: gameSpeed = 2; break;
				case 4: gameSpeed = 3; break;
				case 5: gameSpeed = 4; break;
				case 10: gameSpeed = 5; break;
				case 20: gameSpeed = 10; break;
				case 50: gameSpeed = 20; break;
				case 100: gameSpeed = 50; break;
				case 1000: gameSpeed = 100; break;
			}
		}

/**
		private void TrustXOn(object sender, EventArgs e) { spaceitems[0].AddTrust(20, 0, 0); }
		private void TrustXOff(object sender, EventArgs e) { spaceitems[0].AddTrust(-20, 0, 0); }
		private void TrustnegXOn(object sender, EventArgs e) { spaceitems[0].AddTrust(-20, 0, 0); }
		private void TrustnegXOff(object sender, EventArgs e) { spaceitems[0].AddTrust(20, 0, 0); }
		**/
	}
}
