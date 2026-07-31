using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenButtons : MonoBehaviour
{
    public GameObject volumePanel;
    public Slider volumeSlider;
    public GameObject howToPanel;
    void Start()
    {
        float savedVolume = PlayerPrefs.GetFloat("Volume", 0.5f);

        AudioListener.volume = savedVolume;

        if (volumeSlider != null)
        {
            volumeSlider.value = savedVolume;
        }

        if (volumePanel != null)
        {
            volumePanel.SetActive(false);
        }

        if (howToPanel != null)
        {
            howToPanel.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("RamenGame");
    }
    public void ToggleVolumePanel()
    {
        if (howToPanel != null)
        {
            howToPanel.SetActive(false);
        }

        if (volumePanel != null)
        {
            volumePanel.SetActive(!volumePanel.activeSelf);
        }
    }

    public void ChangeVolume(float volume)
    {
        AudioListener.volume = volume;

        PlayerPrefs.SetFloat("Volume", volume);
        PlayerPrefs.Save();
    }
    public void OpenHowTo()
    {
        if (howToPanel != null)
        {
            howToPanel.SetActive(true);
        }

        if (volumePanel != null)
        {
            volumePanel.SetActive(false);
        }
    }

    public void CloseHowTo()
    {
        if (howToPanel != null)
        {
            howToPanel.SetActive(false);
        }
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}