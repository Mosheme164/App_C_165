using System.Collections.Generic;
using System.Linq;
using Base;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private CanvasScaler canvasScaler;
    [SerializeField] private List<PopupBase> screens;

    private GameScreen _gameScreen;
    private CardGameScreen _cardGameScreen;
    private bool _hasBeenLaunched;
    private bool _isLevelMode;
    

    public GameScreen GameScreen => _gameScreen;
    
    
    public CardGameScreen CardGameScreen => _cardGameScreen;


    protected override void Awake()
    {
        base.Awake();
        
        canvas.worldCamera = Camera.main;

        _gameScreen = screens.FirstOrDefault(popup => popup.PopupType == PopupType.GameScore) as GameScreen;
        _cardGameScreen = screens.FirstOrDefault(popup => popup.PopupType == PopupType.GameLevel) as CardGameScreen;
    }


    private void Start()
    {
        if (BonusManager.Instance.IsWheelClaimable)
        {
            ShowPopup(PopupType.Reward);
        }
        else
        {
            ShowPopup(PopupType.Menu);
        }
    }


    public void UpdateBonus()
    {
        var screen = screens.FirstOrDefault(popup => popup.PopupType == PopupType.Menu) as MenuScreen;

        if (screen != null)
        {
            screen.UpdateBonus();
        }
    }


    public void ShowPopup(PopupType type)
    {
        var screen = screens.FirstOrDefault(popup => popup.PopupType == type);

        if (screen != null)
        {
            screen.Show();
        }
    }


    public void ShowResult()
    {
        var screen = screens.FirstOrDefault(popup => popup.PopupType == PopupType.ResultScore);

        if (screen != null)
        {
            screen.Show();
        }
    }
    
    
    public void HidePopup(PopupType type)
    {
        var screen = screens.FirstOrDefault(popup => popup.PopupType == type);

        if (screen != null && 
            screen.IsOpen.Value)
        {
            screen.Hide();
        }
    }
}
