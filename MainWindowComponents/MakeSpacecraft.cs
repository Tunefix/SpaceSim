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
		private void MakeSpacecraft()
		{
			spacecrafts.Add(new Spacecraft("TESTCRAFT"));
		}
	}
}
