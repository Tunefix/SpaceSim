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
			newSpacecraft("TESTCRAFT", 26000, 5);
			spaceitems["TESTCRAFT"].maxOrbitPoints = 72000;
		}

		private void MakeEarth()
		{
			newSpaceItem("EARTH", SpaceItem.SpaceItemType.CENTER_OF_UNIVERSE, 5.974e24, 6371000);
		}

		private void MakeMoon()
		{
			newSpaceItem("MOON", SpaceItem.SpaceItemType.MOON, 7.342e22, 1737400);
			spaceitems["MOON"].position = new Tuple<double, double, double>(362600000, 0, 0);
			spaceitems["MOON"].velocity = new Tuple<double, double, double>(0, 1022, 0);
			spaceitems["MOON"].maxOrbitPoints = 720000;
		}

		private void newSpacecraft(string name, double mass, double radius)
		{
			spaceitems.Add(name, new Spacecraft(name, mass, radius));
		}

		private void newSpaceItem(string name, SpaceItem.SpaceItemType type, double mass, double radius)
		{
			spaceitems.Add(name, new SpaceItem(name, type, mass, radius));
		}
	}
}
