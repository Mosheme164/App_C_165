using UnityEngine;


public class SettingsScreen : PopupBase
{
    [Space] 
    [SerializeField] private ButtonBase closeButton;
    [SerializeField] private SwitchButton soundButton;
    [SerializeField] private SwitchButton musicButton;
    [SerializeField] private ButtonBase privacyButton;
    [SerializeField] private ButtonBase termsOfUseButton;
    
    
    protected override void SubscribeButtons()
    {
        base.SubscribeButtons();
        
        closeButton.OnClick.AddListener(CloseButton_OnClick);
        soundButton.OnClick.AddListener(SoundButton_OnClick);
        musicButton.OnClick.AddListener(MusicButton_OnClick);
        privacyButton.OnClick.AddListener(PrivacyButton_OnClick);
        termsOfUseButton.OnClick.AddListener(TermsOfUseButton_OnClick);

        musicButton.SetState(AudioManager.Instance.IsMusicEnabled);
        soundButton.SetState(AudioManager.Instance.IsSoundEnabled);
    }


    protected override void UnSubscribeButtons()
    {
        base.UnSubscribeButtons();
        
        closeButton.OnClick.RemoveListener(CloseButton_OnClick);
        soundButton.OnClick.RemoveListener(SoundButton_OnClick);
        musicButton.OnClick.RemoveListener(MusicButton_OnClick);
        privacyButton.OnClick.RemoveListener(PrivacyButton_OnClick);
        termsOfUseButton.OnClick.RemoveListener(TermsOfUseButton_OnClick);
    }


    private void CloseButton_OnClick()
    {
        Hide();
    }
    
    
    private void SoundButton_OnClick()
    {
        soundButton.SwitchState();
        
        AudioManager.Instance.SetSound(soundButton.IsActive);
    }
    
    
    private void MusicButton_OnClick()
    {
        musicButton.SwitchState();
        
        AudioManager.Instance.SetMusic(musicButton.IsActive);
    }


    private void PrivacyButton_OnClick()
    {
        
    }
    
    
    private void TermsOfUseButton_OnClick()
    {
        
    }
}
