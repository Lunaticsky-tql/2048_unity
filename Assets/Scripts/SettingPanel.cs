using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    //close button
    public void OnBtnClose()
    {
        gameObject.SetActive(false);
    } 
    public void OnChangeSound(float soundVolume)
    {
        //todo
    }
    public void OnChangeMusic(float musicVolume)
    {
        //todo
    }

}
