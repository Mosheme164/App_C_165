using System;
using System.Collections.Generic;
using Base;
using UnityEngine;


public class BonusManager : SingletonMonoBehaviour<BonusManager>
{
    private const string ConsecDaysKey = "ConsecDays";
    private const string LastVisitKey = "LastVisit";
    
    [SerializeField] private List<int> rewards;

    private int _lastVisitDay;
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


    private void SaveData()
    {
        PlayerPrefs.SetInt(LastVisitKey, _lastVisitDay);
        PlayerPrefs.SetInt(ConsecDaysKey, _consecDays);
        PlayerPrefs.Save();
    }


    private void LoadData()
    {
        _lastVisitDay = PlayerPrefs.GetInt(LastVisitKey);
        _consecDays = PlayerPrefs.GetInt(ConsecDaysKey);
    }
}
