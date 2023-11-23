using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class ButtonBase : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private bool isSound = true;


    public UnityEvent OnClick { get; } = new UnityEvent();


    protected virtual void Awake()
    {
        button.onClick.AddListener(Button_OnClick);
    }


    protected virtual void OnDestroy()
    {
        button.onClick.RemoveListener(Button_OnClick);
        
        OnClick.RemoveAllListeners();
    }


    public void Show()
    {
        gameObject.SetActive(true);
        
        ChangeInteractable(true);
    }


    public void Hide()
    {
        gameObject.SetActive(false);
        
        ChangeInteractable(false);
    }


    public void SetInteractable(bool isInteractable)
    {
        ChangeInteractable(isInteractable);
    }


    private async void Button_OnClick()
    {
        if (isSound)
        {
            AudioManager.Instance.PlaySound(AudioClipType.Click);
        }

        if (await IsChecked())
        {
            OnClick?.Invoke();
        }
    }


    protected virtual void ChangeInteractable(bool isInteractable)
    {
        button.interactable = isInteractable;
    }


    protected virtual Task<bool> IsChecked()
    {
        return Task.FromResult(true);
    }
}