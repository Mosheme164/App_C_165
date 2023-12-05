using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class CounterBase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Text labelOld;

    protected int _currentValue = -1;


    public void UpdateValue(float value)
    {
        int newValue = (int)value;

        //if (newValue != _currentValue)
        {
            _currentValue = newValue;
            
            SetValue(GetFormattedString());
        }
    }


    protected virtual string GetFormattedString()
    {
        return _currentValue.ToString();
    }


    private void SetValue(string value)
    {
        if (label != null)
        {
            label.text = value;
        }

        if (labelOld != null)
        {
            labelOld.text = value;
        }
    }
}