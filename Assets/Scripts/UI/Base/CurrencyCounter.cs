using UnityEngine;


public class CurrencyCounter : CounterBase
{
    [SerializeField] private CurrencyType currencyType;

    private CurrencyManager _cachedCurrencyManager;
    

    public CurrencyType CurrencyType => currencyType;


    private void Awake()
    {
        _cachedCurrencyManager = CurrencyManager.Instance;
        
        _cachedCurrencyManager.OnCurrencyAmountChanged += CurrencyManager_OnChange;
        
        UpdateCurrency();
    }


    private void OnDestroy()
    {
        _cachedCurrencyManager.OnCurrencyAmountChanged -= CurrencyManager_OnChange;
    }


    private void UpdateCurrency()
    {
        var newValue = _cachedCurrencyManager.GetCurrencyAmount(currencyType);
        
        UpdateValue(newValue);
    }
    
    
    private static string ToSpaceEachThree(int value)
    {
        return value.ToString("### ### ### ### ### ### ##0").TrimStart(' ');
    }


    protected override string GetFormattedString()
    {
        return ToSpaceEachThree(_currentValue);
    }


    private void CurrencyManager_OnChange(CurrencyType type)
    {
        if (type != currencyType) return;

        UpdateCurrency();
    }
}
