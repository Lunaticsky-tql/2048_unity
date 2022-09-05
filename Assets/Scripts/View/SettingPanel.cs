using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : View
{
    public Slider sliderSound;
    public Slider sliderMusic;
    //close button
    public void OnBtnClose()
    {
        Hide();
    } 
    public void OnChangeSound(float soundVolume)
    {
        AudioManager.Instance.OnSoundVolumeChange(soundVolume);
        PlayerPrefs.SetFloat(Const.Sound, soundVolume);
    }
    public void OnChangeMusic(float musicVolume)
    {
        AudioManager.Instance.OnMusicVolumeChange(musicVolume);
        PlayerPrefs.SetFloat(Const.Music, musicVolume);
    }

    public override void Show()
    {
        base.Show();
        sliderSound.value = PlayerPrefs.GetFloat(Const.Sound,0.5f);
        sliderMusic.value = PlayerPrefs.GetFloat(Const.Music,0.5f);
    }
}
