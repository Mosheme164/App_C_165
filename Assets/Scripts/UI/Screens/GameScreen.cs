using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameScreen : PopupBase
{
    [Space]
    [SerializeField] private ButtonBase pauseButton;
    [SerializeField] private Text timer;
    [Space]
    [SerializeField] private ButtonBase tutorialButton;
    [SerializeField] private List<GameObject> stateObjects;


    public void SetTutorial(bool isLonger)
    {
        var timerValue = isLonger
            ? 120
            : 90;

        UpdateTimer(timerValue);

        stateObjects[0].SetActive(!isLonger);
        stateObjects[1].SetActive(isLonger);

        tutorialButton.gameObject.SetActive(true);
    }


    public void UpdateTimer(int value)
    {
        timer.text = $"{value} sec";
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


    private void PauseButton_OnClick()
    {
        UIManager.Instance.ShowPopup(PopupType.Pause);
    }


    private void TutorialButton_OnClick()
    {
        tutorialButton.gameObject.SetActive(false);
        
    }
}
