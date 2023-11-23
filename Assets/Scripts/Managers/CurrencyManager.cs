using System;
using System.Collections.Generic;
using System.Linq;
using Base;
using UnityEngine;


public class CurrencyManager : SingletonMonoBehaviour<CurrencyManager>
{
    [Serializable]
    private class CurrencyDataList
    {
        public List<CurrencyData> currencyDatas;
    }
    
    
    [Serializable]
    private class CurrencyData
    {
        public CurrencyType type;
        public float amount;
    }


    public event Action<CurrencyType> OnCurrencyAmountChanged = delegate { };


    private const string CurrencyDataKey = "CurrencyData";

    
    private List<CurrencyData> _currencyDataList = new List<CurrencyData>();


    protected override void Awake()
    {
        base.Awake();

        LoadData();
    }


    public float GetCurrencyAmount(CurrencyType currencyType)
    {
        var currencyData = GetData(currencyType);

        return currencyData.amount;
    }


    public void SetCurrency(CurrencyType currencyType, float currencyAmount)
    {
        var currencyData = GetData(currencyType);

        currencyData.amount = currencyAmount;
        
        SaveData();
        
        OnCurrencyAmountChanged?.Invoke(currencyType);
    }


    public void AddCurrency(CurrencyType currencyType, float currencyAmount)
    {
        if (currencyAmount <= 0) return;

        var currencyData = GetData(currencyType);

        currencyData.amount += currencyAmount;

        SaveData();
        
        OnCurrencyAmountChanged?.Invoke(currencyType);
    }


    public bool TryRemoveCurrency(CurrencyType currencyType, float currencyAmount)
    {
        var absoluteAmount = Mathf.Abs(currencyAmount);
        var currencyData = GetData(currencyType);

        if (currencyData.amount < absoluteAmount)
        {
            return false;
        }

        currencyData.amount -= absoluteAmount;
        
        SaveData();
        
        OnCurrencyAmountChanged?.Invoke(currencyType);

        return true;
    }


    private CurrencyData GetData(CurrencyType currencyType)
    {
        var currencyData = _currencyDataList.FirstOrDefault(data => data.type == currencyType);

        if (currencyData == null)
        {
            currencyData = CreateData(currencyType, 0f);
        }

        return currencyData;
    }


    private CurrencyData CreateData(CurrencyType currencyType, float amount)
    {
        var newData = new CurrencyData
        {
            type = currencyType,
            amount = amount,
        };

        _currencyDataList.Add(newData);

        return newData;
    }
    

    private void SaveData()
    {
        var currencyDataList = new CurrencyDataList()
        {
            currencyDatas = _currencyDataList
        };
        
        string jsonData = JsonUtility.ToJson(currencyDataList);
        
        PlayerPrefs.SetString(CurrencyDataKey, jsonData);
        PlayerPrefs.Save();
    }


    private void LoadData()
    {
        string jsonData = PlayerPrefs.GetString(CurrencyDataKey);
        
        if (!string.IsNullOrEmpty(jsonData))
        {
            var currencyDataList = JsonUtility.FromJson<CurrencyDataList>(jsonData);
            
            _currencyDataList = currencyDataList.currencyDatas;
        }
        else
        {
            CreateDefaultData();
            
            AddCurrency(CurrencyType.Coins, 6f);
        }
    }


    private void CreateDefaultData()
    {
        var coinsData = new CurrencyData()
        {
            type = CurrencyType.Coins,
            amount = 0,
        };
            
        var locationData = new CurrencyData()
        {
            type = CurrencyType.LocationIndex,
            amount = 0,
        };

        _currencyDataList.Add(coinsData);
        _currencyDataList.Add(locationData);
    }
}