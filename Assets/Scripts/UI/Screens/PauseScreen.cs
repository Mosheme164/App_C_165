using UnityEngine;


public class PauseScreen : PopupBase
{
    [Space] 
    [SerializeField] private bool isCardGame;
    [SerializeField] private ButtonBase continueButton;
    [SerializeField] private ButtonBase restartButton;
    [SerializeField] private ButtonBase menuButton;
    

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


    private void ContinueButton_OnClick()
    {
        if (isCardGame)
        {
            UIManager.Instance.CardGameScreen.SetPause(false);
        }
        else
        {
            LevelManager.Instance.ContinueGame();
        }

        Hide();
    }
    
    
    private void RestartButton_OnClick()
    {
        UIManager.Instance.ShowPopup(isCardGame
            ? PopupType.CardRestart
            : PopupType.Restart);

        Hide();
    }
    
    
    private void MenuButton_OnClick()
    {
        UIManager.Instance.ShowPopup(isCardGame 
            ? PopupType.CardExit 
            : PopupType.Exit);

        Hide();
    }
}