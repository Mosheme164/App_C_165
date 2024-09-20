using UnityEngine;
using UnityEngine.UI;


public class BonusScreen : PopupBase
{
    [SerializeField] private ButtonBase closeButton;
    [SerializeField] private Text rewardAmount;


    protected override void BeforeShow()
    {
        base.BeforeShow();

        BonusManager.Instance.ClaimReward();
        rewardAmount.text = BonusManager.Instance.CurrentReward.ToString();
    }


    protected override void SubscribeButtons()
    {
        base.SubscribeButtons();

        closeButton.OnClick.AddListener(CloseButton_OnClick);
    }


    protected override void UnSubscribeButtons()
    {
        base.UnSubscribeButtons();
        
        closeButton.OnClick.RemoveListener(CloseButton_OnClick);
    }


    private void CloseButton_OnClick()
    {
        Hide();
    }
}
