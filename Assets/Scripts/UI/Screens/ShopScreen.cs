using UnityEngine;


public class ShopScreen : PopupBase
{
    [SerializeField] private ButtonBase closeButton;


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
