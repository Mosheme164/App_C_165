using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;


public class Spawner : MonoBehaviour
{
    [SerializeField] private float minSpawnTime;
    [SerializeField] private float maxSpawnTime;
    [SerializeField] private float timeChangeDuration;
    [Range(0f, 1f)]
    [SerializeField] private float bombChance;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Ball bombPrefab;
    [SerializeField] private List<Sprite> skins;

    private List<Ball> _currentBalls = new List<Ball>();
    private Sprite _currentSkin;
    private float _totalTimer;
    private float _timer;
    private bool _isPause;
    private bool _isFirst;

    private IDisposable _altitudeDisposable;


    public Ball TutorialBall => _currentBalls[0];


    private void Awake()
    {
        _isPause = false;
        _isFirst = true;
        
        GetSkin();
        StartSpawn();
    }


    private void OnDestroy()
    {
        _altitudeDisposable?.Dispose();
    }


    private void Update()
    {
        if (_isPause) return;

        UpdateTimer();
    }


    public void SetPause(bool isPause)
    {
        foreach (var ball in _currentBalls)
        {
            if (ball != null)
            {
                ball.SetPause(isPause);
            }
        }

        _isPause = isPause;
    }


    public void SetPriority(bool isPrior)
    {
        foreach (var ball in _currentBalls)
        {
            ball.SetPriority(isPrior);
        }
    }
    
    
    private void StartSpawn()
    {
        CreateBall();

        SetPause(false);

        _altitudeDisposable = _currentBalls[0].Altitude.Subscribe(value =>
        {
            if (value > 21f)
            {
                UIManager.Instance.GameScreen.SetTutorial();
                
                _altitudeDisposable?.Dispose();
            }
        }).AddTo(this);
    }
    
    
    private void UpdateTimer()
    {
        _totalTimer += Time.deltaTime;
        _timer += Time.deltaTime;

        var spawnTimeFactor = Mathf.InverseLerp(0, timeChangeDuration, _totalTimer);
        var currentSpawnTime = Mathf.Lerp(maxSpawnTime, minSpawnTime, spawnTimeFactor);

        if (_timer >= currentSpawnTime)
        {
            _timer -= currentSpawnTime;
            
            CreateBall();
        }
    }


    private void CreateBall()
    {
        var randomX = Random.Range(-4f, 4f);
        var offset = _isFirst
            ? Vector3.zero
            : Vector3.right * randomX;
        
        var randomNumber = Random.Range(0f, 1f);
        var currentBombChance = _isFirst ? -1f : bombChance;
        var prefab = randomNumber <= currentBombChance
            ? bombPrefab
            : ballPrefab;

        var newBall = Instantiate(prefab, transform);

        newBall.transform.position += offset;
        newBall.SetSkin(_currentSkin);
        newBall.SetPause(false);
        newBall.AddImpulse();
        
        _currentBalls.Add(newBall);

        _isFirst = false;
    }


    private void GetSkin()
    {
        var currentSkinIndex = SkinManager.Instance.CurrentIndex;

        _currentSkin = skins[currentSkinIndex];
    }
}
