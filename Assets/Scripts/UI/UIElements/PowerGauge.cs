using UnityEngine;


public class PowerGauge : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Transform arrow;
    [SerializeField] private Transform minPosition;
    [SerializeField] private Transform maxPosition;
    [SerializeField] private float speed = 1f;

    private RectTransform _rectTransform;
    private float _currentValue;
    private float _direction;
    private bool _isMoving;


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        
        _direction = 1f;
    }


    private void Update()
    {
        if (_isMoving)
        {
            UpdateMovement();
            UpdateVisuals();
        }
    }


    public void SetPriority(bool isPrior)
    {
        canvas.sortingOrder = isPrior
            ? 20
            : 9;
    }


    public float GetValue()
    {
        return Mathf.Clamp01(_currentValue);
    }
    
    
    public void SetAnchoredPosition(Vector2 position)
    {
        _rectTransform.anchoredPosition = position;
    }


    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }


    public void SetMovement(bool isMoving)
    {
        _isMoving = isMoving;
    }


    private void UpdateMovement()
    {
        var delta = _direction * speed * Time.deltaTime;
        _currentValue += delta;

        if (_currentValue < 0f)
        {
            _direction = -_direction;
            _currentValue = -_currentValue;
        }
        else if (_currentValue > 1f)
        {
            _direction = -_direction;
            _currentValue = 1f - (_currentValue - 1f);
        }
    }


    private void UpdateVisuals()
    {
        var position = Vector3.Lerp(minPosition.position, maxPosition.position, _currentValue);

        arrow.position = position;
    }
}
