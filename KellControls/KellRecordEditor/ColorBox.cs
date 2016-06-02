using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KellControls
{
    public class ColorBox : UserControl
    {
        public delegate void ColorChangedHandler(object sender, System.Drawing.Color e);
        public event ColorChangedHandler ColorChanged;

        public ColorBox()
        {
            InitializeComponent();
        }

        private void OnColorChanged(System.Drawing.Color e)
        {
            if (ColorChanged != null)
                ColorChanged(this, e);
        }

        public System.Drawing.Color Color
        {
            get
            {
                return this.BackColor;
            }
            set
            {
                this.BackColor = value;
                OnColorChanged(value);
            }
        }
        private ColorDialog colorDialog1;

        private void InitializeComponent()
        {
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.SuspendLayout();
            // 
            // ColorBox
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Name = "ColorBox";
            this.Size = new System.Drawing.Size(21, 21);
            this.Click += new System.EventHandler(this.ColorBox_Click);
            this.ResumeLayout(false);

        }

        private void ColorBox_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = this.BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.BackColor = colorDialog1.Color;
                OnColorChanged(colorDialog1.Color);
            }
            colorDialog1.Dispose();
        }
    }
}
