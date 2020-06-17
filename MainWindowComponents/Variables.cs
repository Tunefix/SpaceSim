using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceSim
{
	public partial class MainWindow : Form
	{
		double interval = 1000.0 / 60; // Tick-length in milliseconds
		double deltaTime;
		DateTime start;
		DateTime end;
		TimeSpan duration;

		int gameSpeed = 1;

		private Label lbl_ups;
		private Label lbl_delta;
		private Label lbl_gameSpeed;
		private Button gameSpeedUp;
		private Button gameSpeedDown;
		private OrbitVisualizer preview;

		private Button ThrustX;
		private Button ThrustNegX;

		private Label debug;

		private List<SpaceItem> spaceitems = new List<SpaceItem>();
	}
}
