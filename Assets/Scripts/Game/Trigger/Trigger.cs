using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;


public class Trigger<T> : MonoBehaviour where T : MonoBehaviour
{
    private List<Collider2D> _colliders = new List<Collider2D>();
    
    
    public ReactiveProperty<T> TriggerSource { get; } = new ReactiveProperty<T>();


    private void Awake()
    {
        TriggerSource.Value = null;

        _colliders = GetComponents<Collider2D>().ToList();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out T source))
        {
            TriggerSource.Value = source;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent(out T source))
        {
            TriggerSource.Value = null;
        }
    }


    public void SetTrigger(bool isActive)
    {
        foreach (var collider in _colliders)
        {
            collider.enabled = isActive;
        }
    }
}
