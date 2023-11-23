using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[RequireComponent(typeof(EventTrigger))]
public class PageScrollView : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private int pageCount;
    [SerializeField] private float minVelocity = 200f;
    [Header("Page Index (Unnecessary)")]
    [SerializeField] private SwitchButton pageIndexViewPrefab;
    [SerializeField] private Transform pageIndexViewRoot;
    [Header("Page Buttons (Unnecessary)")]
    [SerializeField] private ButtonBase backButton;
    [SerializeField] private ButtonBase forwardButton;

    private List<SwitchButton> _pageIndexViews = new List<SwitchButton>();
    private int _pageIndex;
    private float _gap;
    private float _targetPosition;
    private bool _isReleased;


    private void Start()
    {
        _gap = 1f / (pageCount - 1f);
        _targetPosition = 0f;
        _pageIndex = 0;
        _isReleased = true;
        
        SubscribeButtons();
        CreateIndexViews();
        UpdatePageCounter();
    }


    private void Update()
    {
        if (_isReleased)
        {
            var delta = Mathf.Abs(scrollView.horizontalNormalizedPosition - _targetPosition);
            var speed = delta < 0.003f
                ? 1f
                : 10f * Time.deltaTime;

            var newPosition = Mathf.Lerp(scrollView.horizontalNormalizedPosition, _targetPosition, speed);

            scrollView.horizontalNormalizedPosition = newPosition;
        }
    }


    public void ResetDrag()
    {
        scrollView.horizontalNormalizedPosition = _targetPosition;
    }


    public void OnDragBegin()
    {
        _isReleased = false;
    }


    public void OnDragEnd()
    {
        if (scrollView.velocity.x > minVelocity ||
            _targetPosition - scrollView.horizontalNormalizedPosition > _gap * .5f)
        {
            PrevPage();
        }
        else if (scrollView.velocity.x < -minVelocity ||
                 scrollView.horizontalNormalizedPosition - _targetPosition > _gap * .5f)
        {
            NextPage();
        }
        
        scrollView.velocity = Vector2.zero;
        _isReleased = true;
    }


    private void PrevPage()
    {
        if (_targetPosition > 0f)
        {
            _targetPosition -= _gap;
            _pageIndex--;
            
            UpdatePageCounter();
        }
    }


    private void NextPage()
    {
        if (_targetPosition < 1f)
        {
            _targetPosition += _gap;
            _pageIndex++;
            
            UpdatePageCounter();
        }
    }


    private void UpdatePageCounter()
    {
        for (int i = 0; i < _pageIndexViews.Count; i++)
        {
            _pageIndexViews[i].SetState(i == _pageIndex);
        }

        if (backButton != null)
        {
            backButton.gameObject.SetActive(_pageIndex > 0);
        }

        if (forwardButton != null)
        {
            forwardButton.gameObject.SetActive(_pageIndex < pageCount - 1);
        }
    }
    
    
    private void CreateIndexViews()
    {
        if (pageIndexViewPrefab == null) return;

        for (int i = 0; i < pageCount; i++)
        {
            var newView = Instantiate(pageIndexViewPrefab, pageIndexViewRoot);

            newView.SetState(false);

            _pageIndexViews.Add(newView);
        }
    }


    private void SubscribeButtons()
    {
        if (backButton != null)
        {
            backButton.OnClick.AddListener(PrevPage);
        }

        if (forwardButton != null)
        {
            forwardButton.OnClick.AddListener(NextPage);
        }
    }
}
