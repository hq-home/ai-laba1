using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Laba
{
	public enum BitMatrixMouseMode
	{
		Moving,
		Resizing
	}

	public class BitMatrixEventArgs : EventArgs
	{
		public Point ControlPosition;
		public Point Position;
		public bool isInside;
		public BitMatrixMouseMode Mode;

		//Cursor.Position 
	}

	public class BitMatrix : Control
	{
		[DefaultValue(20)]
		public int Indent { get; set; }

		[DefaultValue(20)]
		public int Step { get; set; }

		[DefaultValue(3)]
		public int Width { get; set; }

		[DefaultValue(5)]
		public int Height { get; set; }

		private bool[] _bits = null;
		public bool[] Bits
		{
			get { return _bits; }
		}

		public bool GetAt(int cell, int row)
		{
			if (cell < 0 || cell >= Width) throw new ArgumentOutOfRangeException("cell", cell, "Index is out of range");
			if (row < 0 || row >= Height) throw new ArgumentOutOfRangeException("row", row, "Index is out of range");

			return _bits[row*Width + cell];
		}

		public void SetAt(int cell, int row, bool value)
		{
			if (cell < 0 || cell >= Width) throw new ArgumentOutOfRangeException("cell", cell, "Index is out of range");
			if (row < 0 || row >= Height) throw new ArgumentOutOfRangeException("row", row, "Index is out of range");
			_bits[row * Width + cell] = value;
		}

		public BitMatrix()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
			Step = 20;
			Indent = 20;
			Width = 9;
			Height = 10;
			_bits = new bool[Width*Height];

			this.MouseMove += BitMatrix_MouseHover;
			this.MouseDown += BitMatrix_MouseDown;
			this.MouseUp += BitMatrix_MouseUp;
		}

		private void BitMatrix_MouseUp(object sender, MouseEventArgs e)
		{
			
		}

		public bool IsResizable = true;

		private bool _isInBottomRightCorner = false;

		protected void BitMatrix_MouseDown(object sender, MouseEventArgs e)
		{
			if (isOverGrid)
			{
				switch (_mode)
				{
					case BitMatrixMouseMode.Moving:
						if (ChangeBitState(e.Button, _lastPos))
						{
							this.Invalidate(new Rectangle(_workingRect.Left - 1, _workingRect.Top - 1, _workingRect.Width + 2,
														  _workingRect.Height + 2));
							this.Update();
						}
						break;
					case BitMatrixMouseMode.Resizing:
						break;
				}
				
			}

			if (MouseDownFired != null)
			{
				MouseDownFired(this, new BitMatrixEventArgs() { Position = e.Location, ControlPosition = _lastPos, isInside = isOverGrid, Mode = _mode });
			}	
		}

		private bool ChangeBitState(MouseButtons pressed, Point pos)
		{
			if (IsValidGridPos(pos))
			{
				var index = pos.Y*Width + pos.X;
				var flag = false;
				switch (pressed)
				{
					case MouseButtons.Left:
						flag = true;
						break;
					case MouseButtons.Right:
						break;
					default:
						flag = !_bits[index];
						break;
				}
				var oldflag = _bits[index];
				_bits[index] = flag;

				return flag != oldflag;
			}
			return false;
		}

		public event EventHandler<BitMatrixEventArgs> MouseHovered;
		public event EventHandler<BitMatrixEventArgs> MouseDownFired;
		public event EventHandler<BitMatrixEventArgs> MouseUpFired;

		private bool isOverGrid = false;

		private Point _lastPos = new Point(-1,-1);

		private BitMatrixMouseMode _mode = BitMatrixMouseMode.Moving;

		protected void BitMatrix_MouseHover(object sender, MouseEventArgs e)
		{
			var hasChanges = _workingRect.Contains(e.Location) != isOverGrid;

			if(hasChanges)
			{
				isOverGrid = !isOverGrid;
			}

			Point pos = Point.Empty;
			
			switch (_mode)
			{
				case BitMatrixMouseMode.Moving:
					if (isOverGrid)
					{
						pos = getGridCoords(e.Location);
						if (IsValidGridPos(pos) && pos != _lastPos)
						{
							_lastPos = pos;
							hasChanges = true;

							if (e.Button != MouseButtons.None)
							{
								ChangeBitState(e.Button, _lastPos);
							}
						}

						//if()
					}

					break;
					case BitMatrixMouseMode.Resizing:

					break;
			}

			if (hasChanges)
			{
				this.Invalidate(new Rectangle(_workingRect.Left - 1, _workingRect.Top - 1, _workingRect.Width + 2, _workingRect.Height + 2));
				this.Update();
			}
			if(MouseHovered!= null)
			{
				MouseHovered(this, new BitMatrixEventArgs() { Position = e.Location, ControlPosition = pos, isInside = isOverGrid, Mode = _mode });
			}
		}

		public bool IsValidGridPos(Point p)
		{
			return p.X > -1 && p.Y > -1;
		}

		public bool PointInTriangle(Point p, Region r)
		{
			var pr = new Region();

			/*
			 * A,B,C - точки треугольника, P - точка

N1 = (By-Ay)*(Px-Ax) - (Bx-Ax)*(Py-Ay); 
N2 = (Cy-By)*(Px-Bx) - (Cx-Bx)*(Py-By); 
N3 = (Ay-Cy)*(Px-Cx) - (Ax-Cx)*(Py-Cy);

Result = ((N1>0) and (N2>0)  and (N3>0)) or ((N1<0) and (N2<0) and (N3<0));
			 */
		}

		private Point getGridCoords(Point p)
		{
			int bx = -1, by = -1;

			for(int x=0; x<Width;x++ )
			{
				var lx = _workingRect.Left + x*Step + 1;
				var rx = lx + Step - 2;
				if(p.X >= lx && p.X<= rx)
				{
					bx = x;
					break;
				}
			}

			for (int y = 0; y < Height; y++)
			{
				var ly = _workingRect.Top + y * Step + 1;
				var ry = ly + Step - 2;
				if (p.Y >= ly && p.Y <= ry)
				{
					by = y;
					break;
				}
			}
			return new Point(bx, by);
		}

		private Rectangle _workingRect;
		protected override void OnPaint(PaintEventArgs e)
		{
			var rect = ClientRectangle;
			rect.Width = Step*(Width + 2) + 1;
			rect.Height = Step * (Height + 2) + 1;
			rect.Inflate(-Indent, -Indent);
			_workingRect = rect;
			var step = Step;

			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			using (var font = new Font(Font.FontFamily, 8f))
			using (var pen = new Pen(Color.FromArgb(50, Color.Navy), isOverGrid ? 3 : 1))
			{
				e.Graphics.DrawRectangle(pen, rect);
				pen.Width = 1;
				pen.DashPattern = new float[] { 4.0F, 2.0F};
				int i = 0;
				int dx = -2, dy = 15;

				for (float x = rect.Left + step; x < rect.Right - step; x += step)
				{
					e.Graphics.DrawLine(pen, x, rect.Bottom, x, rect.Top);
				}
				for (float x = rect.Left; x < rect.Right - step; x += step)
				{
					e.Graphics.DrawString(i.ToString(), font, Brushes.Navy, x - dx, rect.Bottom + 5);
					i++;
				}
				for (float y = rect.Top + step; y < rect.Bottom - Step; y += step)
				{
					e.Graphics.DrawLine(pen, rect.Left, y, rect.Right, y);
				}
				i = 0;
				for (float y = rect.Top + step; y < rect.Bottom; y += step)
				{
					e.Graphics.DrawString(i.ToString(), font, Brushes.Navy, rect.Left - 15, y - dy);
					i++;
				}

				var fillBrush = Brushes.Navy;
				for (int x = 0; x < Width; x++)
				{
					for (int y = 0; y < Height; y++)
					{
						if (_bits[y*Width + x])
						{
							e.Graphics.FillRectangle(fillBrush, new Rectangle(_workingRect.Left + x*Step + 2, _workingRect.Top + y*Step + 2, Step - 4, Step - 4));
						}
					}
				}

			}

			if (isOverGrid && IsValidGridPos(_lastPos))
			{
				e.Graphics.DrawRectangle(new Pen(Color.FromArgb(60,0,0,128), 3),
				                  new Rectangle(_workingRect.Left + _lastPos.X*Step, _workingRect.Top + _lastPos.Y*Step, Step, Step));
			}
		}
	}
}
