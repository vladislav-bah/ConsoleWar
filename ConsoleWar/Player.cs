using System;

namespace ConsoleWar
{
	internal class Player
	{
		private int _leftRigthtCoef = 10;
		private int _upDownCoef = 15;
		private readonly Map _map;

		public Player(Map map)
		{
			_map = map;
		}

		public double X { get; private set; } = 3;

		public double Y { get; private set; } = 3;

		public double Angle { get; private set; }

		public void ProcessPlayer(ConsoleKey command)
		{
			var tickTime = WorldTime.GetTickTime();
			switch (command)
			{
				case ConsoleKey.A:
					Angle += tickTime * _leftRigthtCoef;
					break;
				case ConsoleKey.D:
					Angle -= tickTime * _leftRigthtCoef;
					break;
				case ConsoleKey.W:
					X += Math.Sin(Angle) * _upDownCoef * tickTime;
					Y += Math.Cos(Angle) * _upDownCoef * tickTime;

					if (_map.GetMapUnit(X, Y) == '#')
					{
						X -= Math.Sin(Angle) * _upDownCoef * tickTime;
						Y -= Math.Cos(Angle) * _upDownCoef * tickTime;
					}

					break;

				case ConsoleKey.S:
					X -= Math.Sin(Angle) * _upDownCoef * tickTime;
					Y -= Math.Cos(Angle) * _upDownCoef * tickTime;

					if (_map.GetMapUnit(X, Y) == '#')
					{
						X += Math.Sin(Angle) * _upDownCoef * tickTime;
						Y += Math.Cos(Angle) * _upDownCoef * tickTime;
					}

					break;
			}
		}
	}
}
