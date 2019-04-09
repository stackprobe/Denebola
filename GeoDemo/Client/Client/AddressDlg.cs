using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Charlotte
{
	public partial class AddressDlg : Form
	{
		public AddressDlg()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;
		}

		private void AddressDlg_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void AddressDlg_Shown(object sender, EventArgs e)
		{
			// noop
		}

		private void AddressDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void AddressDlg_FormClosed(object sender, FormClosedEventArgs e)
		{
			Gnd.I.AddressDlg = null;
		}

		private void AddressText_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)1) // ctrl_a
			{
				this.AddressText.SelectAll();
				e.Handled = true;
			}
			else if (e.KeyChar == (char)13) // enter
			{
				if (this.AddressText.Text != "")
				{
					Gnd.I.TileStore.AddressToXY.SetAddress(this.AddressText.Text);
					this.AddressText.Text = "";
				}
				e.Handled = true;
			}
		}
	}
}
