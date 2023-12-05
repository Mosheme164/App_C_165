using UnityEngine;


public class MenuScreen : PopupBase
{
    [Space]
    [SerializeField] private ButtonBase settingsButton;
    [SerializeField] private SwitchButton bonusButton;
    [SerializeField] private ButtonBase shopButton;
    [SerializeField] private ButtonBase playButton;
    

    protected override void Awake()
    {
        base.Awake();
    }


    public void UpdateBonus()
    {
        bonusButton.SetState(BonusManager.Instance.IsClaimable);
        bonusButton.SetInteractable(BonusManager.Instance.IsClaimable);
    }


    protected override void BeforeShow()
    {
        base.BeforeShow();
        
        UpdateBonus();
    }


    protected override void SubscribeButtons()
    {
        base.SubscribeButtons();
        
        settingsButton.OnClick.AddListener(SettingsButton_OnClick);
        bonusButton.OnClick.AddListener(BonusButton_OnClick);
        shopButton.OnClick.AddListener(ShopButton_OnClick);
        playButton.OnClick.AddListener(PlayButton_OnClick);
    }


    protected override void UnSubscribeButtons()
    {
        base.UnSubscribeButtons();
        
        settingsButton.OnClick.RemoveListener(SettingsButton_OnClick);
        bonusButton.OnClick.RemoveListener(BonusButton_OnClick);
        shopButton.OnClick.RemoveListener(ShopButton_OnClick);
        playButton.OnClick.RemoveListener(PlayButton_OnClick);
    }


    private void SettingsButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Settings);
    }
    
    
    private void BonusButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Gift);
    }
    
    
    private void ShopButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Shop);
    }
    
    
    private void PlayButton_OnClick()
    {
        LevelManager.Instance.StartGame();
        UIManager.Instance.ShowPopup(PopupType.GameScore);
        
        Hide();
    }
}