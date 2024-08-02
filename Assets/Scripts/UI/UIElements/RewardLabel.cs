using TMPro;
using UnityEngine;


public class RewardLabel : MonoBehaviour
{
    [SerializeField] private GameObject coinImage;
    [SerializeField] private GameObject rocketImage;
    [SerializeField] private TextMeshProUGUI valueLabel;


    public void SetReward(FortuneWheel.FortuneWheelReward reward)
    {
        bool isCoins = reward.type == FortuneWheel.FortuneWheelRewardType.Coins;
        
        coinImage.SetActive(isCoins);
        rocketImage.SetActive(!isCoins);

        valueLabel.text = reward.amount.ToString();
    }
    

    public void Show()
    {
        gameObject.SetActive(true);
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
