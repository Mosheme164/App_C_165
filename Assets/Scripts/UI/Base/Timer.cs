using System;
using UnityEngine;


public class Timer : CounterBase
{
    protected override string GetFormattedString()
    {
        _currentValue = Mathf.Clamp(_currentValue, 0, Int32.MaxValue);

        var time = new TimeSpan(_currentValue * TimeSpan.TicksPerSecond);

        return time.ToString("c");
    }
}
