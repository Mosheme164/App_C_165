using UnityEngine;
using UnityEngine.UI;


public class ProgressBarBase : MonoBehaviour
{
    [SerializeField] private Image fill;

    private float _progress;


    public void SetProgress(float value)
    {
        _progress = Mathf.Clamp01(value);

        fill.fillAmount = _progress;
    }
}
