using RoboDk.API;

namespace SamplePanelRoboDK.RobotMovers
{
	internal class CounterclockwiseCircularMover : BaseCircularMover
	{
		public CounterclockwiseCircularMover(IItem robot) : base(robot)
		{
		}

		protected override CircularMotionCalculator.MotionDirection Direction =>
			CircularMotionCalculator.MotionDirection.CounterClockwise;
	}
}