using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    public SelectModePanel selectModePanel;
    public SettingPanel settingPanel;
    //click start game button
    public void OnStartGameClick()
    {
        //show select mode panel
        selectModePanel.Show();
        
    }
    //click the setting button
    public void OnSettingClick()
    {
        //show setting panel
        settingPanel.Show();
    }
    //click the exit button
    public void OnExitClick()
    {
        //exit the game
        Application.Quit();
    }
}
