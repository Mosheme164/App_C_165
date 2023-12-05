using UniRx;


public class CoinsCounter : CounterBase
{
    private void Awake()
    {
        LevelManager.Instance.CoinsCollected.Subscribe(value =>
        {
            UpdateValue(value);
        }).AddTo(this);
    }


    protected override string GetFormattedString()
    {
        return base.GetFormattedString();
    }
}
