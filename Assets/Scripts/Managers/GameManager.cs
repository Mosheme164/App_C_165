using System.Collections.Generic;
using Base;
using UnityEngine;


public class GameManager : SingletonMonoBehaviour<GameManager>
{
    [SerializeField] private List<GameObject> _managerPrefabs;


    protected override void Awake()
    {
        base.Awake();

        foreach (var prefab in _managerPrefabs)
        {
            Instantiate(prefab, transform);
        }
        
        AudioManager.Instance.PlayMusic();
    }
}