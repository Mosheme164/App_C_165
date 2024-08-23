using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class Card : StateObject
{
    public event Action<Card> OnClick = delegate { };
    
    
    [SerializeField] private Image openedImage;
    [SerializeField] private Button button;
    
    private int _currentValue;
    private Vector3 defaultPosition;
    private Sequence _swapSequence;
    private Tween _removeTween;


    public int CurrentValue => _currentValue;


    protected override void Awake()
    {
        base.Awake();
        
        defaultPosition = transform.localPosition;
        button.onClick.AddListener(() => OnClick?.Invoke(this));
    }


    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
        OnClick = null;
    }


    public void Initialize(int value, Sprite sprite)
    {
        _currentValue = value;
        openedImage.sprite = sprite;

        SetState(0);
    }


    public void ResetPosition()
    {
        transform.localPosition = defaultPosition;
    }


    public void SetPause(bool isPause)
    {
        if (isPause)
        {
            _swapSequence?.Pause();
            _removeTween?.Pause();
        }
        else
        {
            _swapSequence?.Play();
            _removeTween?.Play();
        }
    }


    public void FlipCard(Action callback = null)
    {
        var targetState = _currentState == 0 ? 1 : 0;
        
        _swapSequence?.Kill();
        _swapSequence = DOTween.Sequence();

        _swapSequence.Append(transform.DOScaleX(0f, .2f).SetEase(Ease.Linear)
            .OnComplete(() => SetState(targetState)));
        _swapSequence.Append(transform.DOScaleX(1f, .2f).SetEase(Ease.Linear));
        _swapSequence.OnComplete(() => callback?.Invoke());

        _swapSequence.Play();
    }


    public void RemoveCard(Action callback = null)
    {
        _removeTween?.Kill();
        _removeTween = transform.DOLocalMove(defaultPosition + Vector3.up * 300f, 1f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                
                callback?.Invoke();
            });
    }
}