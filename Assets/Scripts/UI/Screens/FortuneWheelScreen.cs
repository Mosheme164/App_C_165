using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;


public class FortuneWheelScreen : PopupBase
{
    [Space] 
    [SerializeField] private FortuneWheel wheel;
    [SerializeField] private StateObject resultStateObject;
    [SerializeField] private Text rewardLabel;
    [Space]
    [SerializeField] private ButtonBase spinButton;

    private float _currentAngle;
    

    protected override void Awake()
    {
        base.Awake();
        
        spinButton.SetInteractable(true);
        wheel.ResetWheel();

        SetStateObject(0);
    }


    private void Subscribe()
    {
        spinButton.OnClick.AddListener(SpinButton_OnClick);
    }


    private void UnSubscribe()
    {
        spinButton.OnClick.RemoveListener(SpinButton_OnClick);
    }


    private void SetStateObject(int stateIndex)
    {
        resultStateObject.SetState(stateIndex);
        
        switch (stateIndex)
        {
            case 0:
                spinButton.SetInteractable(false);
                break;
            case 1:
                spinButton.SetInteractable(false);
                break;
            case 2:
                spinButton.SetInteractable(true);
                break;
        }

        if (!BonusManager.Instance.IsWheelClaimable)
        {
            Observable.Timer(TimeSpan.FromSeconds(4f)).Subscribe(_ =>
            {
                UIManager.Instance.ShowPopup(PopupType.Menu);
                Hide();
            }).AddTo(this);
        }
    }


    private void CreateRewards()
    {
        var reward = wheel.GetRandomReward(out var angle);

        wheel.Rotate(angle, () =>
        {
            GetReward(reward);
        });
    }


    private void GetReward(FortuneWheel.FortuneWheelReward reward)
    {
        if (reward.type == FortuneWheel.FortuneWheelRewardType.Coins)
        {
            rewardLabel.text = $"+{reward.amount} coins";

            BonusManager.Instance.ClaimWheel(reward.amount);
            AudioManager.Instance.PlaySound(AudioClipType.SpinWin);
            
            SetStateObject(1);
        }
        else if (reward.type == FortuneWheel.FortuneWheelRewardType.Respin)
        {
            SetStateObject(2);
        }
        else
        {
            BonusManager.Instance.ClaimWheel(0f);
            AudioManager.Instance.PlaySound(AudioClipType.SpinWin);

            SetStateObject(0);
        }
    }


    protected override void AfterShow()
    {
        base.AfterShow();
        
        Subscribe();
        
        spinButton.SetInteractable(true);
    }


    protected override void BeforeHide()
    {
        base.BeforeHide();
        
        UnSubscribe();
    }


    private void SpinButton_OnClick()
    {
        spinButton.SetInteractable(false);
        
        CreateRewards();
    }
}
