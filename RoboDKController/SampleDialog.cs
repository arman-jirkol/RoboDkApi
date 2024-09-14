using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using RoboDk.API;
using RoboDk.API.Exceptions;
using RoboDk.API.Model;

namespace SamplePanelRoboDK
{
    public partial class SampleDialog : Form
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

        // Define if the robot movements will be blocking
        private const bool MoveBlocking = false;

        private SplashDialog _splashDialog;

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

        private void HideSplashScreen()
        {
            this._splashDialog.Close();
        }

        private void ShowSplashScreen()
        {
            this._splashDialog = new SplashDialog();

            _splashDialog.Show();
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

        private void ConfigureKukaRobot()
        {
            try
            {
                _robot.SetConnectionParams(txtRobotIP.Text, int.Parse(txtRobotPort.Text), "C:\\", "morteza", "123");
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
                    MessageBox.Show(@"Connected");
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

        //private bool IsRoboDKAlreadyRunning()
        //{
        //    return Process.GetProcessesByName("RoboDK").Any();
        //}

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

        //private void ConnectToRoboDK()
        //{
        //    _rdk.Connect();
        //}

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
            var rdkProjectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "KUKAKRC2103.rdk");

            if (!File.Exists(rdkProjectPath))
            {
                throw new FileNotFoundException($"Kuka file not found in {rdkProjectPath}");
            }

            var item = _rdk.AddFile(rdkProjectPath);

            var axis3DModelPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Axis.fbx");

            if (File.Exists(axis3DModelPath))
            {
                item = _rdk.AddFile(axis3DModelPath);
            }

            if (item.Valid())
            {
                SelectRobot();
            }
            else
            {
                ShowError($@"Could not load: {rdkProjectPath}");
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

        ////lock internal roboDK features
        //private void LockRoboDK()
        //{
        //    //RDK.setFlagsRoboDK(RoboDK.FLAG_ROBODK_MENUEDIT_ACTIVE | RoboDK.FLAG_ROBODK_MENUEDIT_ACTIVE);
        //    _rdk.SetWindowFlags(WindowFlags.None);
        //    _rdk.SetItemFlags(ItemFlags.None);
        //    if (_robot != null && _robot.Valid()) _robot.SetItemFlags();
        //}

        ////unlock internal roboDK features
        //private void UnlockRoboDK()
        //{
        //    _rdk.SetWindowFlags(WindowFlags.All);
        //    _rdk.SetItemFlags();
        //    _rdk.ShowRoboDK();

        //}

        //get current pos of joints
        private void RetrieveJoints()
        {
            // update the joints
            txtJoints.Text =
                txtJointLinear.Text =
                    txtJointCurveA.Text =
                        txtJointCurveB.Text =
                        "-0.5 , -70 , 111 , 0.0 , 45.0 , 0.0";
            //Values_2_String(_robot.Joints());

            // update the pose
            var pose = _robot.Pose().ToTxyzRxyz();
            txtPosition.Text =
                txtPositionLinear.Text =
                    txtPositionCurveA.Text =
                        txtPositionCurveB.Text =
                            Values_2_String(pose);
        }

        /// <summary>
        ///     Convert a list of numbers provided as a string to an array of value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public double[] String_2_Values(string value)
        {
            double[] output = null;
            try
            {
                output = Array.ConvertAll(value.Split(','), double.Parse);
            }
            catch (FormatException ex)
            {
                ShowError($@"Invalid input: {value}: {ex.Message}");
            }

            return output;
        }

        /// <summary>
        ///     Convert an array of value as a string
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public string Values_2_String(double[] values)
        {
            if (values == null || values.Length < 1) return "Invalid value";

            // Not supported on .NET Framework 2.0:
            //string value = String.Join(" , ", value.Select(p => p.ToString("0.0")).ToArray());
            var output = values[0].ToString("0.0");
            for (var i = 1; i < values.Length; i++) output += $" , {values[i]:0.0}";

            return output;
            //return "";
        }

        private void SampleDialog_Load(object sender, EventArgs e)
        {
            ShowSplashScreen();

            OpenRoboDK();
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

            RetrieveJoints();

            HideSplashScreen();

            _rdk.SetSimulationSpeed(1);

            btnMoveJoint.PerformClick();

        }

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

        private void btnConnectToRobot_Click(object sender, EventArgs e)
        {
            ConfigureKukaRobot();

            ConnectToRobot();
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

        private void btnMoveToHomePosition_Click(object sender, EventArgs e)
        {
            var jointsHome = _robot.JointsHome();

            _robot.MoveJ(jointsHome);
        }

        private void btnReadJoint_Click(object sender, EventArgs e)
        {
            var joints = _robot.Joints();
            // update the joints
            txtJoints.Text = Values_2_String(joints);
        }

        private void btnMoveJoint_Click(object sender, EventArgs e)
        {
            // retrieve the robot joints from the text and validate input
            var joints = String_2_Values(txtJoints.Text);

            // make sure RDK is running and we have a valid input
            if (joints == null) return;

            try
            {
                _robot.MoveJ(joints, MoveBlocking);
            }
            catch (RoboDKException rdkException)
            {
                ShowError("The robot can't move to " + txtJoints.Text + Environment.NewLine + rdkException.Message);
            }
        }

        private void btnReadPosition_Click(object sender, EventArgs e)
        {
            // update the pose
            var position = _robot.Pose().ToTxyzRxyz();
            txtPosition.Text = Values_2_String(position);
        }

        private void btnMovePosition_Click(object sender, EventArgs e)
        {
            // retrieve the robot position from the text and validate input
            var position = String_2_Values(txtPosition.Text);

            // make sure RDK is running and we have a valid input
            if (position == null) return;

            try
            {
                _robot.MoveJ(Matrix.FromTxyzRxyz(position), MoveBlocking);
            }
            catch (RoboDKException roboDKException)
            {
                ShowError("The robot can't move to " + txtPosition.Text + Environment.NewLine + roboDKException.Message);
            }
        }

        private void btnReadJointLinear_Click(object sender, EventArgs e)
        {
            // update the joints
            txtJointLinear.Text = Values_2_String(_robot.Joints());
        }

        private void btnMoveJointLinear_Click(object sender, EventArgs e)
        {
            // retrieve the robot position from the text and validate input
            var joints = String_2_Values(txtJointLinear.Text);

            // make sure RDK is running and we have a valid input
            if (joints == null) return;

            try
            {
                _robot.MoveL(joints, MoveBlocking);
            }
            catch (RoboDKException roboDKException)
            {
                ShowError("The robot can't move to " + txtJointLinear.Text + Environment.NewLine + roboDKException.Message);
            }
        }

        private void btnReadPositionLinear_Click(object sender, EventArgs e)
        {
            // update the pose
            var pose = _robot.Pose().ToTxyzRxyz();
            txtPositionLinear.Text = Values_2_String(pose);
        }

        private void btnMovePositionLinear_Click(object sender, EventArgs e)
        {
            // retrieve the robot position from the text and validate input
            var pose = String_2_Values(txtPositionLinear.Text);

            // make sure RDK is running and we have a valid input
            if (pose == null) return;

            var matrix = Matrix.FromTxyzRxyz(pose);

            try
            {
                _robot.MoveL(matrix, MoveBlocking);
            }
            catch (RoboDKException exception)
            {
                ShowError("The robot can't move to " + txtPositionLinear.Text + Environment.NewLine + exception.Message);
            }
        }

        private void btnReadJointCurve_Click(object sender, EventArgs e)
        {
            // update the joints
            txtJointCurveA.Text =
                txtJointCurveB.Text =
                    Values_2_String(_robot.Joints());
        }

        private void btnMoveJointCurve_Click(object sender, EventArgs e)
        {
            // retrieve the robot position from the text and validate input
            var jointsA = String_2_Values(txtJointCurveA.Text);
            var jointsB = String_2_Values(txtJointCurveB.Text);

            // make sure RDK is running and we have a valid input
            if (jointsA is null || jointsB is null) return;

            try
            {
                _robot.MoveC(jointsA, jointsB, MoveBlocking);
            }
            catch (RoboDKException roboDKException)
            {
                ShowError("The robot can't move to " + txtJointLinear.Text + Environment.NewLine + roboDKException.Message);
            }
        }

        private void btnReadPositionCurve_Click(object sender, EventArgs e)
        {
            // update the pose
            var pose = _robot.Pose().ToTxyzRxyz();
            txtPositionCurveA.Text =
                txtPositionCurveB.Text =
                    Values_2_String(pose);
        }

        private void btnMovePositionCurve_Click(object sender, EventArgs e)
        {
            // retrieve the robot position from the text and validate input
            var poseA = String_2_Values(txtPositionCurveA.Text);
            var poseB = String_2_Values(txtPositionCurveB.Text);

            // make sure RDK is running and we have a valid input
            if (poseA is null || poseB is null) return;

            var matrixA = Matrix.FromTxyzRxyz(poseA);
            var matrixB = Matrix.FromTxyzRxyz(poseB);

            try
            {
                _robot.MoveC(matrixA, matrixB, MoveBlocking);
            }
            catch (RoboDKException exception)
            {
                ShowError("The robot can't move to " + txtPositionLinear.Text + Environment.NewLine + exception.Message);
            }
        }
    }
}