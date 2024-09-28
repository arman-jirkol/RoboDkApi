using System.Collections.Generic;

namespace SamplePanelRoboDK.RobotMovers
{
	internal interface IMover
	{
		void Move(Dictionary<char, double> targetPosition);
	}
}