namespace Test
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.validateCode1 = new KellControls.ValidateCode();
            this.loadingCircle1 = new KellControls.LoadingCircle();
            this.winFormPager1 = new KellControls.WinFormPager();
            this.vistaCalendar1 = new KellControls.VistaCalendar();
            this.kellAdvertisingBar1 = new KellControls.KellAdvertisingBar();
            this.kellPermissionEditor1 = new KellControls.KellPermissionEditor();
            this.kellLinkComboBox1 = new KellControls.KellLinkComboBox();
            this.kellCalendarEx1 = new KellCalendarEx.KellCalendarEx();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(98, 100);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "加载权限";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(98, 129);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "查看权限";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(98, 167);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "保存权限";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(98, 196);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 4;
            this.button4.Text = "载入权限";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(115, 61);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(53, 21);
            this.numericUpDown1.TabIndex = 6;
            this.numericUpDown1.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "权限项距：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "像素";
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(115, 33);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(53, 21);
            this.numericUpDown2.TabIndex = 9;
            this.numericUpDown2.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "像素";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "权限名长度：";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(221, 39);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(761, 21);
            this.textBox1.TabIndex = 13;
            this.textBox1.Text = "Data Source=61.147.122.162;Initial Catalog=kell;Persist Security Info=True;Max Po" +
    "ol Size=1000;User ID=kell;Password=118051";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(257, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(95, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "字符串的DES加密";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(221, 86);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(761, 42);
            this.textBox2.TabIndex = 15;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(406, 62);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 16;
            this.button5.Text = "加密v";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(677, 61);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 17;
            this.button6.Text = "解密^";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(13, 306);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(148, 23);
            this.button7.TabIndex = 21;
            this.button7.Text = "ShowWebpage";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(13, 333);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(148, 23);
            this.button8.TabIndex = 20;
            this.button8.Text = "ShowBBS";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(13, 280);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(148, 23);
            this.button9.TabIndex = 19;
            this.button9.Text = "ShowWebsite";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(13, 362);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(148, 23);
            this.button10.TabIndex = 22;
            this.button10.Text = "ShowMovie";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // validateCode1
            // 
            this.validateCode1.BackColor = System.Drawing.Color.Transparent;
            this.validateCode1.CodeType = KellControls.ValidateCode.CodeTypeEnum.字母;
            this.validateCode1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.validateCode1.Location = new System.Drawing.Point(68, 225);
            this.validateCode1.Name = "validateCode1";
            this.validateCode1.Size = new System.Drawing.Size(105, 41);
            this.validateCode1.TabIndex = 26;
            // 
            // loadingCircle1
            // 
            this.loadingCircle1.Active = true;
            this.loadingCircle1.Color = System.Drawing.Color.DarkOrange;
            this.loadingCircle1.InnerCircleRadius = 1;
            this.loadingCircle1.Location = new System.Drawing.Point(423, 225);
            this.loadingCircle1.Name = "loadingCircle1";
            this.loadingCircle1.NumberSpoke = 12;
            this.loadingCircle1.OuterCircleRadius = 34;
            this.loadingCircle1.RotationSpeed = 100;
            this.loadingCircle1.Size = new System.Drawing.Size(83, 83);
            this.loadingCircle1.SpokeThickness = 14;
            this.loadingCircle1.TabIndex = 25;
            this.loadingCircle1.Text = "loadingCircle1";
            // 
            // winFormPager1
            // 
            this.winFormPager1.BackColor = System.Drawing.Color.Transparent;
            this.winFormPager1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("winFormPager1.BackgroundImage")));
            this.winFormPager1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.winFormPager1.Location = new System.Drawing.Point(221, 134);
            this.winFormPager1.Name = "winFormPager1";
            this.winFormPager1.RecordCount = 0;
            this.winFormPager1.Size = new System.Drawing.Size(545, 23);
            this.winFormPager1.TabIndex = 24;
            // 
            // vistaCalendar1
            // 
            this.vistaCalendar1.BackColor = System.Drawing.Color.Transparent;
            this.vistaCalendar1.Location = new System.Drawing.Point(221, 225);
            this.vistaCalendar1.Name = "vistaCalendar1";
            this.vistaCalendar1.Size = new System.Drawing.Size(161, 160);
            this.vistaCalendar1.Style = KellControls.VistaCalendar.VistaCalendarStyle.Blue;
            this.vistaCalendar1.TabIndex = 23;
            // 
            // kellAdvertisingBar1
            // 
            this.kellAdvertisingBar1.AdvFont = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kellAdvertisingBar1.BeginString = "<li>\r\n  <a href=\"";
            this.kellAdvertisingBar1.EndString = "\" target=\"_blank\"";
            this.kellAdvertisingBar1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kellAdvertisingBar1.ListPageUrl = null;
            this.kellAdvertisingBar1.Location = new System.Drawing.Point(279, 173);
            this.kellAdvertisingBar1.Name = "kellAdvertisingBar1";
            this.kellAdvertisingBar1.ReduceLastWords = 5;
            this.kellAdvertisingBar1.Size = new System.Drawing.Size(200, 16);
            this.kellAdvertisingBar1.TabIndex = 18;
            this.kellAdvertisingBar1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.kellAdvertisingBar1_LinkClicked);
            // 
            // kellPermissionEditor1
            // 
            this.kellPermissionEditor1.AutoSize = true;
            this.kellPermissionEditor1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.kellPermissionEditor1.Location = new System.Drawing.Point(0, 0);
            this.kellPermissionEditor1.Name = "kellPermissionEditor1";
            this.kellPermissionEditor1.Permission = null;
            this.kellPermissionEditor1.Size = new System.Drawing.Size(70, 14);
            this.kellPermissionEditor1.TabIndex = 5;
            this.kellPermissionEditor1.PermissionChanged += new KellControls.KellPermissionEditor.PermissionChangedHandler(this.kellPermissionEditor1_PermissionChanged);
            // 
            // kellLinkComboBox1
            // 
            this.kellLinkComboBox1.AddNewType = true;
            this.kellLinkComboBox1.AutoSize = true;
            this.kellLinkComboBox1.Direction = KellControls.KellLinkComboBox.LinkDirection.Horizontal;
            this.kellLinkComboBox1.Loadover = false;
            this.kellLinkComboBox1.Location = new System.Drawing.Point(389, 332);
            this.kellLinkComboBox1.Name = "kellLinkComboBox1";
            this.kellLinkComboBox1.NewItemString = "新增项";
            this.kellLinkComboBox1.Node = null;
            this.kellLinkComboBox1.Size = new System.Drawing.Size(124, 23);
            this.kellLinkComboBox1.TabIndex = 27;
            this.kellLinkComboBox1.UpLevelString = "上一级";
            // 
            // kellCalendarEx1
            // 
            this.kellCalendarEx1.Location = new System.Drawing.Point(259, 12);
            this.kellCalendarEx1.MinimumSize = new System.Drawing.Size(500, 400);
            this.kellCalendarEx1.Name = "kellCalendarEx1";
            this.kellCalendarEx1.Size = new System.Drawing.Size(500, 460);
            this.kellCalendarEx1.TabIndex = 28;
            this.kellCalendarEx1.TodayColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(994, 484);
            this.Controls.Add(this.kellCalendarEx1);
            this.Controls.Add(this.kellLinkComboBox1);
            this.Controls.Add(this.validateCode1);
            this.Controls.Add(this.loadingCircle1);
            this.Controls.Add(this.winFormPager1);
            this.Controls.Add(this.vistaCalendar1);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.kellAdvertisingBar1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.numericUpDown2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.kellPermissionEditor1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Activated += new System.EventHandler(this.Form1_Activated);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private KellControls.KellPermissionEditor kellPermissionEditor1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private KellControls.KellAdvertisingBar kellAdvertisingBar1;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private KellControls.VistaCalendar vistaCalendar1;
        private KellControls.WinFormPager winFormPager1;
        private KellControls.LoadingCircle loadingCircle1;
        private KellControls.ValidateCode validateCode1;
        private KellControls.KellLinkComboBox kellLinkComboBox1;
        private KellCalendarEx.KellCalendarEx kellCalendarEx1;
    }
}

