using UnityEngine;


public class MenuScreen : PopupBase
{
    [Space]
    [SerializeField] private ButtonBase settingsButton;
    [SerializeField] private SwitchButton bonusButton;
    [SerializeField] private ButtonBase shopButton;
    [SerializeField] private ButtonBase shopButton2;
    [SerializeField] private ButtonBase playButton;
    [SerializeField] private ButtonBase cardGameButton;


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
        shopButton2.OnClick.AddListener(ShopButton2_OnClick);
        playButton.OnClick.AddListener(PlayButton_OnClick);
        cardGameButton.OnClick.AddListener(CardGameButton_OnClick);
    }


    protected override void UnSubscribeButtons()
    {
        base.UnSubscribeButtons();
        
        settingsButton.OnClick.RemoveListener(SettingsButton_OnClick);
        bonusButton.OnClick.RemoveListener(BonusButton_OnClick);
        shopButton.OnClick.RemoveListener(ShopButton_OnClick);
        shopButton2.OnClick.RemoveListener(ShopButton2_OnClick);
        playButton.OnClick.RemoveListener(PlayButton_OnClick);
        cardGameButton.OnClick.RemoveListener(CardGameButton_OnClick);
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
    
    
    private void ShopButton2_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Achievements);
    }
    
    
    private void PlayButton_OnClick()
    {
        LevelManager.Instance.StartGame();
        UIManager.Instance.ShowPopup(PopupType.GameScore);
        
        Hide();
    }


    private void CardGameButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.GameLevel);
        
        Hide();
    }
}