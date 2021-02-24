using System;

namespace ConsoleWar
{
	internal static class WorldTime
	{
		private static DateTime _timeFrom;
		private static DateTime _timeTo;
		private static double _currentTickTime;

		static WorldTime()
		{
			_timeFrom = DateTime.Now;
		}

		public static void UpdateTime()
		{
			_timeTo = DateTime.Now;
			_currentTickTime = (_timeTo - _timeFrom).TotalSeconds;
			_timeFrom = _timeTo;
		}

		public static double GetTickTime()
		{
			return _currentTickTime;
		}
	}
}
