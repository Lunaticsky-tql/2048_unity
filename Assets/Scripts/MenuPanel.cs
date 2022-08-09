using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    //click start game button
    public void OnStartGameClick()
    {
        //load the game scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("02-game");
    }
    //click the setting button
    public void OnSettingClick()
    {
        //todo
    }
    //click the exit button
    public void OnExitClick()
    {
        //exit the game
        Application.Quit();
    }
}
