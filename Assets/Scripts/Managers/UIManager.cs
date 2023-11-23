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
    private bool _hasBeenLaunched;
    private bool _isLevelMode;
    

    public GameScreen GameScreen => _gameScreen;


    protected override void Awake()
    {
        base.Awake();
        
        canvas.worldCamera = Camera.main;

        _gameScreen = screens.FirstOrDefault(popup => popup.PopupType == PopupType.GameLevel) as GameScreen;
    }


    private void Start()
    {
        //ShowPopup(PopupType.Menu);
        //ShowPopup(PopupType.GameLevel);
    }


    public void ShowPopup(PopupType type)
    {
        var screen = screens.FirstOrDefault(popup => popup.PopupType == type);

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
