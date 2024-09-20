using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


public class CardController : MonoBehaviour
{
    public event Action<bool> OnGameFinish = delegate { };


    private const float GameDuration = 106f;

    [SerializeField] private GameObject fader;
    [SerializeField] private GameObject raycastBlocker;
    [SerializeField] private Transform cardsRoot;
    [SerializeField] private List<Sprite> cardSprites;

    private List<Card> _cards = new List<Card>();
    private List<Card> _flippedCards = new List<Card>();
    private Action _delayedAction;
    private float _delayTimer;
    private bool _isPause;


    public ReactiveProperty<float> TimeLeft { get; } = new ReactiveProperty<float>();
    
    
    public ReactiveProperty<int> PairsCount { get; } = new ReactiveProperty<int>();

    
    public ReactiveProperty<int> CurrentScore { get; } = new ReactiveProperty<int>();



    private void Awake()
    {
        _cards = cardsRoot.GetComponentsInChildren<Card>().ToList();

        SubscribeCards();
    }


    private void Update()
    {
        UpdateTimers();
    }


    private void OnDestroy()
    {
        OnGameFinish = null;
    }


    public void StartGame()
    {
        ResetValues();
        ResetCards();
        
        SetFader(false);
        SetPause(false);
    }


    public void SetPause(bool isPause)
    {
        _isPause = isPause;
        
        raycastBlocker.SetActive(isPause);

        foreach (var card in _cards)
        {
            card.SetPause(isPause);
        }
    }
    
    
    private void UpdateTimers()
    {
        if (_isPause) return;

        if (_delayTimer > 0f)
        {
            _delayTimer -= Time.deltaTime;

            if (_delayTimer <= 0f)
            {
                _delayedAction?.Invoke();
                _delayedAction = null;
            }
        }

        if (TimeLeft.Value > 0f)
        {
            TimeLeft.Value -= Time.deltaTime;

            if (TimeLeft.Value <= 0f)
            {
                FinishGame(false);
            }
        }
    }


    private void FinishGame(bool isWin)
    {
        SetPause(true);

        OnGameFinish?.Invoke(isWin);
    }
    
    
    private void ResetValues()
    {
        TimeLeft.Value = GameDuration;
        CurrentScore.Value = 0;
        _delayTimer = 0f;
        _delayedAction = null;
    }


    private void ResetCards()
    {
        var values = GetValuesList();
        
        foreach (var card in _cards)
        {
            var randomIndex = Random.Range(0, values.Count);
            var value = values[randomIndex];

            values.RemoveAt(randomIndex);
            
            card.gameObject.SetActive(true);
            card.Initialize(value, cardSprites[value]);
            card.ResetState();
        }
        
        _flippedCards.Clear();
    }


    private void SubscribeCards()
    {
        foreach (var card in _cards)
        {
            card.OnClick += Card_OnClick;
        }
    }


    private void SetFader(bool isActive)
    {
        fader.gameObject.SetActive(isActive);
    }
    

    private List<int> GetValuesList()
    {
        var result = new List<int>();
        var currentValue = -1;
        
        for (int i = 0; i < _cards.Count; i++)
        {
            if (i % 2 == 0)
            {
                currentValue++;
            }

            result.Add(currentValue);
        }

        PairsCount.Value = currentValue + 1;
        
        return result;
    }


    private void CheckPair()
    {
        var firstCard = _flippedCards[0];
        var secondCard = _flippedCards[1];
        
        if (firstCard.CurrentValue == secondCard.CurrentValue)
        {
            CurrentScore.Value++;
            
            AudioManager.Instance.PlaySound(AudioClipType.Success);
            
            SetFader(true);
            
            firstCard.RemoveCard();
            secondCard.RemoveCard(() =>
            {
                _flippedCards.Clear();

                SetFader(false);
                
                if (CurrentScore.Value >= PairsCount.Value)
                {
                    FinishGame(true);
                }
            });
        }
        else
        {
            AudioManager.Instance.PlaySound(AudioClipType.Denial);
            
            _delayTimer = .5f;
            _delayedAction += () =>
            {
                firstCard.FlipCard();
                secondCard.FlipCard(() =>
                {
                    _flippedCards.Clear();
                });
            };
        }
    }


    private void Card_OnClick(Card card)
    {
        if (_flippedCards.Count >= 2) return;
        if (_flippedCards.Contains(card)) return;
        
        _flippedCards.Add(card);
        
        AudioManager.Instance.PlaySound(AudioClipType.Click);

        if (_flippedCards.Count >= 2)
        {
            card.FlipCard(CheckPair);
        }
        else
        {
            card.FlipCard();
        }
    }
}