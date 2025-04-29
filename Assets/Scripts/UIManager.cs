using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour

{
    public static UIManager instance;

    public Text healthText;
    public Image healthImage;

    public GameObject pauseScreen, optionsScreen, decisionScreen;


    public Slider musicVolSlider, sfxVolSlider;

    public string mainMenu, levelSelect;

    private void Awake()
    {
        instance = this;
    }
   
    public void Resume()
    {
        GameManager.instance.PauseUnpause();
    }

    public void OpenOptions()
    {
        optionsScreen.SetActive(true);
    }

    public void CloseOptions() 
    { 
        optionsScreen.SetActive(false);

    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);
    }

    public void ExitMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }

    public void SetMusicLevel()
    {
        AudioManager.instance.SetMusicLevel();
    }

    public void SetSFXLevel()
    {
        AudioManager.instance.SetSFXLevel();
    }


    public void OpenDecisionScreen()
    {
        decisionScreen.SetActive(true);
    }

    public void CloseDecisionScreen()
    {
        decisionScreen.SetActive(false);

    }
}
