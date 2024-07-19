using System;
using Base;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;


public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    [SerializeField] private int continuePrice = 100;
    [SerializeField] private Spawner spawnerPrefab;
    [SerializeField] private Transform spawnerRoot;

    private Spawner _spawner;
    private int _coinsAdded;
    private bool _isPause = true;


    public ReactiveProperty<int> CoinsCollected { get; } = new ReactiveProperty<int>();


    public int CoinsToAdd => CoinsCollected.Value - _coinsAdded;


    private void Update()
    {
        if (_isPause) return;

        InputUpdate();
    }
    
    
    public bool CanContinue()
    {
        var bankAmount = CurrencyManager.Instance.GetCurrencyAmount(CurrencyType.Coins);
        var totalAmount = (int)bankAmount + CoinsToAdd;

        return totalAmount >= continuePrice;
    }
    
    
    public bool TryContinue()
    {
        var bankAmount = CurrencyManager.Instance.GetCurrencyAmount(CurrencyType.Coins);
        var totalAmount = (int)bankAmount + CoinsToAdd;

        if (totalAmount < continuePrice) return false;
        
        if (CoinsToAdd >= continuePrice)
        {
            _coinsAdded += continuePrice;
            return true;
        }
        
        var coinsToRemove = continuePrice - CoinsToAdd;
        
        _coinsAdded = CoinsCollected.Value;

        return CurrencyManager.Instance.TryRemoveCurrency(CurrencyType.Coins, coinsToRemove);
    }
    
    
    public void CollectCoins()
    {
        CurrencyManager.Instance.AddCurrency(CurrencyType.Coins, CoinsToAdd);
        
        _coinsAdded = CoinsCollected.Value;
    }


    public void SetPriority(bool isPrior)
    {
        _spawner.SetPriority(isPrior);
    }


    public void PopFirst()
    {
        var tutorialBall = _spawner.TutorialBall;

        Pop(tutorialBall);
    }


    public void StartGame()
    {
        _spawner = Instantiate(spawnerPrefab, spawnerRoot);
        
        UIManager.Instance.GameScreen.SetPauseButton(true);
        _spawner.SetPause(false);
        _isPause = false;

        CoinsCollected.Value = 0;
        _coinsAdded = 0;
    }


    public void SetPause(bool isPause)
    {
        _spawner.SetPause(isPause);
        _isPause = isPause;
    }


    public void ContinueGame()
    {
        UIManager.Instance.GameScreen.SetPauseButton(true);
        _spawner.SetPause(false);
        _isPause = false;
    }


    public void RestartGame()
    {
        ClearGame();
        StartGame();
    }


    public void FinishGame()
    {
        _spawner.SetPause(true);
        _isPause = true;
        
        UIManager.Instance.GameScreen.SetPauseButton(false);

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
                Explode(ball);
            }
            else
            {
                Pop(ball);
            }
        }
    }


    private void Pop(Ball ball)
    {
        UIManager.Instance.GameScreen.CreateBubble();
        AddCoin();

        Destroy(ball.gameObject);
                
        AudioManager.Instance.PlaySound(AudioClipType.ClickSlots);
    }


    private void Explode(Ball ball)
    {
        ball.Explode();
                
        AudioManager.Instance.PlaySound(AudioClipType.Lose);

        FinishGame();
    }
}
