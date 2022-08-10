using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectModePanel : View
{
    public void OnSelectModePanel(int gameSize)
    {
        PlayerPrefs.SetInt(Const.GameModel, gameSize);
        //to game scene
        SceneManager.LoadSceneAsync("02-game");
    }
}