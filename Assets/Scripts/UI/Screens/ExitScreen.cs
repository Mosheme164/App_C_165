using UnityEngine;


public class ExitScreen : PopupBase
{
    [Space]
    [SerializeField] private ButtonBase yesButton;
    [SerializeField] private ButtonBase noButton;
    
    
    protected override void SubscribeButtons()
    {
        base.SubscribeButtons();

        yesButton.OnClick.AddListener(YesButton_OnClick);
        noButton.OnClick.AddListener(NoButton_OnClick);
    }


    protected override void UnSubscribeButtons()
    {
        base.UnSubscribeButtons();
        
        yesButton.OnClick.RemoveListener(YesButton_OnClick);
        noButton.OnClick.RemoveListener(NoButton_OnClick);
    }


    private void YesButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Menu);
        UIManager.Instance.HidePopup(PopupType.GameScore);
        LevelManager.Instance.ClearGame();
        
        Hide();
    }
    
    
    private void NoButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Pause);
        
        Hide();
    }
}
