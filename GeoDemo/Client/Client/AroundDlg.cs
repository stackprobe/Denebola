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
	public partial class AroundDlg : Form
	{
		public string Ret_CircleR;
		public bool OkPressed = false;

		public AroundDlg()
		{
			InitializeComponent();

			this.MinimumSize = this.Size;
		}

		private void AroundDlg_Load(object sender, EventArgs e)
		{
			// noop
		}

		private void AroundDlg_Shown(object sender, EventArgs e)
		{
			// noop
		}

		private void AroundDlg_FormClosing(object sender, FormClosingEventArgs e)
		{
			// noop
		}

		private void AroundDlg_FormClosed(object sender, FormClosedEventArgs e)
		{
			// noop
		}

		private void CancelBtn_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void OkBtn_Click(object sender, EventArgs e)
		{
			this.Ret_CircleR = this.CircleR.Text;
			this.OkPressed = true;

			this.Close();
		}

		private void CircleR_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)1) // ctrl_a
			{
				this.CircleR.SelectAll();
				e.Handled = true;
			}
			else if (e.KeyChar == (char)13) // enter
			{
				this.OkBtn.Focus();
				e.Handled = true;
			}
		}
	}
}
