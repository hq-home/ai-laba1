﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private BitMatrix bm;
		private void Form1_Load(object sender, EventArgs e)
		{
            
            //this.KeyPreview = true;
			bm = new BitMatrix(3, 5) {Parent = panel1, Dock = DockStyle.Fill};
			bm.MouseHovered += ch_MouseHovered;

            this.KeyDown += panel1_KeyDown;
            
			tbColumns.Text = bm.Width.ToString();
			tbRows.Text = bm.Height.ToString();
		}

        protected void panel1_KeyDown(object sender, KeyEventArgs e)
        {
            var s = 0;
        }

		void ch_MouseHovered(object sender, BitMatrixEventArgs e)
		{
			if (e.ControlPosition.X > -1 && e.ControlPosition.Y > -1)
			{
				tbX.Text = e.ControlPosition.X.ToString();
				tbY.Text = e.ControlPosition.Y.ToString();
			}
			tbColumns.Text = bm.Width.ToString();
			tbRows.Text = bm.Height.ToString();
		}
	}
}