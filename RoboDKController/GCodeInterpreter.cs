using System;
using System.Collections.Generic;
using RoboDk.API;
using SamplePanelRoboDK.RobotMovers;

namespace SamplePanelRoboDK
{
	class GCodeInterpreter
	{
		private readonly IItem _robot;
		private readonly Dictionary<char, double> _position = new Dictionary<char, double>
			{ { 'X', 0.0 }, { 'Y', 0.0 }, { 'Z', 0.0 }, { 'R', 0.0 } };

		private double _feedRate = 100.0; // Default feed rate
		private bool _isSpindleOn = false;

		public GCodeInterpreter(IItem robot)
		{
			_robot = robot;
		}

		/// <summary>
		/// Interprets command line and runs it on robot/simulator
		/// </summary>
		/// <param name="line"></param>
		public void InterpretLine(string line)
		{

			// Strip comments (everything after ';') and whitespace
			line = line.Split(';')[0].Trim();
			if (string.IsNullOrEmpty(line))
				return;

			// Split the line into commands (e.g., G1 X10 Y20)
			string[] commands = line.Split(' ');
			IMover mover = null;
			foreach (var cmd in commands)
			{
				ProcessCommand(cmd, out var outMover);
				if (outMover != null)
					mover = outMover;
			}

			if (!(mover is null))
			{
				mover.Move(_position);
			}
		}

		private void ProcessCommand(string cmd, out IMover mover)
		{
			mover = null;
			char code = cmd[0]; // First character (G, M, X, Y, Z, F)
			string value = cmd.Substring(1); // The value after the character

			switch (code)
			{
				case 'G':
					ProcessGCommand(int.Parse(value), out mover);
					break;
				case 'M':
					ProcessMCommand(int.Parse(value));
					break;
				case 'X':
				case 'Y':
				case 'Z':
				case 'R':
					SetPosition(code, double.Parse(value));
					break;
				case 'F':
					SetFeedRate(double.Parse(value));
					break;
				default:
					//TODO By Arman: Add Exception
					//Console.WriteLine($"Unknown command: {cmd}");
					break;
			}
		}

		private void ProcessGCommand(int value, out IMover mover)
		{
			mover = null;
			switch (value)
			{
				case 0:
					mover = new RapidMover(_robot);
					break;
				case 1:
					mover = new LinearMover(_robot);
					break;
				case 2:
					mover = new ClockwiseCircularMover(_robot);
					break;
				case 3:
					mover = new CounterclockwiseCircularMover(_robot);
					break;
				case 28:
					mover = new RapidMover(_robot);
					SetHomePosition();
					break;
				default:
					//TODO By Arman: Add Exception
					//Console.WriteLine($"Unsupported G-code: G{value}");
					break;
			}
		}

		private void ProcessMCommand(int value)
		{
			switch (value)
			{
				case 3:
					SpindleOn();
					break;
				case 5:
					SpindleOff();
					break;
				default:
					//TODO By Arman: Add Exception
					//Console.WriteLine($"Unsupported M-code: M{value}");
					break;
			}
		}

		private void SetPosition(char axis, double value)
		{
			_position[axis] = value;
		}

		private void SetFeedRate(double value)
		{
			Console.WriteLine($"Setting feed rate to {value}");
			_feedRate = value;
		}

		private void SpindleOn()
		{
			_isSpindleOn = true;
			Console.WriteLine("Spindle turned ON (M3)");
		}

		private void SpindleOff()
		{
			_isSpindleOn = false;
			Console.WriteLine("Spindle turned OFF (M5)");
		}

		private void SetHomePosition()
		{
			Console.WriteLine("Homing all axes (G28)");
			_position['X'] = 0.0;
			_position['Y'] = 0.0;
			_position['Z'] = 0.0;
		}
	}
}