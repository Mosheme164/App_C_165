using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ShopItem : MonoBehaviour
{
    public enum ShopItemState
    {
        None                = 0,
        
        Closed              = 1,
        Opened              = 2,
        Selected            = 3,
    }


    [SerializeField] private bool isStadium;
    [SerializeField] private int id;
    [SerializeField] private float price;
    [SerializeField] private List<Text> priceLabelsOld;
    [SerializeField] private List<TextMeshProUGUI> priceLabels;
    [SerializeField] private ButtonBase buyButton;
    [SerializeField] private ButtonBase selectButton;
    [Space]
    [SerializeField] private GameObject openedObject;
    [SerializeField] private GameObject selectedObject;
    [SerializeField] private GameObject closedObject;

    private ShopItemState _currentState;


    public int Id => id;
    

    private void Start()
    {
        Initialize();
    }


    private void OnDestroy()
    {
        CurrencyManager.Instance.OnCurrencyAmountChanged -= CurrencyAmount_OnChange;
    }


    private void Initialize()
    {
        buyButton.OnClick.AddListener(BuyButton_OnClick);
        selectButton.OnClick.AddListener(SelectButton_OnClick);

        CurrencyManager.Instance.OnCurrencyAmountChanged += CurrencyAmount_OnChange;

        Initialize(isStadium);
        
        SetPrice(price);
    }


    private void Initialize(bool isStadium)
    {
        ShopItemState initialState;
        
        if (isStadium)
        {
            SkinManager.Instance.OnSelect2.AddListener(Skin_OnSelect);
            
            initialState = !SkinManager.Instance.IsPurchased2(id)
                ? ShopItemState.Closed
                : SkinManager.Instance.CurrentIndex2 == id
                    ? ShopItemState.Selected
                    : ShopItemState.Opened;
        }
        else
        {
            SkinManager.Instance.OnSelect.AddListener(Skin_OnSelect);
            
            initialState = !SkinManager.Instance.IsPurchased(id)
                ? ShopItemState.Closed
                : SkinManager.Instance.CurrentIndex == id
                    ? ShopItemState.Selected
                    : ShopItemState.Opened;
        }
        
        ChangeState(initialState);
    }


    private void SetPrice(float amount)
    {
        foreach (var priceLabel in priceLabelsOld)
        {
            priceLabel.text = CurrencyCounter.ToIdleNotation((int)amount);
        }

        foreach (var priceLabel in priceLabels)
        {
            priceLabel.text = CurrencyCounter.ToIdleNotation((int)amount);
        }
    }


    private void Open()
    {
        if (CurrencyManager.Instance.TryRemoveCurrency(CurrencyType.Coins, price))
        {
            ChangeState(ShopItemState.Opened);

            if (isStadium)
            {
                SkinManager.Instance.BuyPack2(id);
            }
            else
            {
                SkinManager.Instance.BuyPack(id);
            }

            AudioManager.Instance.PlaySound(AudioClipType.Purchase);
        }
    }


    private void Select()
    {
        if (isStadium)
        {
            SkinManager.Instance.SelectPack2(id);
        }
        else
        {
            SkinManager.Instance.SelectPack(id);
        }

        ChangeState(ShopItemState.Selected);
    }


    private void SetInteractable(bool isInteractable)
    {
        buyButton.SetInteractable(isInteractable);

        if (priceLabels != null && priceLabels.Count > 1)
        {
            priceLabels[0].gameObject.SetActive(isInteractable);
            priceLabels[1].gameObject.SetActive(!isInteractable);
        }
        
        if (priceLabelsOld != null && priceLabelsOld.Count > 1)
        {
            priceLabelsOld[0].gameObject.SetActive(isInteractable);
            priceLabelsOld[1].gameObject.SetActive(!isInteractable);
        }
    } 
    

    private void ChangeState(ShopItemState state)
    {
        _currentState = state;
        
        openedObject.SetActive(state == ShopItemState.Opened);
        selectedObject.SetActive(state == ShopItemState.Selected);
        closedObject.SetActive(state == ShopItemState.Closed);

        SetInteractable(state != ShopItemState.Selected);
        CheckInteractable();
    }


    private void CheckInteractable()
    {
        if (_currentState == ShopItemState.Closed)
        {
            var coinsAmount = CurrencyManager.Instance.GetCurrencyAmount(CurrencyType.Coins);

            SetInteractable(coinsAmount >= price);
        }
    }


    private void BuyButton_OnClick()
    {
        if (_currentState == ShopItemState.Closed)
        {
            Open();
        }
    }
    
    
    private void SelectButton_OnClick()
    {
        if (_currentState == ShopItemState.Opened)
        {
            Select();
        }
    }


    private void CurrencyAmount_OnChange(CurrencyType type)
    {
        if (type == CurrencyType.Coins)
        {
            CheckInteractable();
        }
    }


    private void Skin_OnSelect()
    {
        if (_currentState == ShopItemState.Selected)
        {
            ChangeState(ShopItemState.Opened);
        }
    }
}