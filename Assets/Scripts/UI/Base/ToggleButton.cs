using UnityEngine;
using UnityEngine.UI;


public class ToggleButton : SwitchButton
{
    [SerializeField] private InputField inputField;
    [SerializeField] private string text;


    protected override void Awake()
    {
        base.Awake();

        OnClick.AddListener(() => inputField.text = text);
    }


    public void OnValueChanged()
    {
        _isActive = inputField.text == text;
        
        UpdateVisuals();
    }


    protected override void UpdateVisuals()
    {
        base.UpdateVisuals();
        
        ChangeInteractable(!_isActive);
    }
}
