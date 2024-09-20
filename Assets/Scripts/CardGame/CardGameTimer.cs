using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


public class CardGameTimer : CounterBase
{
    [SerializeField] private Image backImage;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite alertSprite;
    
    
    public void SubscribeTimer(CardController cardController)
    {
        cardController.TimeLeft.Subscribe(value =>
        {
            UpdateValue(value);

            backImage.sprite = value > 10f ? normalSprite : alertSprite;
            
        }).AddTo(this);
    }
    
    
    protected override string GetFormattedString()
    {
        return TimeSpan.FromSeconds(_currentValue).ToString(@"mm\:ss");
    }
}
