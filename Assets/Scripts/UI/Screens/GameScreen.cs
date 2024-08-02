using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameScreen : PopupBase
{
    private const string TutorialShownKey = "TutorialShown";
    
    [Space]
    [SerializeField] private ButtonBase pauseButton;
    [SerializeField] private SelfDestruct scoreBubblePrefab;
    [Space]
    [SerializeField] private ButtonBase tutorialButton;
    [SerializeField] private Image background;
    [SerializeField] private List<Sprite> skins;

    private Sprite _currentSkin;
    private bool _isTutorialShown;


    protected override void Awake()
    {
        base.Awake();
        
        LoadData();
    }


    public void SetPauseButton(bool isActive)
    {
        pauseButton.SetInteractable(isActive);
    }
    

    public void CreateBubble()
    {
        var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var newBubble = Instantiate(scoreBubblePrefab, transform);

        newPosition = new Vector3(newPosition.x, newPosition.y, 10f);
        newBubble.transform.position = newPosition;
    }


    public void SetTutorial()
    {
        if (_isTutorialShown) return;
        
        tutorialButton.gameObject.SetActive(true);

        LevelManager.Instance.SetPause(true);
        LevelManager.Instance.SetPriority(true);
    }


    protected override void SubscribeButtons()
    {
        base.SubscribeButtons();
        
        pauseButton.OnClick.AddListener(PauseButton_OnClick);
        tutorialButton.OnClick.AddListener(TutorialButton_OnClick);
    }


    protected override void UnSubscribeButtons()
    {
        base.UnSubscribeButtons();
        
        pauseButton.OnClick.RemoveListener(PauseButton_OnClick);
        tutorialButton.OnClick.RemoveListener(TutorialButton_OnClick);
    }


    protected override void BeforeShow()
    {
        base.BeforeShow();

        GetSkin();
        
        background.sprite = _currentSkin;
    }


    private void PauseButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Pause);
        LevelManager.Instance.SetPause(true);
    }


    private void TutorialButton_OnClick()
    {
        tutorialButton.gameObject.SetActive(false);
        
        LevelManager.Instance.SetPause(false);
        LevelManager.Instance.SetPriority(false);
        LevelManager.Instance.PopFirst();

        _isTutorialShown = true;
        
        SaveData();
    }
    
    
    private void GetSkin()
    {
        var currentSkinIndex = SkinManager.Instance.CurrentIndex2;

        _currentSkin = skins[currentSkinIndex];
    }


    private void SaveData()
    {
        PlayerPrefs.SetInt(TutorialShownKey, _isTutorialShown ? 1 : 0);
        PlayerPrefs.Save();
    }


    private void LoadData()
    {
        _isTutorialShown = PlayerPrefs.GetInt(TutorialShownKey) == 1;
    }
}
