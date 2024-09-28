using System.Collections.Generic;
using RoboDk.API;

namespace SamplePanelRoboDK.RobotMovers
{
	internal abstract class BaseMover : IMover
	{
		protected readonly IItem _robot;

		protected BaseMover(IItem robot)
		{
			_robot = robot;
		}

		public abstract void Move(Dictionary<char, double> targetPosition);
	}
}