using System.Text;

namespace ConsoleWar
{
	internal class Map
	{
		private const int MAP_HEIGHT = 32;
		private const int MAP_WIDTH = 32;

		private readonly StringBuilder _map = new StringBuilder();

		public char GetMapUnit(double x, double y)
		{
			return _map[(int)y * MAP_WIDTH + (int)x];
		}

		public void SetMapUnit(double x, double y, char c)
		{
			_map[(int)y * MAP_WIDTH + (int)x] = c;
		}

		public void DrawMap(char[] screen, int screenWidth)
		{
			for (int x = 0; x < MAP_WIDTH; x++)
			{
				for (int y = 0; y < MAP_HEIGHT; y++)
				{
					screen[(y + 1) * screenWidth + x] = _map[y * MAP_WIDTH + x];
				}
			}
		}

		public void Initialize()
		{
			_map.Clear();
			_map.Append("################################");
			_map.Append("#............#.................#");
			_map.Append("#............#.................#");
			_map.Append("#............#.................#");
			_map.Append("#............#.............#...#");
			_map.Append("#............#.............#...#");
			_map.Append("#............#.............#...#");
			_map.Append("##.........................#####");
			_map.Append("##.............................#");
			_map.Append("##.............................#");
			_map.Append("#....##..##....................#");
			_map.Append("#..............................#");
			_map.Append("#..............................#");
			_map.Append("#..#...........................#");
			_map.Append("#..............................#");
			_map.Append("#.....................###......#");
			_map.Append("#.....................###......#");
			_map.Append("#..............................#");
			_map.Append("#..............................#");
			_map.Append("#..............................#");
			_map.Append("#..............................#");
			_map.Append("#####..######.......########...#");
			_map.Append("#..............................#");
			_map.Append("#.................#............#");
			_map.Append("#.................#............#");
			_map.Append("#.................#............#");
			_map.Append("#.................#............#");
			_map.Append("#.........##......#............#");
			_map.Append("#..........#......#............#");
			_map.Append("#.................#............#");
			_map.Append("#.................#............#");
			_map.Append("################################");
		}
	}
}
