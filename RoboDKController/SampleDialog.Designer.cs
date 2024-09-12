namespace SamplePanelRoboDK
{
    partial class SampleDialog
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
			this.pnlControl = new System.Windows.Forms.Panel();
			this.btnReadPositionCurve = new System.Windows.Forms.Button();
			this.txtPositionCurveA = new System.Windows.Forms.TextBox();
			this.btnMovePositionCurve = new System.Windows.Forms.Button();
			this.btnReadJointCurve = new System.Windows.Forms.Button();
			this.txtJointCurveA = new System.Windows.Forms.TextBox();
			this.btnMoveJointCurve = new System.Windows.Forms.Button();
			this.btnReadPositionLinear = new System.Windows.Forms.Button();
			this.txtPositionLinear = new System.Windows.Forms.TextBox();
			this.btnMovePositionLinear = new System.Windows.Forms.Button();
			this.btnReadJointLinear = new System.Windows.Forms.Button();
			this.txtJointLinear = new System.Windows.Forms.TextBox();
			this.btnMoveJointLinear = new System.Windows.Forms.Button();
			this.txtRobotPort = new System.Windows.Forms.TextBox();
			this.txtRobotIP = new System.Windows.Forms.TextBox();
			this.btnConnectToRobot = new System.Windows.Forms.Button();
			this.btnReadPosition = new System.Windows.Forms.Button();
			this.btnReadJoint = new System.Windows.Forms.Button();
			this.chkRunMode = new System.Windows.Forms.CheckBox();
			this.btnMoveToHomePosition = new System.Windows.Forms.Button();
			this.txtPosition = new System.Windows.Forms.TextBox();
			this.btnMovePosition = new System.Windows.Forms.Button();
			this.txtJoints = new System.Windows.Forms.TextBox();
			this.btnMoveJoint = new System.Windows.Forms.Button();
			this.pnlRDK = new System.Windows.Forms.Panel();
			this.txtJointCurveB = new System.Windows.Forms.TextBox();
			this.txtPositionCurveB = new System.Windows.Forms.TextBox();
			this.pnlControl.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlControl
			// 
			this.pnlControl.BackColor = System.Drawing.Color.Black;
			this.pnlControl.Controls.Add(this.txtPositionCurveB);
			this.pnlControl.Controls.Add(this.txtJointCurveB);
			this.pnlControl.Controls.Add(this.btnReadPositionCurve);
			this.pnlControl.Controls.Add(this.txtPositionCurveA);
			this.pnlControl.Controls.Add(this.btnMovePositionCurve);
			this.pnlControl.Controls.Add(this.btnReadJointCurve);
			this.pnlControl.Controls.Add(this.txtJointCurveA);
			this.pnlControl.Controls.Add(this.btnMoveJointCurve);
			this.pnlControl.Controls.Add(this.btnReadPositionLinear);
			this.pnlControl.Controls.Add(this.txtPositionLinear);
			this.pnlControl.Controls.Add(this.btnMovePositionLinear);
			this.pnlControl.Controls.Add(this.btnReadJointLinear);
			this.pnlControl.Controls.Add(this.txtJointLinear);
			this.pnlControl.Controls.Add(this.btnMoveJointLinear);
			this.pnlControl.Controls.Add(this.txtRobotPort);
			this.pnlControl.Controls.Add(this.txtRobotIP);
			this.pnlControl.Controls.Add(this.btnConnectToRobot);
			this.pnlControl.Controls.Add(this.btnReadPosition);
			this.pnlControl.Controls.Add(this.btnReadJoint);
			this.pnlControl.Controls.Add(this.chkRunMode);
			this.pnlControl.Controls.Add(this.btnMoveToHomePosition);
			this.pnlControl.Controls.Add(this.txtPosition);
			this.pnlControl.Controls.Add(this.btnMovePosition);
			this.pnlControl.Controls.Add(this.txtJoints);
			this.pnlControl.Controls.Add(this.btnMoveJoint);
			this.pnlControl.Dock = System.Windows.Forms.DockStyle.Top;
			this.pnlControl.Location = new System.Drawing.Point(0, 0);
			this.pnlControl.Name = "pnlControl";
			this.pnlControl.Size = new System.Drawing.Size(1443, 151);
			this.pnlControl.TabIndex = 0;
			// 
			// btnReadPositionCurve
			// 
			this.btnReadPositionCurve.Location = new System.Drawing.Point(1043, 118);
			this.btnReadPositionCurve.Name = "btnReadPositionCurve";
			this.btnReadPositionCurve.Size = new System.Drawing.Size(43, 28);
			this.btnReadPositionCurve.TabIndex = 29;
			this.btnReadPositionCurve.Text = "R";
			this.btnReadPositionCurve.UseVisualStyleBackColor = true;
			this.btnReadPositionCurve.Click += new System.EventHandler(this.btnReadPositionCurve_Click);
			// 
			// txtPositionCurveA
			// 
			this.txtPositionCurveA.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPositionCurveA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.txtPositionCurveA.Location = new System.Drawing.Point(552, 82);
			this.txtPositionCurveA.Name = "txtPositionCurveA";
			this.txtPositionCurveA.Size = new System.Drawing.Size(351, 29);
			this.txtPositionCurveA.TabIndex = 28;
			// 
			// btnMovePositionCurve
			// 
			this.btnMovePositionCurve.Location = new System.Drawing.Point(909, 118);
			this.btnMovePositionCurve.Name = "btnMovePositionCurve";
			this.btnMovePositionCurve.Size = new System.Drawing.Size(128, 27);
			this.btnMovePositionCurve.TabIndex = 27;
			this.btnMovePositionCurve.Text = "Move Position Curve";
			this.btnMovePositionCurve.UseVisualStyleBackColor = true;
			this.btnMovePositionCurve.Click += new System.EventHandler(this.btnMovePositionCurve_Click);
			// 
			// btnReadJointCurve
			// 
			this.btnReadJointCurve.Location = new System.Drawing.Point(503, 118);
			this.btnReadJointCurve.Name = "btnReadJointCurve";
			this.btnReadJointCurve.Size = new System.Drawing.Size(43, 27);
			this.btnReadJointCurve.TabIndex = 26;
			this.btnReadJointCurve.Text = "R";
			this.btnReadJointCurve.UseVisualStyleBackColor = true;
			this.btnReadJointCurve.Click += new System.EventHandler(this.btnReadJointCurve_Click);
			// 
			// txtJointCurveA
			// 
			this.txtJointCurveA.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtJointCurveA.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.txtJointCurveA.Location = new System.Drawing.Point(12, 82);
			this.txtJointCurveA.Name = "txtJointCurveA";
			this.txtJointCurveA.Size = new System.Drawing.Size(351, 29);
			this.txtJointCurveA.TabIndex = 25;
			// 
			// btnMoveJointCurve
			// 
			this.btnMoveJointCurve.Location = new System.Drawing.Point(369, 118);
			this.btnMoveJointCurve.Name = "btnMoveJointCurve";
			this.btnMoveJointCurve.Size = new System.Drawing.Size(128, 27);
			this.btnMoveJointCurve.TabIndex = 24;
			this.btnMoveJointCurve.Text = "Move Joint Curve";
			this.btnMoveJointCurve.UseVisualStyleBackColor = true;
			this.btnMoveJointCurve.Click += new System.EventHandler(this.btnMoveJointCurve_Click);
			// 
			// btnReadPositionLinear
			// 
			this.btnReadPositionLinear.Location = new System.Drawing.Point(1043, 48);
			this.btnReadPositionLinear.Name = "btnReadPositionLinear";
			this.btnReadPositionLinear.Size = new System.Drawing.Size(43, 29);
			this.btnReadPositionLinear.TabIndex = 23;
			this.btnReadPositionLinear.Text = "R";
			this.btnReadPositionLinear.UseVisualStyleBackColor = true;
			this.btnReadPositionLinear.Click += new System.EventHandler(this.btnReadPositionLinear_Click);
			// 
			// txtPositionLinear
			// 
			this.txtPositionLinear.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPositionLinear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.txtPositionLinear.Location = new System.Drawing.Point(552, 47);
			this.txtPositionLinear.Name = "txtPositionLinear";
			this.txtPositionLinear.Size = new System.Drawing.Size(351, 29);
			this.txtPositionLinear.TabIndex = 22;
			// 
			// btnMovePositionLinear
			// 
			this.btnMovePositionLinear.Location = new System.Drawing.Point(909, 48);
			this.btnMovePositionLinear.Name = "btnMovePositionLinear";
			this.btnMovePositionLinear.Size = new System.Drawing.Size(128, 29);
			this.btnMovePositionLinear.TabIndex = 21;
			this.btnMovePositionLinear.Text = "Move Position Linear";
			this.btnMovePositionLinear.UseVisualStyleBackColor = true;
			this.btnMovePositionLinear.Click += new System.EventHandler(this.btnMovePositionLinear_Click);
			// 
			// btnReadJointLinear
			// 
			this.btnReadJointLinear.Location = new System.Drawing.Point(503, 47);
			this.btnReadJointLinear.Name = "btnReadJointLinear";
			this.btnReadJointLinear.Size = new System.Drawing.Size(43, 29);
			this.btnReadJointLinear.TabIndex = 17;
			this.btnReadJointLinear.Text = "R";
			this.btnReadJointLinear.UseVisualStyleBackColor = true;
			this.btnReadJointLinear.Click += new System.EventHandler(this.btnReadJointLinear_Click);
			// 
			// txtJointLinear
			// 
			this.txtJointLinear.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtJointLinear.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.txtJointLinear.Location = new System.Drawing.Point(12, 47);
			this.txtJointLinear.Name = "txtJointLinear";
			this.txtJointLinear.Size = new System.Drawing.Size(351, 29);
			this.txtJointLinear.TabIndex = 16;
			// 
			// btnMoveJointLinear
			// 
			this.btnMoveJointLinear.Location = new System.Drawing.Point(369, 47);
			this.btnMoveJointLinear.Name = "btnMoveJointLinear";
			this.btnMoveJointLinear.Size = new System.Drawing.Size(128, 29);
			this.btnMoveJointLinear.TabIndex = 15;
			this.btnMoveJointLinear.Text = "Move Joint Linear";
			this.btnMoveJointLinear.UseVisualStyleBackColor = true;
			this.btnMoveJointLinear.Click += new System.EventHandler(this.btnMoveJointLinear_Click);
			// 
			// txtRobotPort
			// 
			this.txtRobotPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRobotPort.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtRobotPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.txtRobotPort.Location = new System.Drawing.Point(1229, 48);
			this.txtRobotPort.Name = "txtRobotPort";
			this.txtRobotPort.Size = new System.Drawing.Size(68, 29);
			this.txtRobotPort.TabIndex = 14;
			this.txtRobotPort.Text = "1234";
			// 
			// txtRobotIP
			// 
			this.txtRobotIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.txtRobotIP.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtRobotIP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.txtRobotIP.Location = new System.Drawing.Point(1108, 48);
			this.txtRobotIP.Name = "txtRobotIP";
			this.txtRobotIP.Size = new System.Drawing.Size(115, 29);
			this.txtRobotIP.TabIndex = 13;
			this.txtRobotIP.Text = "10.20.30.40";
			// 
			// btnConnectToRobot
			// 
			this.btnConnectToRobot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnConnectToRobot.Location = new System.Drawing.Point(1303, 48);
			this.btnConnectToRobot.Name = "btnConnectToRobot";
			this.btnConnectToRobot.Size = new System.Drawing.Size(128, 29);
			this.btnConnectToRobot.TabIndex = 12;
			this.btnConnectToRobot.Text = "Connect to Robot";
			this.btnConnectToRobot.UseVisualStyleBackColor = true;
			this.btnConnectToRobot.Click += new System.EventHandler(this.btnConnectToRobot_Click);
			// 
			// btnReadPosition
			// 
			this.btnReadPosition.Location = new System.Drawing.Point(1043, 12);
			this.btnReadPosition.Name = "btnReadPosition";
			this.btnReadPosition.Size = new System.Drawing.Size(43, 29);
			this.btnReadPosition.TabIndex = 11;
			this.btnReadPosition.Text = "R";
			this.btnReadPosition.UseVisualStyleBackColor = true;
			this.btnReadPosition.Click += new System.EventHandler(this.btnReadPosition_Click);
			// 
			// btnReadJoint
			// 
			this.btnReadJoint.Location = new System.Drawing.Point(503, 12);
			this.btnReadJoint.Name = "btnReadJoint";
			this.btnReadJoint.Size = new System.Drawing.Size(43, 29);
			this.btnReadJoint.TabIndex = 10;
			this.btnReadJoint.Text = "R";
			this.btnReadJoint.UseVisualStyleBackColor = true;
			this.btnReadJoint.Click += new System.EventHandler(this.btnReadJoint_Click);
			// 
			// chkRunMode
			// 
			this.chkRunMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.chkRunMode.AutoSize = true;
			this.chkRunMode.ForeColor = System.Drawing.Color.DeepSkyBlue;
			this.chkRunMode.Location = new System.Drawing.Point(1229, 16);
			this.chkRunMode.Name = "chkRunMode";
			this.chkRunMode.Size = new System.Drawing.Size(96, 17);
			this.chkRunMode.TabIndex = 9;
			this.chkRunMode.Text = "Apply to Robot";
			this.chkRunMode.UseVisualStyleBackColor = true;
			this.chkRunMode.CheckedChanged += new System.EventHandler(this.chkRunMode_CheckedChanged);
			// 
			// btnMoveToHomePosition
			// 
			this.btnMoveToHomePosition.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnMoveToHomePosition.Location = new System.Drawing.Point(1331, 12);
			this.btnMoveToHomePosition.Name = "btnMoveToHomePosition";
			this.btnMoveToHomePosition.Size = new System.Drawing.Size(100, 23);
			this.btnMoveToHomePosition.TabIndex = 8;
			this.btnMoveToHomePosition.Text = "Home";
			this.btnMoveToHomePosition.UseVisualStyleBackColor = true;
			this.btnMoveToHomePosition.Click += new System.EventHandler(this.btnMoveToHomePosition_Click);
			// 
			// txtPosition
			// 
			this.txtPosition.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.txtPosition.Location = new System.Drawing.Point(552, 12);
			this.txtPosition.Name = "txtPosition";
			this.txtPosition.Size = new System.Drawing.Size(351, 29);
			this.txtPosition.TabIndex = 7;
			// 
			// btnMovePosition
			// 
			this.btnMovePosition.Location = new System.Drawing.Point(909, 12);
			this.btnMovePosition.Name = "btnMovePosition";
			this.btnMovePosition.Size = new System.Drawing.Size(128, 29);
			this.btnMovePosition.TabIndex = 6;
			this.btnMovePosition.Text = "Move Position";
			this.btnMovePosition.UseVisualStyleBackColor = true;
			this.btnMovePosition.Click += new System.EventHandler(this.btnMovePosition_Click);
			// 
			// txtJoints
			// 
			this.txtJoints.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtJoints.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.txtJoints.Location = new System.Drawing.Point(12, 12);
			this.txtJoints.Name = "txtJoints";
			this.txtJoints.Size = new System.Drawing.Size(351, 29);
			this.txtJoints.TabIndex = 5;
			// 
			// btnMoveJoint
			// 
			this.btnMoveJoint.Location = new System.Drawing.Point(369, 12);
			this.btnMoveJoint.Name = "btnMoveJoint";
			this.btnMoveJoint.Size = new System.Drawing.Size(128, 29);
			this.btnMoveJoint.TabIndex = 4;
			this.btnMoveJoint.Text = "Move Joint";
			this.btnMoveJoint.UseVisualStyleBackColor = true;
			this.btnMoveJoint.Click += new System.EventHandler(this.btnMoveJoint_Click);
			// 
			// pnlRDK
			// 
			this.pnlRDK.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlRDK.Location = new System.Drawing.Point(0, 151);
			this.pnlRDK.Name = "pnlRDK";
			this.pnlRDK.Size = new System.Drawing.Size(1443, 524);
			this.pnlRDK.TabIndex = 0;
			// 
			// txtJointCurveB
			// 
			this.txtJointCurveB.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtJointCurveB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.txtJointCurveB.Location = new System.Drawing.Point(12, 117);
			this.txtJointCurveB.Name = "txtJointCurveB";
			this.txtJointCurveB.Size = new System.Drawing.Size(351, 29);
			this.txtJointCurveB.TabIndex = 30;
			// 
			// txtPositionCurveB
			// 
			this.txtPositionCurveB.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.txtPositionCurveB.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
			this.txtPositionCurveB.Location = new System.Drawing.Point(552, 117);
			this.txtPositionCurveB.Name = "txtPositionCurveB";
			this.txtPositionCurveB.Size = new System.Drawing.Size(351, 29);
			this.txtPositionCurveB.TabIndex = 31;
			// 
			// SampleDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1443, 675);
			this.Controls.Add(this.pnlRDK);
			this.Controls.Add(this.pnlControl);
			this.Name = "SampleDialog";
			this.ShowIcon = false;
			this.Text = "KUKA Control Dialog";
			this.pnlControl.ResumeLayout(false);
			this.pnlControl.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlControl;
        private System.Windows.Forms.Panel pnlRDK;
        private System.Windows.Forms.Button btnMoveJoint;
        private System.Windows.Forms.TextBox txtJoints;
        private System.Windows.Forms.Button btnMovePosition;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Button btnMoveToHomePosition;
        private System.Windows.Forms.CheckBox chkRunMode;
        private System.Windows.Forms.Button btnReadPosition;
        private System.Windows.Forms.Button btnReadJoint;
        private System.Windows.Forms.TextBox txtRobotPort;
        private System.Windows.Forms.TextBox txtRobotIP;
        private System.Windows.Forms.Button btnConnectToRobot;
        private System.Windows.Forms.Button btnReadJointLinear;
        private System.Windows.Forms.TextBox txtJointLinear;
        private System.Windows.Forms.Button btnMoveJointLinear;
        private System.Windows.Forms.Button btnReadPositionLinear;
        private System.Windows.Forms.TextBox txtPositionLinear;
        private System.Windows.Forms.Button btnMovePositionLinear;
		private System.Windows.Forms.Button btnReadPositionCurve;
		private System.Windows.Forms.TextBox txtPositionCurveA;
		private System.Windows.Forms.Button btnMovePositionCurve;
		private System.Windows.Forms.Button btnReadJointCurve;
		private System.Windows.Forms.TextBox txtJointCurveA;
		private System.Windows.Forms.Button btnMoveJointCurve;
		private System.Windows.Forms.TextBox txtJointCurveB;
		private System.Windows.Forms.TextBox txtPositionCurveB;
	}
}