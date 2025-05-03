using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public string firstLevel;

    public string currentLevel;

    public GameObject continueBtn;

    public void Start()
    {
        if (PlayerPrefs.HasKey("Continue"))
        {
            Debug.Log("HAY CONTINUE");
            EventSystem.current.SetSelectedGameObject(null);
            continueBtn.SetActive(true);
            EventSystem.current.SetSelectedGameObject(continueBtn.gameObject);
        }
    }

    public void NewGame()
    {
        SceneManager.LoadScene(firstLevel);
        PlayerPrefs.SetInt("Continue", 1); 
    }


    public void Continue()
    {

        if (PlayerPrefs.HasKey("Continue"))
        {
           SceneManager.LoadScene(PlayerPrefs.GetInt("Continue"));
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
