using System;
using System.Collections.Generic;
using Base;
using UnityEngine;
using UnityEngine.Events;


public class SkinManager : SingletonMonoBehaviour<SkinManager>
{
    [Serializable]
    private class PurchaseData
    {
        public List<int> purchasedIndexes;
    }
    
    
    private const string CurrentSkinIndexKey = "CurrentSkinIndex";
    private const string PurchasedSkinsKey = "PurchasedSkins";
    
    private PurchaseData _purchaseData;
    private int _currentSkinIndex;


    public int CurrentIndex => _currentSkinIndex;


    public int PurchasedAmount => _purchaseData.purchasedIndexes.Count;
    

    public UnityEvent OnSelect { get; } = new UnityEvent();

    
    public UnityEvent OnBuy { get; } = new UnityEvent();

    
    protected override void Awake()
    {
        base.Awake();
        
        LoadData();
    }


    private void OnDestroy()
    {
        OnSelect.RemoveAllListeners();
        OnBuy.RemoveAllListeners();
    }


    public void BuyPack(int packIndex)
    {
        _purchaseData.purchasedIndexes.Add(packIndex);
        
        SaveData();
        
        OnBuy?.Invoke();
    }


    public void SelectPack(int packIndex)
    {
        _currentSkinIndex = packIndex;
        
        SaveData();
        
        OnSelect?.Invoke();
    }


    public bool IsPurchased(int packIndex)
    {
        return _purchaseData.purchasedIndexes.Contains(packIndex);
    }

    private void SaveData()
    {
        string jsonData = JsonUtility.ToJson(_purchaseData);
        
        PlayerPrefs.SetString(PurchasedSkinsKey, jsonData);
        PlayerPrefs.SetInt(CurrentSkinIndexKey, _currentSkinIndex);
        PlayerPrefs.Save();
    }


    private void LoadData()
    {
        _currentSkinIndex = PlayerPrefs.GetInt(CurrentSkinIndexKey);
        
        string jsonData = PlayerPrefs.GetString(PurchasedSkinsKey);

        if (!string.IsNullOrEmpty(jsonData))
        {
            _purchaseData = JsonUtility.FromJson<PurchaseData>(jsonData);
        }
        else
        {
            CreateDefaultData();
        }
    }
    
    
    private void CreateDefaultData()
    {
        _purchaseData = new PurchaseData()
        {
            purchasedIndexes = new List<int>{0}
        };

        _currentSkinIndex = 0;
    }
}
