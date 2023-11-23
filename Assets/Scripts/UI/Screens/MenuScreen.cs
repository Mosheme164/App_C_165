using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class MenuScreen : PopupBase
{
    [Space]
    [SerializeField] private ButtonBase settingsButton;
    [SerializeField] private ButtonBase matchBoardButton;
    [SerializeField] private ButtonBase shopButton;
    [SerializeField] private Transform matchItemsRoot;
    

    protected override void Awake()
    {
        base.Awake();

    }


    protected override void SubscribeButtons()
    {
        base.SubscribeButtons();
        
        settingsButton.OnClick.AddListener(SettingsButton_OnClick);
        matchBoardButton.OnClick.AddListener(MatchBoardButton_OnClick);
        shopButton.OnClick.AddListener(ShopButton_OnClick);
    }


    protected override void UnSubscribeButtons()
    {
        base.UnSubscribeButtons();
        
        settingsButton.OnClick.RemoveListener(SettingsButton_OnClick);
        matchBoardButton.OnClick.RemoveListener(MatchBoardButton_OnClick);
        shopButton.OnClick.RemoveListener(ShopButton_OnClick);
    }


    private void SettingsButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Settings);
    }
    
    
    private void MatchBoardButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Achievements);
    }
    
    
    private void ShopButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Shop);
    }
}