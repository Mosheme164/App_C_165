using UniRx;
using UnityEngine;
using UnityEngine.UI;


public class CardGameScore : MonoBehaviour
{
    [SerializeField] private Text valueLabel;

    private int _currentScore;
    private int _maxScore;
    
    
    public void SubscribeLabel(CardController cardController)
    {
        cardController.CurrentScore.Subscribe(value =>
        {
            _currentScore = value;
            UpdateView();
            
        }).AddTo(this);
        
        cardController.PairsCount.Subscribe(value =>
        {
            _maxScore = value;
            UpdateView();
            
        }).AddTo(this);
    }


    private void UpdateView()
    {
        valueLabel.text = $"{_currentScore}/{_maxScore}";
    }
}