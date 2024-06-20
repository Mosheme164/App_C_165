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

        var canContinue = LevelManager.Instance.CanContinue();
        continueButton.SetInteractable(canContinue);
    }


    private void ContinueButton_OnClick()
    {
        if (LevelManager.Instance.TryContinue())
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
        LevelManager.Instance.CollectCoins();
        
        Observable.Timer(TimeSpan.FromSeconds(.38f)).Subscribe(_ =>
        {
            LevelManager.Instance.RestartGame();
        }).AddTo(this);

        Hide();
    }
    
    
    private void MenuButton_OnClick()
    {
        LevelManager.Instance.CollectCoins();
        
        Observable.Timer(TimeSpan.FromSeconds(.38f)).Subscribe(_ =>
        {
            LevelManager.Instance.ClearGame();
            UIManager.Instance.HidePopup(PopupType.GameScore);
            UIManager.Instance.ShowPopup(PopupType.Menu);

        }).AddTo(this);
        
        Hide();
    }
}