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
		#region [ Constants ]

		public const int MAXWIDTH = 8;
		public const int MAXHEIGHT = 16;
		public const int MINWIDTH = 3;
		public const int MINHEIGHT = 5;

		const float tri_delta = 0.60f * Step;
		const float tri_d = 2.0f;

		
		#endregion
		/// <summary>
		/// Mouse Cursor fly in right-bottom corner of a Grid
		/// </summary>
		private bool _isInBottomRightCorner = false;
		/// <summary>
		/// Mouse Cursor fly over Grid 
		/// </summary>
		private bool isOverGrid = false;
		/// <summary>
		/// Coords of last highlighted position in the Grid
		/// </summary>
		private Point _lastPos = new Point(-1, -1);
		/// <summary>
		/// Mode = simple moving or change Grid size
		/// </summary>
		private BitMatrixMouseMode _mode = BitMatrixMouseMode.Moving;
		/// <summary>
		/// Area of a Grid
		/// </summary>
		private Rectangle _workingRect = Rectangle.Empty;

		#region [ Properties ]

		[DefaultValue(20)]
		public int Indent { get; set; }

		[DefaultValue(20)]
		public int Step { get; set; }

		private int _width;
		[DefaultValue(3)]
		public int Width { 
			get { return _width; }
			set
			{
				if(_width != value)
				{
					var arrayGrow = value > _width;
					_width = value;
					if (arrayGrow) BitsGrow();
					RecalcWorkingRect();
				}
			}
		}

		private int _height;
		[DefaultValue(5)]
		public int Height
		{
			get { return _height; }
			set
			{
				if (_height != value)
				{
					var arrayGrow = value > _height;
					_height = value;
					if (arrayGrow) BitsGrow();
					RecalcWorkingRect();
				}
			}
		}

		/// <summary>
		/// Grid representation as solid bit array
		/// </summary>
		private bool[] _bits = null;
		public bool[] Bits
		{
			get { return _bits; }
		}

		/// <summary>
		/// Allow Grid to be resized
		/// </summary>
		public bool IsResizable = true;

		/// <summary>
		/// Fail safe method to get Grid cell value by row index and cell index is 0-based.
		/// </summary>
		/// <param name="cell">0-based cell index</param>
		/// <param name="row">0-based row index</param>
		/// <returns>boolean value of Grid cell</returns>
		public bool GetAt(int cell, int row)
		{
			if (cell < 0 || cell >= Width) throw new ArgumentOutOfRangeException("cell", cell, "Index is out of range");
			if (row < 0 || row >= Height) throw new ArgumentOutOfRangeException("row", row, "Index is out of range");

			return _bits[row*Width + cell];
		}
		/// <summary>
		/// Fail safe method to set Grid cell value by row index and cell index is 0-based.
		/// </summary>
		/// <param name="cell">0-based cell index</param>
		/// <param name="row">0-based row index</param>
		/// <param name="value">value to set</param>
		public void SetAt(int cell, int row, bool value)
		{
			if (cell < 0 || cell >= Width) throw new ArgumentOutOfRangeException("cell", cell, "Index is out of range");
			if (row < 0 || row >= Height) throw new ArgumentOutOfRangeException("row", row, "Index is out of range");
			_bits[row * Width + cell] = value;
		}

		#endregion

		public BitMatrix()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
			Step = 20;
			Indent = 20;
			Width = 9;
			Height = 10;

			this.MouseMove += BitMatrix_MouseHover;
			this.MouseDown += BitMatrix_MouseDown;
			this.MouseUp += BitMatrix_MouseUp;
		}

		#region [ Internal Events ]
		protected void BitMatrix_MouseUp(object sender, MouseEventArgs e)
		{
            _mode = BitMatrixMouseMode.Moving;
		}

		protected void BitMatrix_MouseDown(object sender, MouseEventArgs e)
		{
			if (isOverGrid)
			{
				switch (_mode)
				{
					case BitMatrixMouseMode.Moving:
						if (IsResizable && _isInBottomRightCorner)
						{
							_mode = BitMatrixMouseMode.Resizing;
						}
						else if (ChangeBitState(e.Button, _lastPos))
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

			SetCursorView();

			if (MouseDownFired != null)
			{
				MouseDownFired(this, new BitMatrixEventArgs() { Position = e.Location, ControlPosition = _lastPos, isInside = isOverGrid, Mode = _mode });
			}
		}

		protected void BitMatrix_MouseHover(object sender, MouseEventArgs e)
		{
			var hasChanges = _workingRect.Contains(e.Location) != isOverGrid;

			if (hasChanges)
			{
				isOverGrid = !isOverGrid;
			}

			Point pos = Point.Empty;

			switch (_mode)
			{
				case BitMatrixMouseMode.Moving:
					if (isOverGrid)
					{
						pos = GetGridCoords(e.Location);
						if (IsValidGridPos(pos) && pos != _lastPos)
						{
							_lastPos = pos;
							hasChanges = true;

							if (e.Button != MouseButtons.None)
							{
								ChangeBitState(e.Button, _lastPos);
							}
						}
					}
					var rect = _workingRect;
					//float delta = 0.60f * Step;

					var pit = PointInTriangle(e.Location,
						new Point((int)(rect.Right - tri_delta), rect.Bottom),
						new Point(rect.Right, (int)(rect.Bottom - tri_delta)),
						new Point(rect.Right, rect.Bottom));

					if (hasChanges |= _isInBottomRightCorner != pit)
					{
						_isInBottomRightCorner = pit;
					}



					break;
				case BitMatrixMouseMode.Resizing:



					break;
			}

			SetCursorView();

			if (hasChanges)
			{
				this.Invalidate(new Rectangle(_workingRect.Left - 1, _workingRect.Top - 1, _workingRect.Width + 2, _workingRect.Height + 2));
				this.Update();
			}

			if (MouseHovered != null)
			{
				MouseHovered(this, new BitMatrixEventArgs() { Position = e.Location, ControlPosition = pos, isInside = isOverGrid, Mode = _mode });
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			var rect = _workingRect;
			var step = Step;

			e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

			using (var font = new Font(Font.FontFamily, 8f))
			using (var pen = new Pen(Color.FromArgb(50, Color.Navy), 1))
			{
				e.Graphics.DrawRectangle(pen, rect);

				pen.DashPattern = new float[] { 4.0F, 2.0F };
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

				if (IsResizable && isOverGrid && _isInBottomRightCorner)
				{
					float delta1 = 0.20f * Step;
					float delta2 = 0.40f * Step;
					float delta3 = 0.60f * Step;
					float d = 2.0f;
					pen.Width = 2;
					pen.DashPattern = new float[] { 64.0f };
					e.Graphics.DrawLine(pen, rect.Right, rect.Bottom + Step / 2, rect.Right, rect.Top);
					e.Graphics.DrawLine(pen, rect.Left, rect.Bottom, rect.Right + Step / 2, rect.Bottom);
					e.Graphics.DrawLine(pen, rect.Right - delta1, rect.Bottom - d, rect.Right - d, rect.Bottom - delta1);
					e.Graphics.DrawLine(pen, rect.Right - delta2, rect.Bottom - d, rect.Right - d, rect.Bottom - delta2);
					e.Graphics.DrawLine(pen, rect.Right - delta3, rect.Bottom - d, rect.Right - d, rect.Bottom - delta3);
				}

				var fillBrush = Brushes.Navy;
				for (int x = 0; x < Width; x++)
				{
					for (int y = 0; y < Height; y++)
					{
						if (_bits[y * Width + x])
						{
							e.Graphics.FillRectangle(fillBrush, new Rectangle(_workingRect.Left + x * Step + 2, _workingRect.Top + y * Step + 2, Step - 4, Step - 4));
						}
					}
				}

			}

			if (isOverGrid && IsValidGridPos(_lastPos))
			{
				e.Graphics.DrawRectangle(new Pen(Color.FromArgb(60, 0, 0, 128), 3),
								  new Rectangle(_workingRect.Left + _lastPos.X * Step, _workingRect.Top + _lastPos.Y * Step, Step, Step));
			}
		}
		#endregion

		#region [ Public Event Hanlers ]
		public event EventHandler<BitMatrixEventArgs> MouseHovered;
		public event EventHandler<BitMatrixEventArgs> MouseDownFired;
		public event EventHandler<BitMatrixEventArgs> MouseUpFired;
		#endregion

		#region [ Helper Methods ]
		private void BitsGrow()
		{
			var b = new bool[Height * Step + Width];
			if (_bits != null)
				Array.Copy(_bits, b, _bits.Length);
			_bits = b;
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

		private void RecalcWorkingRect()
		{
			_workingRect = ClientRectangle;
			_workingRect.Height = Step * (Height + 2) + 1;
			_workingRect.Width = Step * (Width + 2) + 1;
			_workingRect.Inflate(-Indent, -Indent);
		}

		private void SetCursorView()
		{
			if (IsResizable && _isInBottomRightCorner)
				Cursor.Current = Cursors.SizeNWSE;
			else if (isOverGrid)
				Cursor.Current = Cursors.Hand;
			else Cursor.Current = Cursors.Arrow;
		}

		public bool IsValidGridPos(Point p)
		{
			return p.X > -1 && p.Y > -1;
		}

		public bool PointInTriangle(Point p, Point a, Point b, Point c)
		{
            var n1 = (b.Y - a.Y) * (p.X - a.X) - (b.X - a.X) * (p.Y - a.Y);
            var n2 = (c.Y - b.Y) * (p.X - b.X) - (c.X - b.X) * (p.Y - b.Y);
            var n3 = (a.Y - c.Y) * (p.X - c.X) - (a.X - c.X) * (p.Y - c.Y);
            return ((n1 >= 0) && (n2 >= 0) && (n3 >= 0)) || ((n1 <= 0) && (n2 <= 0) && (n3 <= 0));
        }

		private Point GetGridCoords(Point p)
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
		#endregion
	}
}
