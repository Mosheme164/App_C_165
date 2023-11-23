using System;
using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Events;


public class PopupBase : MonoBehaviour
{
    [SerializeField] private PopupType type;
    [Header("Animation")]
    [SerializeField] private float animationDuration;
    [SerializeField] private AnimationCurve showCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] private AnimationCurve hideCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] protected Transform root;

    private Tween _scaleTween;


    public PopupType PopupType => type;


    public ReactiveProperty<bool> IsOpen { get; } = new ReactiveProperty<bool>(false);


    public UnityEvent OnShowStart { get; } = new UnityEvent();
    
    
    public UnityEvent OnShowEnd { get; } = new UnityEvent();
    
    
    public UnityEvent OnHideStart { get; } = new UnityEvent();
    
    
    public UnityEvent OnHideEnd { get; } = new UnityEvent();


    protected virtual void Awake()
    {
        
    }


    protected virtual void OnDestroy()
    {
        UnSubscribeButtons();
        
        _scaleTween?.Kill();
    }


    public void Show(bool isImmediately = false, Action callback = null)
    {
        IsOpen.Value = true;

        BeforeShow();
        
        if (isImmediately)
        {
            root.localScale = Vector3.one;
            
            AfterShow();
            
            callback?.Invoke();
        }
        else
        {
            _scaleTween?.Kill();
            _scaleTween = root.DOScale(Vector3.one, animationDuration)
                .SetEase(showCurve)
                .OnComplete(() =>
                {
                    AfterShow();
                    
                    callback?.Invoke();
                });
        }
    }


    public void Hide(bool isImmediately = false, Action callback = null)
    {
        BeforeHide();

        if (isImmediately)
        {
            root.localScale = Vector3.zero;
            
            AfterHide();
            
            callback?.Invoke();
            
            IsOpen.Value = false;
        }
        else
        {
            _scaleTween?.Kill();
            _scaleTween = root.DOScale(Vector3.zero, animationDuration)
                .SetEase(hideCurve)
                .OnComplete(() =>
                {
                    AfterHide();
                    
                    callback?.Invoke();
                    
                    IsOpen.Value = false;
                });
        }
    }


    protected virtual void BeforeShow()
    {
        gameObject.SetActive(true);
        
        OnShowStart?.Invoke();
    }


    protected virtual void AfterShow()
    {
        SubscribeButtons();
        
        OnShowEnd?.Invoke();
    }


    protected virtual void BeforeHide()
    {
        UnSubscribeButtons();
        
        OnHideStart?.Invoke();
    }


    protected virtual void AfterHide()
    {
        gameObject.SetActive(false);
        
        OnHideEnd?.Invoke();
    }


    protected virtual void SubscribeButtons()
    {
    }


    protected virtual void UnSubscribeButtons()
    {
    }
}