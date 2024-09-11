using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RoboDk.API;
using RoboDk.API.Exceptions;
using RoboDk.API.Model;

namespace SamplePanelRoboDK
{
    public partial class SampleDialog : Form
    {
        // Define if the robot movements will be blocking
        private const bool MoveBlocking = false;

        private SpashDialog spashDialog;

        // RDK holds the main object to interact with RoboDK.
        // The RoboDK application starts when a RoboDK object is created.
        private IRoboDK _rdk;

        // Keep the ROBOT item as a global variable
        private IItem _robot;

        public SampleDialog()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            this.pnlRDK.Resize += this.Panel_Resized;

            this.FormClosed += this.SampleDialog_FormClosed;

            this.Load += SampleDialog_Load;
            this.Shown += SampleDialog_Shown;
        }

        private void SampleDialog_Shown(object sender, EventArgs e)
        {

            EmbedRoboDK();

            _rdk.ShowRoboDK();

            LoadKukaRobotFile();

            //Hide roboDK internal features:
            //LockRoboDK();

            //Active 3d viewport only:
            //_rdk.SetWindowFlags(WindowFlags.View3DActive);

            //Activate all features:
            _rdk.SetWindowFlags(WindowFlags.All);

            RetreiveJoints();

            HideSplashScreen();

            _rdk.SetSimulationSpeed(1);

        }

        private void HideSplashScreen()
        {
            this.spashDialog.Close();
        }

        private void SampleDialog_Load(object sender, EventArgs e)
        {
            ShowSplashScreen();

            OpenRoboDK();
        }

        private void ShowSplashScreen()
        {
            this.spashDialog = new SpashDialog();

            spashDialog.Show();
            Application.DoEvents();
        }

        private void OpenRoboDK()
        {
            if (!IsRoboDKReady())
            {
                //or let rdk to use exists instance
                KillOldRoboDKProcess();

                var roboDK = new RoboDK(" -NOSPLASH -HIDDEN -ColorBgBottom=black -ColorBgTop=black ");
                
                //if (IsRoboDKAlreadyRunning())
                //{
                //    roboDK.StartNewInstance = false;
                //}

                _rdk = roboDK;

                _rdk.ShowRoboDK();
            }
        }

        private bool IsRoboDKAlreadyRunning()
        {
            return Process.GetProcessesByName("RoboDK").Any();
        }

        private void KillOldRoboDKProcess()
        {
            var p = Process.GetProcessesByName("RoboDK").FirstOrDefault();

            if (p != null)
            {
                p.Kill();
                p.WaitForExit();
            }
        }

        public bool IsRoboDKReady()
        {
            // check if the RDK object has been initialized:
            if (_rdk == null)
            {
                return false;
            }

            // Check if the RDK API is connected
            if (!_rdk.Connected())
            {
                return false;
            }

            return true;
        }

