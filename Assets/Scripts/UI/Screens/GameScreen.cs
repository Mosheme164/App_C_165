using UnityEngine;


public class GameScreen : PopupBase
{
    [Space]
    [SerializeField] private ButtonBase pauseButton;
    [SerializeField] private SelfDestruct scoreBubblePrefab;
    [Space]
    [SerializeField] private ButtonBase tutorialButton;


    public void CreateBubble()
    {
        var newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var newBubble = Instantiate(scoreBubblePrefab, transform);

        newPosition = new Vector3(newPosition.x, newPosition.y, 10f);
        newBubble.transform.position = newPosition;
    }


    public void SetTutorial(bool isLonger)
    {
        var timerValue = isLonger
            ? 120
            : 90;
        
        tutorialButton.gameObject.SetActive(true);
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
        LevelManager.Instance.SetPause(true);
    }


    private void TutorialButton_OnClick()
    {
        tutorialButton.gameObject.SetActive(false);
        
    }
}
