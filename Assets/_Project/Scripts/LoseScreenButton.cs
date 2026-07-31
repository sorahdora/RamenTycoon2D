using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenButtons : MonoBehaviour
{
    public void RetryGame()
    {
        SceneManager.LoadScene("RamenGame");
    }

    public void ExitGame()
    {
        Debug.Log("Exit Game");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}