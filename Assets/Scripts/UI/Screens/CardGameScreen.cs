using UnityEngine;


public class CardGameScreen : PopupBase
{
    [SerializeField] private CardGameTimer gameTimer;
    [SerializeField] private CardGameScore scoreLabel;
    [SerializeField] private CardController cardController;
    [SerializeField] private StateObject gameState;
    [Space]
    [SerializeField] private ButtonBase pauseButton;
    [SerializeField] private ButtonBase startButton;
    [SerializeField] private ButtonBase restartButton1;
    [SerializeField] private ButtonBase restartButton2;
    [SerializeField] private ButtonBase menuButton1;
    [SerializeField] private ButtonBase menuButton2;
    [SerializeField] private ButtonBase menuButton3;


    protected override void Awake()
    {
        base.Awake();
        
        gameTimer.SubscribeTimer(cardController);
        scoreLabel.SubscribeLabel(cardController);
        
        cardController.OnGameFinish += CardController_OnGameFinish;
    }


    public void SetPause(bool isPause)
    {
        cardController.SetPause(isPause);
    }


    public void StartGame()
    {
        gameState.SetState(1);
        cardController.StartGame();
    }


    protected override void SubscribeButtons()
    {
        base.SubscribeButtons();
        
        pauseButton.OnClick.AddListener(PauseButton_OnClick);
        startButton.OnClick.AddListener(StartButton_OnClick);
        restartButton1.OnClick.AddListener(StartButton_OnClick);
        restartButton2.OnClick.AddListener(StartButton_OnClick);
        menuButton1.OnClick.AddListener(MenuButton_OnClick);
        menuButton2.OnClick.AddListener(MenuButton_OnClick);
        menuButton3.OnClick.AddListener(MenuButton_OnClick);
    }


    protected override void UnSubscribeButtons()
    {
        base.UnSubscribeButtons();
        
        pauseButton.OnClick.RemoveListener(PauseButton_OnClick);
        startButton.OnClick.RemoveListener(StartButton_OnClick);
        restartButton1.OnClick.RemoveListener(StartButton_OnClick);
        restartButton2.OnClick.RemoveListener(StartButton_OnClick);
        menuButton1.OnClick.RemoveListener(MenuButton_OnClick);
        menuButton2.OnClick.RemoveListener(MenuButton_OnClick);
        menuButton3.OnClick.RemoveListener(MenuButton_OnClick);
    }


    protected override void AfterShow()
    {
        base.AfterShow();

        gameState.SetState(0);
    }


    private void CardController_OnGameFinish(bool isWin)
    {
        var stateIndex = isWin ? 3 : 2;
        var clipType = isWin ? AudioClipType.Win : AudioClipType.Lose;

        gameState.SetState(stateIndex);
        
        AudioManager.Instance.PlaySound(clipType);

        if (isWin)
        {
            CurrencyManager.Instance.AddCurrency(CurrencyType.Coins, 1000f);
        }
    }


    private void PauseButton_OnClick()
    {
        SetPause(true);

        UIManager.Instance.ShowPopup(PopupType.CardPause);
    }


    private void StartButton_OnClick()
    {
        StartGame();
    }


    private void MenuButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Menu);
        
        Hide();
    }
}
