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
        //todo
        PlayerPrefs.SetFloat(Const.Sound, soundVolume);
    }
    public void OnChangeMusic(float musicVolume)
    {
        //todo
        PlayerPrefs.SetFloat(Const.Music, musicVolume);
    }

    public override void Show()
    {
        base.Show();
        sliderSound.value = PlayerPrefs.GetFloat(Const.Sound,0);
        sliderMusic.value = PlayerPrefs.GetFloat(Const.Music,0);
    }
}
