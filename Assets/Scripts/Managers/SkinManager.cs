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
        public List<int> purchasedIndexes2;
    }
    
    
    private const string CurrentSkinIndexKey = "CurrentSkinIndex";
    private const string CurrentSkinIndexKey2 = "CurrentSkinIndex2";
    private const string PurchasedSkinsKey = "PurchasedSkins";
    
    private PurchaseData _purchaseData;
    private int _currentSkinIndex;
    private int _currentSkinIndex2;


    public int CurrentIndex => _currentSkinIndex;

    
    public int CurrentIndex2 => _currentSkinIndex2;


    public int PurchasedAmount => _purchaseData.purchasedIndexes.Count;
    

    public UnityEvent OnSelect { get; } = new UnityEvent();
    
    
    public UnityEvent OnSelect2 { get; } = new UnityEvent();


    public UnityEvent OnBuy { get; } = new UnityEvent();

    
    public UnityEvent OnBuy2 { get; } = new UnityEvent();

    
    protected override void Awake()
    {
        base.Awake();
        
        LoadData();
    }


    private void OnDestroy()
    {
        OnSelect.RemoveAllListeners();
        OnBuy.RemoveAllListeners();
        OnSelect2.RemoveAllListeners();
        OnBuy2.RemoveAllListeners();
    }


    public void BuyPack(int packIndex)
    {
        _purchaseData.purchasedIndexes.Add(packIndex);
        
        SaveData();
        
        OnBuy?.Invoke();
    }
    
    
    public void BuyPack2(int packIndex)
    {
        _purchaseData.purchasedIndexes2.Add(packIndex);
        
        SaveData();
        
        OnBuy2?.Invoke();
    }


    public void SelectPack(int packIndex)
    {
        _currentSkinIndex = packIndex;
        
        SaveData();
        
        OnSelect?.Invoke();
    }
    
    
    public void SelectPack2(int packIndex)
    {
        _currentSkinIndex2 = packIndex;
        
        SaveData();
        
        OnSelect2?.Invoke();
    }


    public bool IsPurchased(int packIndex)
    {
        return _purchaseData.purchasedIndexes.Contains(packIndex);
    }
    
    
    public bool IsPurchased2(int packIndex)
    {
        return _purchaseData.purchasedIndexes2.Contains(packIndex);
    }


    private void SaveData()
    {
        string jsonData = JsonUtility.ToJson(_purchaseData);
        
        PlayerPrefs.SetString(PurchasedSkinsKey, jsonData);
        PlayerPrefs.SetInt(CurrentSkinIndexKey, _currentSkinIndex);
        PlayerPrefs.SetInt(CurrentSkinIndexKey2, _currentSkinIndex2);
        PlayerPrefs.Save();
    }


    private void LoadData()
    {
        _currentSkinIndex = PlayerPrefs.GetInt(CurrentSkinIndexKey);
        _currentSkinIndex2 = PlayerPrefs.GetInt(CurrentSkinIndexKey2);
        
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
            purchasedIndexes = new List<int>{0},
            purchasedIndexes2 = new List<int>{0}
        };

        _currentSkinIndex = 0;
        _currentSkinIndex2 = 0;
    }
}
