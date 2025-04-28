using Godot;

namespace ULTRAmiami.Utils;

public static class TimerExtensions
{
	public static float GetTimePassed(this Timer timer)
		=> (float)(timer.GetWaitTime() - timer.GetTimeLeft());
	
	public static float GetTimePercentage(this Timer timer)
	{
		if (timer.IsStopped())
			return 0;
		
		return (float)(timer.GetTimePassed() / timer.GetWaitTime());
	}
}
