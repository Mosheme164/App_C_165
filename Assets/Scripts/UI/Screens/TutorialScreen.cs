using System.Collections.Generic;
using UnityEngine;


public class TutorialScreen : PopupBase
{
    [SerializeField] private ButtonBase mainButton;
    [SerializeField] private List<GameObject> stateObjects;

    private int _currentState;


    protected override void Awake()
    {
        base.Awake();

        mainButton.SetInteractable(false);
        _currentState = 0;
        
        UpdateStates();
    }


    protected override void AfterShow()
    {
        base.AfterShow();

        mainButton.SetInteractable(true);
    }


    protected override void SubscribeButtons()
    {
        base.SubscribeButtons();
        
        mainButton.OnClick.AddListener(Button_OnCLick);
    }


    protected override void UnSubscribeButtons()
    {
        base.UnSubscribeButtons();
        
        mainButton.OnClick.RemoveListener(Button_OnCLick);
    }


    private void NextState()
    {
        _currentState++;

        UpdateStates();

        if (_currentState >= stateObjects.Count)
        {
            //UIManager.Instance.SetTutorialShown();
            UIManager.Instance.ShowPopup(PopupType.Reward);
            
            Hide();
        }
    }


    private void UpdateStates()
    {
        for (int i = 0; i < stateObjects.Count; i++)
        {
            stateObjects[i].SetActive(_currentState == i);
        }
    }


    private void Button_OnCLick()
    {
        NextState();
    }
}