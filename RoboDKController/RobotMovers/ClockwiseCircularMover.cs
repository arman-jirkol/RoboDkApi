using RoboDk.API;

namespace SamplePanelRoboDK.RobotMovers
{
	internal class ClockwiseCircularMover : BaseCircularMover
	{
		public ClockwiseCircularMover(IItem robot) : base(robot)
		{
		}

		protected override CircularMotionCalculator.MotionDirection Direction =>
			CircularMotionCalculator.MotionDirection.Clockwise;
	}
}