        private void ConnectToRoboDK()
        {
            _rdk.Connect();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        private void EmbedRoboDK()
        {
            // hook window pointer to the integrated panel
            //SetParent(RDK.PROCESS.MainWindowHandle, panel_rdk.Handle);
            SetParent(_rdk.GetWindowHandle(), pnlRDK.Handle);

            //RDK.SetWindowState(RoboDk.API.Model.WindowState.Show); // show RoboDK window if it was hidden
            _rdk.SetWindowState(RoboDk.API.Model.WindowState
                .Cinema); // sets cinema mode (remove the menu, the toolbar, the title bar and the status bar)
            _rdk.SetWindowState(RoboDk.API.Model.WindowState.Fullscreen); // maximizes the screen and sets cinema mode

            // make form height larger
            Size = new Size(Size.Width, 700);


        }

        //load KUKA KRC2 3D model from bi dir
        private void LoadKukaRobotFile()
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "KUKAKRC2102.rdk");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Kuka file not found in {filePath}");
            }

            var item = _rdk.AddFile(filePath);

            if (item.Valid())
            {
                SelectRobot();
            }
            else
            {
                ShowError($@"Could not load: {filePath}");
            }

        }

        /// <summary>
        ///     Update the ROBOT variable by choosing the robot available in the currently open station
        ///     If more than one robot is available, a popup will be displayed
        /// </summary>
        public void SelectRobot()
        {
            _robot = _rdk.ItemUserPick("Select a robot", ItemType.Robot); // select robot among available robots
            //ROBOT = RL.getItem("ABB IRB120", ITEM_TYPE_ROBOT); // select by name
            //ITEM = RL.ItemUserPick("Select an item"); // Select any item in the station
            if (_robot.Valid())
            {
                _robot.NewLink(); // This will create a new communication link (another instance of the RoboDK API), this is useful if we are moving 2 robots at the same time.                
            }
            else
            {
                ShowError(@"Could not select robot. Robot is not available, Load a file first");
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        private void Panel_Resized(object sender, EventArgs e)
        {
            // resize the content of the panel_rdk when it is resized
            MoveWindow(_rdk.GetWindowHandle(), 0, 0, pnlRDK.Width, pnlRDK.Height, true);
        }

        private void SampleDialog_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Force to stop and close RoboDK (optional)
            // RDK.CloseAllStations(); // close all stations
            // RDK.Save("path_to_save.rdk"); // save the project if desired
            _rdk.CloseRoboDK();
            _rdk = null;
        }

        //lock internal robodk feaures
        private void LockRoboDK()
        {
            //RDK.setFlagsRoboDK(RoboDK.FLAG_ROBODK_MENUEDIT_ACTIVE | RoboDK.FLAG_ROBODK_MENUEDIT_ACTIVE);
            _rdk.SetWindowFlags(WindowFlags.None);
            _rdk.SetItemFlags(ItemFlags.None);
            if (_robot != null && _robot.Valid()) _robot.SetItemFlags();
        }

        //unlock internal robodk feaures
        private void UnlockRoboDK()
        {
            _rdk.SetWindowFlags(WindowFlags.All);
            _rdk.SetItemFlags();
            _rdk.ShowRoboDK();

        }

        //get current pos of joints
        private void RetreiveJoints()
        {

            var joints = _robot.Joints();
            var pose = _robot.Pose();

            // update the joints
            var strjoints = Values_2_String(joints);
            txtJoints.Text = strjoints;

            // update the pose as xyzwpr
            var xyzwpr = pose.ToTxyzRxyz();
            var strpose = Values_2_String(xyzwpr);
            txtPosition.Text = strpose;
        }

        private void btnMoveRobotJoint_Click(object sender, EventArgs e)
        {
            // retrieve the robot joints from the text and validate input
            var joints = String_2_Values(txtJoints.Text);

            // make sure RDK is running and we have a valid input
            if (joints == null) return;

            try
            {
                //bool jnts_valid = ROBOT.setJoints(joints, RoboDK.SETJOINTS_SATURATE_APPLY);
                //Console.WriteLine("Robot joints are valid: " + jnts_valid.ToString());
                _robot.MoveJ(joints, MoveBlocking);
            }
            catch (RdkException rdkException)
            {
                ShowError("The robot can't move to " + txtJoints.Text + Environment.NewLine + rdkException.Message);
            }
        }

        private void btnMoveRobotPos_Click(object sender, EventArgs e)
        {
            // retrieve the robot position from the text and validate input
            var xyzwpr = String_2_Values(txtPosition.Text);

            // make sure RDK is running and we have a valid input
            if (xyzwpr == null) return;

            //Mat pose = Mat.FromXYZRPW(xyzwpr);
            var pose = RoboDk.API.Matrix.FromTxyzRxyz(xyzwpr);
            try
            {
                _robot.MoveJ(pose, MoveBlocking);
            }
            catch (RdkException rdkex)
            {
                ShowError("The robot can't move to " + txtPosition.Text + Environment.NewLine + rdkex.Message);
            }
        }

        private void btnMoveRobotLinear_Click(object sender, EventArgs e)
        {
            // retrieve the robot position from the text and validate input
            var xyzwpr = String_2_Values(txtLinearPos.Text);

            // make sure RDK is running and we have a valid input
            if (xyzwpr == null) return;

            RoboDk.API.Matrix pos = RoboDk.API.Matrix.FromXYZRPW(xyzwpr);
            var pose = RoboDk.API.Matrix.FromTxyzRxyz(xyzwpr);
            try
            {
                _robot.MoveL(xyzwpr, MoveBlocking);
            }
            catch (RdkException rdkex)
            {
                ShowError("The robot can't move to " + txtLinearPos.Text + Environment.NewLine + rdkex.Message);
            }
        }

        private void btnMoveRobotPosLinear_Click(object sender, EventArgs e)
        {
            // retrieve the robot position from the text and validate input
            var pose = String_2_Values(txtPosLinear.Text);

            // make sure RDK is running and we have a valid input
            if (pose == null) return;

            //Matrix matrix = Matrix.FromXyzrpw(pose);
            var matrix = RoboDk.API.Matrix.FromTxyzRxyz(pose);

            try
            {
                _robot.MoveL(matrix, MoveBlocking);
            }
            catch (RdkException exception)
            {
                ShowError("The robot can't move to " + txtPosLinear.Text + Environment.NewLine + exception.Message);
            }
        }

        /// <summary>
        ///     Convert a list of numbers provided as a string to an array of values
        /// </summary>
        /// <param name="strvalues"></param>
        /// <returns></returns>
        public double[] String_2_Values(string strvalues)
        {
            double[] dvalues = null;
            try
            {
                dvalues = Array.ConvertAll(strvalues.Split(','), double.Parse);
            }
            catch (FormatException ex)
            {
                ShowError($@"Invalid input: {strvalues}: {ex.Message}");
            }

            return dvalues;
        }

        /// <summary>
        ///     Convert an array of values as a string
        /// </summary>
        /// <param name="dvalues"></param>
        /// <returns></returns>
        public string Values_2_String(double[] dvalues)
        {
            if (dvalues == null || dvalues.Length < 1) return "Invalid values";
            // Not supported on .NET Framework 2.0:
            //string strvalues = String.Join(" , ", dvalues.Select(p => p.ToString("0.0")).ToArray());
            var strvalues = dvalues[0].ToString("0.0");
            for (var i = 1; i < dvalues.Length; i++) strvalues += $" , {dvalues[i]:0.0}";

            return strvalues;
            //return "";
        }

        private void btnMoveToHomePos_Click(object sender, EventArgs e)
        {
            var jointsHome = _robot.JointsHome();

            _robot.MoveJ(jointsHome);
        }

        private void chkRunMode_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRunMode.Tag != null)
            {
                return;
            }

            chkRunMode.Tag = "processing";

            if (chkRunMode.Checked)
            {
                //run on robot:

                // Important: stop any previous program generation (if we selected offline programming mode)
                _rdk.Disconnect();

                // Connect to real robot
                if (_robot.Connect())
                {
                    // Set to Run on Robot robot mode:
                    _rdk.SetRunMode(RunMode.RunRobot);
                }
                else
                {
                    ShowError(@"Can't connect to the robot. Check connection and parameters.");

                    chkRunMode.Checked = false;
                }
            }
            else
            {
                //run in simulation:

                // Important: stop any previous program generation (if we selected offline programming mode)
                _rdk.Disconnect();
                
                // Set to simulation mode:
                _rdk.SetRunMode();
            }

            chkRunMode.Tag = null;
        }

        private void btnReadRobotJoint_Click(object sender, EventArgs e)
        {
            var joints = _robot.Joints();
            // update the joints
            var strjoints = Values_2_String(joints);
            txtJoints.Text = strjoints;
        }

        private void btnReadRobotPos_Click(object sender, EventArgs e)
        {
            // update the pose as xyzwpr
            var pose = _robot.Pose();
            var xyzwpr = pose.ToTxyzRxyz();
            var strpose = Values_2_String(xyzwpr);
            txtPosition.Text = strpose;
        }

        private void btnReadRobotLinearPos_Click(object sender, EventArgs e)
        {
            var joints = _robot.Joints();
            // update the joints
            var strjoints = Values_2_String(joints);
            txtLinearPos.Text = strjoints;
        }

        private void btnReadRobotPosL_Click(object sender, EventArgs e)
        {
            // update the pose
            var pose = _robot.Pose().ToTxyzRxyz();
            txtPosLinear.Text = Values_2_String(pose);
        }

        private void btnConnectToRobot_Click(object sender, EventArgs e)
        {
            ConfigureKukaRobot();
            
            ConnectToRobot();
        }

        private void ConfigureKukaRobot()
        {
            try
            {
                _robot.setConnectionParams(txtRobotIP.Text, int.Parse(txtRobotPort.Text), "C:\\", "morteza", "123");
            }
            catch (Exception ex)
            {
                ShowError("Unable to set the robot connection params" + Environment.NewLine + ex.Message);
            }
        }

        private void ConnectToRobot()
        {
            try
            {
                if (_robot.Connect(txtRobotIP.Text))
                {
                    MessageBox.Show("Connected");
                }
                else
                {
                    var parameters = _robot.ConnectionParams();

                    ShowError($"Unable to connect to the robot\nIpAddress: {parameters.RobotIp}\nPort: {parameters.Port}");
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }
}