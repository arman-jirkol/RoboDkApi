using System.Collections.Generic;
using RoboDk.API;

namespace SamplePanelRoboDK.RobotMovers
{
	internal abstract class BaseCircularMover : BaseMover
	{
		protected abstract CircularMotionCalculator.MotionDirection Direction { get; }

		protected BaseCircularMover(IItem robot) : base(robot)
		{
		}

		public override void Move(Dictionary<char, double> targetPosition)
		{
			var current = _robot.Pose().ToTxyzRxyz();

			var position = _robot.Pose().ToTxyzRxyz();
			position[0] = targetPosition['X'];
			position[1] = targetPosition['Y'];
			position[2] = targetPosition['Z'];

			var midPosition = _robot.Pose().ToTxyzRxyz();
			var midPoint =
				CircularMotionCalculator.CalculateSecondaryPoint(
					current, position, targetPosition['R'], Direction);
			midPosition[0] = midPoint.X;
			midPosition[1] = midPoint.Y;
			midPosition[2] = midPoint.Z;

			_robot.MoveC(Matrix.FromTxyzRxyz(midPosition), Matrix.FromTxyzRxyz(position), false);
		}
	}
}