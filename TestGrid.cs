using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba
{
    public partial class TestGrid : UserControl
    {
        private BitMatrix bm;
        public TestGrid()
        {
            InitializeComponent();

            bm = new BitMatrix(3, 5) { Parent = panel1, Dock = DockStyle.Fill };
            bm.MouseHovered += ch_MouseHovered;

            //this.KeyDown += panel1_KeyDown;
        }

        private void TestGrid_Load(object sender, EventArgs e)
        {
            tbColumns.Text = bm.Width.ToString();
            tbRows.Text = bm.Height.ToString();
        }

        private void TestGrid_KeyDown(object sender, KeyEventArgs e)
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
