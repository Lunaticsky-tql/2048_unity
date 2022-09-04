using UnityEngine;
using UnityEngine.SceneManagement;

public class LosePanel : View
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