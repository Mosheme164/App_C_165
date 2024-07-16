using System;
using System.Collections.Generic;
using Base;
using UnityEngine;


public class BonusManager : SingletonMonoBehaviour<BonusManager>
{
    private const string ConsecDaysKey = "ConsecDays";
    private const string LastVisitKey = "LastVisit";
    private const string LastVisitWheelKey = "LastVisitWheel";
    
    [SerializeField] private List<int> rewards;

    private int _lastVisitDay;
    private int _lastVisitWheelDay;
    private int _consecDays;


    public string TimeRemaining
    {
        get
        {
            var now = DateTime.UtcNow;

            return $"{23 - now.Hour}:{59 - now.Minute}:{59 - now.Second}";
        }
    }


    public int CurrentReward => rewards[_consecDays];


    public bool IsClaimable => CurrentDay != _lastVisitDay;


    public bool IsWheelClaimable => CurrentDay != _lastVisitWheelDay;


    private int CurrentDay = DateTime.UtcNow.Day;


    protected override void Awake()
    {
        base.Awake();
        
        LoadData();
    }


    public void ClaimReward()
    {
        _consecDays = CurrentDay == _lastVisitDay + 1
            ? Mathf.Clamp(_consecDays++, 0, 6)
            : 0;
        
        CurrencyManager.Instance.AddCurrency(CurrencyType.Coins, CurrentReward);

        _lastVisitDay = CurrentDay;
        
        SaveData();
        
        UIManager.Instance.UpdateBonus();
    }


    public void ClaimWheel(float amount)
    {
        CurrencyManager.Instance.AddCurrency(CurrencyType.Coins, amount);

        _lastVisitWheelDay = CurrentDay;
        
        SaveData();
    }


    private void SaveData()
    {
        PlayerPrefs.SetInt(LastVisitKey, _lastVisitDay);
        PlayerPrefs.SetInt(LastVisitWheelKey, _lastVisitWheelDay);
        PlayerPrefs.SetInt(ConsecDaysKey, _consecDays);
        PlayerPrefs.Save();
    }


    private void LoadData()
    {
        _lastVisitDay = PlayerPrefs.GetInt(LastVisitKey);
        _lastVisitWheelDay = PlayerPrefs.GetInt(LastVisitWheelKey);
        _consecDays = PlayerPrefs.GetInt(ConsecDaysKey);
    }
}