using System;
using Base;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;


public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    [SerializeField] private Spawner spawnerPrefab;
    [SerializeField] private Transform spawnerRoot;

    private Spawner _spawner;
    private bool _isPause = true;


    public ReactiveProperty<int> CoinsCollected { get; } = new ReactiveProperty<int>();


    private void Update()
    {
        if (_isPause) return;

        InputUpdate();
    }


    public void StartGame()
    {
        _spawner = Instantiate(spawnerPrefab, spawnerRoot);
        
        _isPause = false;
        _spawner.SetPause(false);

        CoinsCollected.Value = 0;

        //UIManager.Instance.GameScreen.SetButtons(true);
    }


    public void SetPause(bool isPause)
    {
        _spawner.SetPause(isPause);
    }


    public void ContinueGame()
    {
        _spawner.SetPause(false);
    }


    public void RestartGame()
    {
        ClearGame();
        StartGame();
    }


    public void FinishGame()
    {
        _spawner.SetPause(true);
        
        //UIManager.Instance.GameScreen.SetButtons(false);

        Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ =>
        {
            UIManager.Instance.ShowResult();
        }).AddTo(this);
    }


    public void ClearGame()
    {
        if (_spawner != null)
        {
            Destroy(_spawner.gameObject);
            
            _spawner = null;
        }
    }


    public void AddCoin()
    {
        CoinsCollected.Value += 1;
    }


    private void InputUpdate()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0) &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            OnMouseDown();
        }
#else
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began && 
                !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                OnMouseDown();
            }
        }
#endif
    }


    private void OnMouseDown()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        var raycastHit = Physics2D.Raycast(ray.origin, ray.direction);

        if (raycastHit.collider != null &&
            raycastHit.collider.TryGetComponent<Ball>(out var ball))
        {
            if (ball.IsBomb)
            {
                ball.Explode();
                
                AudioManager.Instance.PlaySound(AudioClipType.Lose);

                FinishGame();
            }
            else
            {
                UIManager.Instance.GameScreen.CreateBubble();
                AddCoin();

                Destroy(ball.gameObject);
                
                AudioManager.Instance.PlaySound(AudioClipType.ClickSlots);
            }
        }
    }
}
