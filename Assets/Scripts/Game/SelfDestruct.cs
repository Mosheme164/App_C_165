using System;
using UniRx;
using UnityEngine;


public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float duration;

    
    private void Awake()
    {
        Observable.Timer(TimeSpan.FromSeconds(duration)).Subscribe(_ =>
        {
            Destroy(gameObject);
        }).AddTo(this);
    }
}
