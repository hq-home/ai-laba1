using System;
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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        //private Form1 tab1;
        private Control tab1;
        private void MainForm_Load(object sender, EventArgs e)
        {
            this.KeyPreview = false;
            tab1 = new TestGrid() { Visible=true, Parent = this };

            this.KeyDown += MainForm_KeyDown;

            this.tabPage1.Controls.Add(tab1);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            var t = 0;   
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            /*if (keyData == Keys.Alt)
            {
                return true;
            }
            else*/
                return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
