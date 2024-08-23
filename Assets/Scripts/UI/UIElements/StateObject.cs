using System.Collections.Generic;
using UnityEngine;


public class StateObject : MonoBehaviour
{
    [SerializeField] private int defaultState = 0;
    [SerializeField] private List<GameObject> states;

    protected int _currentState;
    

    protected virtual void Awake()
    {
        SetState(defaultState);
    }


    public void SetState(int index)
    {
        if (index >= states.Count)
        {
            Debug.LogError("StateObject state index overflow", this);
            return;
        }
        
        ChangeState(index);
    }


    private void ChangeState(int index)
    {
        _currentState = index;
        
        for (int i = 0; i < states.Count; i++)
        {
            states[i].SetActive(i == index);
        }
    }
}
