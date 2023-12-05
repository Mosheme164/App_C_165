using System.Collections.Generic;
using UnityEngine;


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


    private void Awake()
    {
        _isPause = false;
        
        GetSkin();
        StartSpawn();
    }


    private void Update()
    {
        if (_isPause) return;

        UpdateTimer();
    }


    public void StartSpawn()
    {
        CreateBall();

        SetPause(false);
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
        var offset = Vector3.right * randomX;
        var randomNumber = Random.Range(0f, 1f);
        var prefab = randomNumber <= bombChance
            ? bombPrefab
            : ballPrefab;

        var newBall = Instantiate(prefab, transform);

        newBall.transform.position += offset;
        newBall.SetSkin(_currentSkin);
        newBall.SetPause(false);
        newBall.AddImpulse();
        
        _currentBalls.Add(newBall);
    }


    private void GetSkin()
    {
        var currentSkinIndex = SkinManager.Instance.CurrentIndex;

        _currentSkin = skins[currentSkinIndex];
    }
}
