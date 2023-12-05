using UnityEngine;

public class TimerCounter : CounterBase
{
    private string _time;
    
    
    private void Update()
    {
        UpdateTimer();
    }


    private void UpdateTimer()
    {
        _time = BonusManager.Instance.TimeRemaining;
        
        UpdateValue(Time.deltaTime);
    }


    protected override string GetFormattedString()
    {
        return _time;
    }
}
