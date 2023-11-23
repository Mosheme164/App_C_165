using UnityEngine;


public class ProgressBar : MonoBehaviour
{
    [SerializeField] private bool isWidth;
    [SerializeField] private bool isTurnOff;
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private RectTransform fillTransform;


    private float _currentValue = 1f;
    

    public void SetValue(float value)
    {
        value = Mathf.Clamp01(value);
        
        if (!Mathf.Approximately(_currentValue, value))
        {
            _currentValue = value;
            
            UpdateVisuals();
        }
    }


    private void UpdateVisuals()
    {
        float height = isWidth
            ? fillTransform.sizeDelta.y
            : Mathf.Lerp(minSize, maxSize, _currentValue);

        float width = isWidth
            ? Mathf.Lerp(minSize, maxSize, _currentValue)
            : fillTransform.sizeDelta.x;

        if (isTurnOff)
        {
            bool isZero = Mathf.Approximately(_currentValue, 0f);
            fillTransform.gameObject.SetActive(!isZero);
        }

        fillTransform.sizeDelta = new Vector2(width, height);
    }
}
