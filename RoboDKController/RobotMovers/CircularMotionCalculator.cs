using System;

namespace SamplePanelRoboDK.RobotMovers
{
	internal class CircularMotionCalculator
	{
		// A simple 3D point structure
		public struct Point3D
		{
			public double X { get; }
			public double Y { get; }
			public double Z { get; }

			public Point3D(double x, double y, double z)
			{
				X = x;
				Y = y;
				Z = z;
			}

			public static Point3D operator +(Point3D a, Point3D b)
			{
				return new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
			}

			public static Point3D operator -(Point3D a, Point3D b)
			{
				return new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
			}

			public static Point3D operator *(Point3D a, double scalar)
			{
				return new Point3D(a.X * scalar, a.Y * scalar, a.Z * scalar);
			}

			public static Point3D operator /(Point3D a, double scalar)
			{
				return new Point3D(a.X / scalar, a.Y / scalar, a.Z / scalar);
			}
		}

		public enum MotionDirection
		{
			Clockwise,
			CounterClockwise
		}

		/// <summary>
		/// Calculates secondary point on arc from starting point ,target point and radius.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="target"></param>
		/// <param name="radius"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Point3D CalculateSecondaryPoint(double[] start, double[] target, double radius,
			MotionDirection direction)
		{
			var startPoint = new Point3D(start[0], start[1], start[2]);
			var targetPoint = new Point3D(target[0], target[1], target[2]);
			return CalculateSecondaryPoint(startPoint, targetPoint, radius, direction);
		}

		/// <summary>
		/// Calculates secondary point on arc from starting point ,target point and radius.
		/// </summary>
		/// <param name="start"></param>
		/// <param name="target"></param>
		/// <param name="radius"></param>
		/// <param name="direction"></param>
		/// <returns></returns>
		public static Point3D CalculateSecondaryPoint(Point3D start, Point3D target, double radius,
			MotionDirection direction)
		{
			// Step 1: Calculate the midpoint
			Point3D midpoint = (start + target) / 2;

			// Step 2: Calculate the vector from start to target
			Point3D directionVector = target - start;

			// Step 3: Find the perpendicular vector (normal to the plane, assuming XY movement)
			// If moving clockwise, use one direction; if counterclockwise, use the opposite direction.
			Point3D normal =
				direction == MotionDirection.Clockwise ?
					new Point3D(-directionVector.Y, directionVector.X, 0) :
					new Point3D(directionVector.Y, -directionVector.X, 0);

			// Step 4: Normalize the normal vector
			double length = Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);
			normal = normal / length;

			// Step 5: Calculate the distance from the midpoint to the secondary point (P2)
			double halfDistance = GetSecondaryToMidpointDistance(radius, directionVector);

			// Step 6: Calculate secondary point by offsetting the midpoint along the normal vector
			Point3D secondaryPoint = midpoint + (normal * halfDistance);

			return secondaryPoint;
		}

		/// <summary>
		/// Calculate the distance from the midpoint to the secondary point (P2)
		/// </summary>
		/// <param name="radius"></param>
		/// <param name="directionVector"></param>
		/// <returns></returns>
		private static double GetSecondaryToMidpointDistance(double radius, Point3D directionVector)
		{
			return radius - Math.Sqrt(Math.Pow(radius, 2) -
			                          Math.Pow(
				                          Math.Sqrt(directionVector.X * directionVector.X +
				                                    directionVector.Y * directionVector.Y +
				                                    directionVector.Z * directionVector.Z) / 2, 2));
		}
	}
}