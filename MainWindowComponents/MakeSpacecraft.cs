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
			spaceitems.Add(new Spacecraft("TESTCRAFT", 26000, 5));
		}

		private void MakeEarth()
		{
			spaceitems.Add(new SpaceItem("EARTH", SpaceItem.SpaceItemType.CENTER_OF_UNIVERSE, 5.974e24, 6371000));
		}
	}
}
