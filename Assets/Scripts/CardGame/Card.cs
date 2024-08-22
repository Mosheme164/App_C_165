using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


public class Card : StateObject
{
    public event Action<Card> OnClick = delegate {  };
    
    
    [SerializeField] private Image openedImage;
    [SerializeField] private Button button;
    
    private int _currentValue;
    private Sequence _swapSequence;


    public int CurrentValue => _currentValue;


    private void Start()
    {
        button.onClick.AddListener(() => OnClick?.Invoke(this));
    }


    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners(); 
    }


    public void Initialize(int value, Sprite sprite)
    {
        _currentValue = value;
        openedImage.sprite = sprite;

        SetState(0);
    }


    public void SetPause(bool isPause)
    {
        if (isPause)
        {
            _swapSequence?.Pause();
        }
        else
        {
            _swapSequence?.Play();
        }
    }


    public void SwapCard(Action callback = null)
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
}