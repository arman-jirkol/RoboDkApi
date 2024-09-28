using System.Collections.Generic;
using RoboDk.API;

namespace SamplePanelRoboDK.RobotMovers
{
	internal class RapidMover : BaseMover
	{
		public RapidMover(IItem robot) : base(robot)
		{
		}

		public override void Move(Dictionary<char, double> targetPosition)
		{
			var position = _robot.Pose().ToTxyzRxyz();
			position[0] = targetPosition['X'];
			position[1] = targetPosition['Y'];
			position[2] = targetPosition['Z'];

			_robot.MoveJ(Matrix.FromTxyzRxyz(position));
		}
	}
}