using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleWar
{
	class Program
	{
		private const int SCREEN_WIDTH = 150;
		private const int SCREEN_HEIGHT = 60;

		private const double DEPTH = 16;
		private const double FOV = Math.PI / 3.5;

		//private static double _playerX = 3.0;
		//private static double _playerY = 3.0;
		//private static double _playerA;
		private static Player _player;
		private static Map _map;

		//private static readonly char[] Screen = new char[SCREEN_WIDTH * SCREEN_HEIGHT];

		static async Task Main()
		{
			Console.SetWindowSize(SCREEN_WIDTH, SCREEN_HEIGHT);
			Console.SetBufferSize(SCREEN_WIDTH, SCREEN_HEIGHT);
			Console.CursorVisible = false;

			_map = new Map();
			_map.Initialize();

			var screen = new char[SCREEN_WIDTH * SCREEN_HEIGHT];

			_player = new Player(_map);

			while (true)
			{
				WorldTime.UpdateTime();
				_map.Initialize();
				var elapsedTime = WorldTime.GetTickTime();

				if (Console.KeyAvailable)
				{
					var command = Console.ReadKey(true).Key;
					_player.ProcessPlayer(command);
					
					if (command == ConsoleKey.Escape)
					{
						break;
					}
				}

				//Ray Casting
				var rayCastingTasks = new List<Task<Dictionary<int, char>>>();
				for (int x = 0; x < SCREEN_WIDTH; x++)
				{
					var x1 = x;
					rayCastingTasks.Add(Task.Run(() => CastRay(x1)));
				}
				foreach (Dictionary<int, char> dictionary in await Task.WhenAll(rayCastingTasks))
				{
					foreach (var key in dictionary.Keys)
					{
						screen[key] = dictionary[key];
					}
				}

				//Stats
				char[] stats = $"X: {_player.X}, Y: {_player.Y}, A: {_player.Angle}, FPS: {(int)(1 / elapsedTime)}"
					.ToCharArray();
				stats.CopyTo(screen, 0);

				//Map
				_map.DrawMap(screen, SCREEN_WIDTH);
				screen[(int)(_player.Y + 1) * SCREEN_WIDTH + (int)_player.X] = 'P';

				Console.SetCursorPosition(0, 0);
				Console.Write(screen, 0, SCREEN_WIDTH * SCREEN_HEIGHT);
			}
		}

		public static Dictionary<int, char> CastRay(int x)
		{
			var result = new Dictionary<int, char>();

			var rayAngle = (_player.Angle + FOV / 2) - x * FOV / SCREEN_WIDTH;

			var distanceToWall = 0d;
			var hitWall = false;
			var isBound = false;
			var wallSize = 1d;

			var rayY = Math.Cos(rayAngle);
			var rayX = Math.Sin(rayAngle);

			while (!hitWall && distanceToWall < DEPTH)
			{
				distanceToWall += 0.1;

				var testX = (int)(_player.X + rayX * distanceToWall);
				var testY = (int)(_player.Y + rayY * distanceToWall);

				if (testX < 0 || testX >= DEPTH + _player.X || testY < 0 || testY >= DEPTH + _player.Y)
				{
					hitWall = true;
					distanceToWall = DEPTH;
				}
				else
				{
					var testCell = _map.GetMapUnit(testX, testY);

					if (testCell == '#')
					{
						hitWall = true;

						var boundsVectorsList = new List<(double X, double Y)>();

						for (var tx = 0; tx < 2; tx++)
						{
							for (var ty = 0; ty < 2; ty++)
							{
								var vx = testX + tx - _player.X;
								var vy = testY + ty - _player.Y;

								var vectorModule = Math.Sqrt(vx * vx + vy * vy);
								var cosAngle = (rayX * vx / vectorModule) + (rayY * vy / vectorModule);
								boundsVectorsList.Add((vectorModule, cosAngle));
							}
						}

						boundsVectorsList = boundsVectorsList.OrderBy(v => v.X).ToList();

						var boundAngle = 0.03 / distanceToWall;

						if (Math.Acos(boundsVectorsList[0].Y) < boundAngle ||
							Math.Acos(boundsVectorsList[1].Y) < boundAngle)
							isBound = true;
					}
					else
					{
						_map.SetMapUnit(testX, testY, '*');
					}
				}
			}

			var ceiling = (int)(SCREEN_HEIGHT / 2.0 - SCREEN_HEIGHT * FOV / distanceToWall);
			var floor = SCREEN_HEIGHT - ceiling;

			ceiling += (int)(SCREEN_HEIGHT - SCREEN_HEIGHT * wallSize);

			char wallShade;

			if (isBound)
				wallShade = '|';
			else if (distanceToWall <= DEPTH / 4.0)
				wallShade = '\u2588';
			else if (distanceToWall < DEPTH / 3.0)
				wallShade = '\u2593';
			else if (distanceToWall < DEPTH / 2.0)
				wallShade = '\u2592';
			else if (distanceToWall < DEPTH)
				wallShade = '\u2591';
			else
				wallShade = ' ';

			for (var y = 0; y < SCREEN_HEIGHT; y++)
			{
				if (y < ceiling)
					result[y * SCREEN_WIDTH + x] = ' ';
				else if (y > ceiling && y <= floor)
					result[y * SCREEN_WIDTH + x] = wallShade;
				else
				{
					char floorShade;
					var b = 1.0 - (y - SCREEN_HEIGHT / 2.0) / (SCREEN_HEIGHT / 2.0);

					if (b < 0.25)
						floorShade = '#';
					else if (b < 0.5)
						floorShade = 'x';
					else if (b < 0.75)
						floorShade = '-';
					else if (b < 0.9)
						floorShade = '.';
					else
						floorShade = ' ';

					result[y * SCREEN_WIDTH + x] = floorShade;
				}
			}

			return result;
		}
	}
}
