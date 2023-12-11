using System;
using UniRx;
using UnityEngine;


public class Ball : MonoBehaviour
{
    [SerializeField] private bool isBomb;
    [SerializeField] private float fallSpeed = 5f;
    [SerializeField] private float impulsePower = 300f;
    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer explosionRenderer;

    private Vector2 _savedVelocity;
    private bool _isPause;


    public bool IsBomb => isBomb;


    public ReactiveProperty<float> Altitude { get; } = new ReactiveProperty<float>();


    private void Awake()
    {
        _isPause = false;
        
        SetPriority(false);
    }


    private void FixedUpdate()
    {
        if (_isPause) return;
        
        if (rigidBody.velocity.y < -fallSpeed)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, -fallSpeed);
        }

        Altitude.Value = transform.localPosition.y;
    }


    private void Update()
    {
        if (transform.position.y < -30f)
        {
            Destroy(gameObject);
        }
    }


    public void SetPriority(bool isPrior)
    {
        spriteRenderer.sortingOrder = isPrior
            ? 15
            : 5;
    }


    public void AddImpulse()
    {
        AddForce(Vector2.up * impulsePower);
    }


    public void Explode()
    {
        rigidBody.isKinematic = true;
        rigidBody.velocity = Vector2.zero;

        explosionRenderer.enabled = true;
        spriteRenderer.enabled = false;

        Observable.Timer(TimeSpan.FromSeconds(1f)).Subscribe(_ =>
        {
            Destroy(gameObject);
        }).AddTo(this);
    }


    public void SetSkin(Sprite sprite)
    {
        if (isBomb) return;
        
        spriteRenderer.sprite = sprite;
    }


    public void SetPause(bool isPause)
    {
        if (_isPause == isPause) return;
        
        if (isPause)
        {
            _savedVelocity = rigidBody.velocity;
            rigidBody.velocity = Vector2.zero;
        }
        else
        {
            rigidBody.velocity = _savedVelocity;
            _savedVelocity = Vector2.zero;
        }

        rigidBody.isKinematic = isPause;
        _isPause = isPause;
    }


    private void AddForce(Vector2 force)
    {
        rigidBody.AddForce(force, ForceMode2D.Impulse);
    }
}