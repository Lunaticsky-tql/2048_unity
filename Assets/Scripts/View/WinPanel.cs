using UnityEngine;
using UnityEngine.SceneManagement;

public class WinPanel : View
{
    public void OnRestartButtonClicked()
    {
        GameObject.Find("Canvas/GamePanel").GetComponent<GamePanel>().RestartGame();
        Hide();
        
    }
    public void OnExitButtonClicked()
    {
        SceneManager.LoadSceneAsync(0);
    }
}