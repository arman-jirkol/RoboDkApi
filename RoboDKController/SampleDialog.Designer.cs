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
            this.chkRunMode = new System.Windows.Forms.CheckBox();
            this.btnMoveToHomePos = new System.Windows.Forms.Button();
            this.txtPosition = new System.Windows.Forms.TextBox();
            this.btnMoveRobotPos = new System.Windows.Forms.Button();
            this.txtJoints = new System.Windows.Forms.TextBox();
            this.btnMoveRobotJoint = new System.Windows.Forms.Button();
            this.pnlRDK = new System.Windows.Forms.Panel();
            this.btnReadRobotPos = new System.Windows.Forms.Button();
            this.btnReadRobotJoint = new System.Windows.Forms.Button();
            this.txtRobotIP = new System.Windows.Forms.TextBox();
            this.btnConnectToRobot = new System.Windows.Forms.Button();
            this.txtRobotPort = new System.Windows.Forms.TextBox();
            this.pnlControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlControl
            // 
            this.pnlControl.BackColor = System.Drawing.Color.Black;
            this.pnlControl.Controls.Add(this.txtRobotPort);
            this.pnlControl.Controls.Add(this.txtRobotIP);
            this.pnlControl.Controls.Add(this.btnConnectToRobot);
            this.pnlControl.Controls.Add(this.btnReadRobotPos);
            this.pnlControl.Controls.Add(this.btnReadRobotJoint);
            this.pnlControl.Controls.Add(this.chkRunMode);
            this.pnlControl.Controls.Add(this.btnMoveToHomePos);
            this.pnlControl.Controls.Add(this.txtPosition);
            this.pnlControl.Controls.Add(this.btnMoveRobotPos);
            this.pnlControl.Controls.Add(this.txtJoints);
            this.pnlControl.Controls.Add(this.btnMoveRobotJoint);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControl.Location = new System.Drawing.Point(0, 0);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(1217, 87);
            this.pnlControl.TabIndex = 0;
            // 
            // chkRunMode
            // 
            this.chkRunMode.AutoSize = true;
            this.chkRunMode.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.chkRunMode.Location = new System.Drawing.Point(1092, 55);
            this.chkRunMode.Name = "chkRunMode";
            this.chkRunMode.Size = new System.Drawing.Size(96, 17);
            this.chkRunMode.TabIndex = 9;
            this.chkRunMode.Text = "Apply to Robot";
            this.chkRunMode.UseVisualStyleBackColor = true;
            this.chkRunMode.CheckedChanged += new System.EventHandler(this.chkRunMode_CheckedChanged);
            // 
            // btnMoveToHomePos
            // 
            this.btnMoveToHomePos.Location = new System.Drawing.Point(1092, 12);
            this.btnMoveToHomePos.Name = "btnMoveToHomePos";
            this.btnMoveToHomePos.Size = new System.Drawing.Size(100, 23);
            this.btnMoveToHomePos.TabIndex = 8;
            this.btnMoveToHomePos.Text = "Home";
            this.btnMoveToHomePos.UseVisualStyleBackColor = true;
            this.btnMoveToHomePos.Click += new System.EventHandler(this.btnMoveToHomePos_Click);
            // 
            // txtPosition
            // 
            this.txtPosition.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPosition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtPosition.Location = new System.Drawing.Point(12, 42);
            this.txtPosition.Name = "txtPosition";
            this.txtPosition.Size = new System.Drawing.Size(269, 29);
            this.txtPosition.TabIndex = 7;
            // 
            // btnMoveRobotPos
            // 
            this.btnMoveRobotPos.Location = new System.Drawing.Point(287, 42);
            this.btnMoveRobotPos.Name = "btnMoveRobotPos";
            this.btnMoveRobotPos.Size = new System.Drawing.Size(128, 29);
            this.btnMoveRobotPos.TabIndex = 6;
            this.btnMoveRobotPos.Text = "Move Robot Pos";
            this.btnMoveRobotPos.UseVisualStyleBackColor = true;
            this.btnMoveRobotPos.Click += new System.EventHandler(this.btnMoveRobotPos_Click);
            // 
            // txtJoints
            // 
            this.txtJoints.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtJoints.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtJoints.Location = new System.Drawing.Point(12, 12);
            this.txtJoints.Name = "txtJoints";
            this.txtJoints.Size = new System.Drawing.Size(269, 29);
            this.txtJoints.TabIndex = 5;
            // 
            // btnMoveRobotJoint
            // 
            this.btnMoveRobotJoint.Location = new System.Drawing.Point(287, 12);
            this.btnMoveRobotJoint.Name = "btnMoveRobotJoint";
            this.btnMoveRobotJoint.Size = new System.Drawing.Size(128, 29);
            this.btnMoveRobotJoint.TabIndex = 4;
            this.btnMoveRobotJoint.Text = "Move Robot Joint";
            this.btnMoveRobotJoint.UseVisualStyleBackColor = true;
            this.btnMoveRobotJoint.Click += new System.EventHandler(this.btnMoveRobotJoint_Click);
            // 
            // pnlRDK
            // 
            this.pnlRDK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRDK.Location = new System.Drawing.Point(0, 87);
            this.pnlRDK.Name = "pnlRDK";
            this.pnlRDK.Size = new System.Drawing.Size(1217, 588);
            this.pnlRDK.TabIndex = 0;
            // 
            // btnReadRobotPos
            // 
            this.btnReadRobotPos.Location = new System.Drawing.Point(421, 42);
            this.btnReadRobotPos.Name = "btnReadRobotPos";
            this.btnReadRobotPos.Size = new System.Drawing.Size(43, 29);
            this.btnReadRobotPos.TabIndex = 11;
            this.btnReadRobotPos.Text = "R";
            this.btnReadRobotPos.UseVisualStyleBackColor = true;
            this.btnReadRobotPos.Click += new System.EventHandler(this.btnReadRobotPos_Click);
            // 
            // btnReadRobotJoint
            // 
            this.btnReadRobotJoint.Location = new System.Drawing.Point(421, 12);
            this.btnReadRobotJoint.Name = "btnReadRobotJoint";
            this.btnReadRobotJoint.Size = new System.Drawing.Size(43, 29);
            this.btnReadRobotJoint.TabIndex = 10;
            this.btnReadRobotJoint.Text = "R";
            this.btnReadRobotJoint.UseVisualStyleBackColor = true;
            this.btnReadRobotJoint.Click += new System.EventHandler(this.btnReadRobotJoint_Click);
            // 
            // txtRobotIP
            // 
            this.txtRobotIP.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRobotIP.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtRobotIP.Location = new System.Drawing.Point(488, 11);
            this.txtRobotIP.Name = "txtRobotIP";
            this.txtRobotIP.Size = new System.Drawing.Size(115, 29);
            this.txtRobotIP.TabIndex = 13;
            this.txtRobotIP.Text = "10.20.30.40";
            // 
            // btnConnectToRobot
            // 
            this.btnConnectToRobot.Location = new System.Drawing.Point(683, 11);
            this.btnConnectToRobot.Name = "btnConnectToRobot";
            this.btnConnectToRobot.Size = new System.Drawing.Size(128, 29);
            this.btnConnectToRobot.TabIndex = 12;
            this.btnConnectToRobot.Text = "Connect to Robot";
            this.btnConnectToRobot.UseVisualStyleBackColor = true;
            this.btnConnectToRobot.Click += new System.EventHandler(this.btnConnectToRobot_Click);
            // 
            // txtRobotPort
            // 
            this.txtRobotPort.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRobotPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtRobotPort.Location = new System.Drawing.Point(609, 11);
            this.txtRobotPort.Name = "txtRobotPort";
            this.txtRobotPort.Size = new System.Drawing.Size(68, 29);
            this.txtRobotPort.TabIndex = 14;
            this.txtRobotPort.Text = "1234";
            // 
            // SampleDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 675);
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
        private System.Windows.Forms.Button btnMoveRobotJoint;
        private System.Windows.Forms.TextBox txtJoints;
        private System.Windows.Forms.Button btnMoveRobotPos;
        private System.Windows.Forms.TextBox txtPosition;
        private System.Windows.Forms.Button btnMoveToHomePos;
        private System.Windows.Forms.CheckBox chkRunMode;
        private System.Windows.Forms.Button btnReadRobotPos;
        private System.Windows.Forms.Button btnReadRobotJoint;
        private System.Windows.Forms.TextBox txtRobotPort;
        private System.Windows.Forms.TextBox txtRobotIP;
        private System.Windows.Forms.Button btnConnectToRobot;
    }
}