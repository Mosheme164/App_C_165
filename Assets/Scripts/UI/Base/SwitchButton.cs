using UnityEngine;


public class SwitchButton : ButtonBase
{
    [SerializeField] private GameObject onImage;
    [SerializeField] private GameObject offImage;

    protected bool _isActive;


    public bool IsActive => _isActive;
    
    
    public void SwitchState()
    {
        _isActive = !_isActive;

        UpdateVisuals();
    }


    public void SetState(bool isActive)
    {
        _isActive = isActive;

        UpdateVisuals();
    }


    protected virtual void UpdateVisuals()
    {
        onImage.SetActive(_isActive);
        offImage.SetActive(!_isActive);
    }
}
