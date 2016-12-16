namespace Laba
{
    partial class TestGrid
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbRows = new System.Windows.Forms.Label();
            this.lbColumns = new System.Windows.Forms.Label();
            this.tbRows = new System.Windows.Forms.TextBox();
            this.tbColumns = new System.Windows.Forms.TextBox();
            this.tbY = new System.Windows.Forms.TextBox();
            this.tbX = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // lbRows
            // 
            this.lbRows.AutoSize = true;
            this.lbRows.Location = new System.Drawing.Point(25, 42);
            this.lbRows.Name = "lbRows";
            this.lbRows.Size = new System.Drawing.Size(34, 13);
            this.lbRows.TabIndex = 17;
            this.lbRows.Text = "Rows";
            // 
            // lbColumns
            // 
            this.lbColumns.AutoSize = true;
            this.lbColumns.Location = new System.Drawing.Point(12, 15);
            this.lbColumns.Name = "lbColumns";
            this.lbColumns.Size = new System.Drawing.Size(47, 13);
            this.lbColumns.TabIndex = 16;
            this.lbColumns.Text = "Columns";
            this.lbColumns.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // tbRows
            // 
            this.tbRows.Enabled = false;
            this.tbRows.Location = new System.Drawing.Point(65, 39);
            this.tbRows.Name = "tbRows";
            this.tbRows.Size = new System.Drawing.Size(35, 20);
            this.tbRows.TabIndex = 15;
            // 
            // tbColumns
            // 
            this.tbColumns.Enabled = false;
            this.tbColumns.Location = new System.Drawing.Point(65, 12);
            this.tbColumns.Name = "tbColumns";
            this.tbColumns.Size = new System.Drawing.Size(35, 20);
            this.tbColumns.TabIndex = 14;
            // 
            // tbY
            // 
            this.tbY.Enabled = false;
            this.tbY.Location = new System.Drawing.Point(143, 39);
            this.tbY.Name = "tbY";
            this.tbY.Size = new System.Drawing.Size(45, 20);
            this.tbY.TabIndex = 13;
            // 
            // tbX
            // 
            this.tbX.Enabled = false;
            this.tbX.Location = new System.Drawing.Point(143, 13);
            this.tbX.Name = "tbX";
            this.tbX.Size = new System.Drawing.Size(45, 20);
            this.tbX.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Y:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(120, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "X:";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 65);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(205, 299);
            this.panel1.TabIndex = 9;
            // 
            // TestGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbRows);
            this.Controls.Add(this.lbColumns);
            this.Controls.Add(this.tbRows);
            this.Controls.Add(this.tbColumns);
            this.Controls.Add(this.tbY);
            this.Controls.Add(this.tbX);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Name = "TestGrid";
            this.Size = new System.Drawing.Size(519, 439);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TestGrid_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbRows;
        private System.Windows.Forms.Label lbColumns;
        private System.Windows.Forms.TextBox tbRows;
        private System.Windows.Forms.TextBox tbColumns;
        private System.Windows.Forms.TextBox tbY;
        private System.Windows.Forms.TextBox tbX;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
    }
}
