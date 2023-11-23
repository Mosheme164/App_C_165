using System.Collections.Generic;
using UnityEngine;


public class StateSelector : MonoBehaviour
{
    private List<GameObject> _objects = new List<GameObject>();
    
    
    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _objects.Add(transform.GetChild(i).gameObject);
        }
    }


    public void SetState(int state)
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            _objects[i].SetActive(i == state);
        }
    }
}
