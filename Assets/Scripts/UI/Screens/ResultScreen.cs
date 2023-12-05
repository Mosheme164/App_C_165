using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


public class ResultScreen : PopupBase
{
    [Space]
    [SerializeField] private ButtonBase continueButton;
    [SerializeField] private ButtonBase restartButton;
    [SerializeField] private ButtonBase menuButton;
    [SerializeField] private Text coinsAmount;


    private int _coinsCollected;


    protected override void SubscribeButtons()
    {
        base.SubscribeButtons();

        continueButton.OnClick.AddListener(ContinueButton_OnClick);
        restartButton.OnClick.AddListener(RestartButton_OnClick);
        menuButton.OnClick.AddListener(MenuButton_OnClick);
    }


    protected override void UnSubscribeButtons()
    {
        base.UnSubscribeButtons();
        
        continueButton.OnClick.RemoveListener(ContinueButton_OnClick);
        restartButton.OnClick.RemoveListener(RestartButton_OnClick);
        menuButton.OnClick.RemoveListener(MenuButton_OnClick);
    }


    protected override void BeforeShow()
    {
        base.BeforeShow();

        _coinsCollected = LevelManager.Instance.CoinsCollected.Value;
        coinsAmount.text = _coinsCollected.ToString();

        var currencyAmount = CurrencyManager.Instance.GetCurrencyAmount(CurrencyType.Coins);

        continueButton.SetInteractable(currencyAmount + _coinsCollected >= 100f);
    }


    private void CollectCoins()
    {
        CurrencyManager.Instance.AddCurrency(CurrencyType.Coins, _coinsCollected);
    }
    
    
    private void ContinueButton_OnClick()
    {
        CollectCoins();
        
        if (CurrencyManager.Instance.TryRemoveCurrency(CurrencyType.Coins, 100f))
        {
            Observable.Timer(TimeSpan.FromSeconds(.38f)).Subscribe(_ =>
            {
                LevelManager.Instance.ContinueGame();
            }).AddTo(this);
            
            Hide();
        }
        else
        {
            Debug.LogError("LOGIC ERROR");
        }
    }
    
    
    private void RestartButton_OnClick()
    {
        CollectCoins();
        
        Observable.Timer(TimeSpan.FromSeconds(.38f)).Subscribe(_ =>
        {
            LevelManager.Instance.RestartGame();
        }).AddTo(this);

        Hide();
    }
    
    
    private void MenuButton_OnClick()
    {
        CollectCoins();
        
        Observable.Timer(TimeSpan.FromSeconds(.38f)).Subscribe(_ =>
        {
            LevelManager.Instance.ClearGame();
            UIManager.Instance.HidePopup(PopupType.GameScore);
            UIManager.Instance.ShowPopup(PopupType.Menu);

        }).AddTo(this);
        
        Hide();
    }
}