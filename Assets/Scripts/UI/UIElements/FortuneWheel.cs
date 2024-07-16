using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;


public class FortuneWheel : MonoBehaviour
{
    public enum FortuneWheelRewardType
    {
        None                = 0,
        
        Empty               = 1,
        Coins               = 2,
        Multiplier          = 3,
        Respin              = 4,
    }
    
    
    [Serializable]
    public class FortuneWheelReward
    {
        public float weight;
        public FortuneWheelRewardType type;
        public float amount;
    }


    private const float FullCircleDegrees = 360f;
    
    [SerializeField] private int numberOfFullCircles;
    [SerializeField] private float rotationDuration;
    [SerializeField] private AnimationCurve rotationCurve;
    [SerializeField] private Transform rotatingPart;
    [Space]
    [SerializeField] private List<FortuneWheelReward> rewards;

    private Tween _rotationTween;


    private void OnDestroy()
    {
        _rotationTween?.Kill();
    }


    public void ResetWheel()
    {
        rotatingPart.rotation = Quaternion.identity;
    }


    public void Rotate(float angle, Action callback = null)
    {
        AudioManager.Instance.PlaySound(AudioClipType.SpinStart);

        var rotationAngle = FullCircleDegrees * numberOfFullCircles + angle;
        var targetRotation = new Vector3(0f, 0f, rotationAngle);

        _rotationTween?.Kill();
        _rotationTween = rotatingPart.DORotate(targetRotation, rotationDuration, RotateMode.FastBeyond360)
            .SetEase(rotationCurve)
            .OnComplete(() =>
            {
                AudioManager.Instance.PlaySound(AudioClipType.SpinStop);

                callback?.Invoke();
            });
    }
    
    
    public FortuneWheelReward GetRandomReward(out float rotationAngle)
    {
        rotationAngle = 0f;
        float totalWeight = 0f;
        
        foreach (var reward in rewards)
        {
            totalWeight += reward.weight;
        }

        float randomWeight = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        rotationAngle = randomWeight / totalWeight * FullCircleDegrees;
        
        foreach (var reward in rewards)
        {
            currentWeight += reward.weight;

            if (randomWeight <= currentWeight)
            {
                return reward;
            }
        }
        
        return null;
    }
}
