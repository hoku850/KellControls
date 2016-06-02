namespace KellControls
{
    partial class FloatingCircleLoading
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.关闭载入提示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadingCircle = new KellControls.LoadingCircle();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.关闭载入提示ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(149, 26);
            // 
            // 关闭载入提示ToolStripMenuItem
            // 
            this.关闭载入提示ToolStripMenuItem.Name = "关闭载入提示ToolStripMenuItem";
            this.关闭载入提示ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.关闭载入提示ToolStripMenuItem.Text = "关闭载入提示";
            this.关闭载入提示ToolStripMenuItem.Click += new System.EventHandler(this.关闭载入提示ToolStripMenuItem_Click_1);
            // 
            // LoadingCircle
            // 
            this.LoadingCircle.Active = true;
            this.LoadingCircle.BackColor = System.Drawing.SystemColors.Control;
            this.LoadingCircle.Color = System.Drawing.Color.DarkOrange;
            this.LoadingCircle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoadingCircle.InnerCircleRadius = 21;
            this.LoadingCircle.Location = new System.Drawing.Point(0, 0);
            this.LoadingCircle.Name = "LoadingCircle";
            this.LoadingCircle.NumberSpoke = 12;
            this.LoadingCircle.OuterCircleRadius = 43;
            this.LoadingCircle.RotationSpeed = 100;
            this.LoadingCircle.Size = new System.Drawing.Size(100, 100);
            this.LoadingCircle.SpokeThickness = 14;
            this.LoadingCircle.TabIndex = 0;
            this.LoadingCircle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LoadingCircle_MouseDown);
            this.LoadingCircle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.LoadingCircle_MouseMove);
            this.LoadingCircle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LoadingCircle_MouseUp);
            // 
            // FloatingCircleLoading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(100, 100);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.LoadingCircle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FloatingCircleLoading";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FloatingCircleLoading";
            this.TopMost = true;
            this.TransparencyKey = System.Drawing.SystemColors.Control;
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public LoadingCircle LoadingCircle;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 关闭载入提示ToolStripMenuItem;
    }
}