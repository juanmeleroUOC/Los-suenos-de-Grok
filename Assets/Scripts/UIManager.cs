using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class UIManager : MonoBehaviour

{
    public static UIManager instance;

    public Text healthText;
    public Image healthImage;

    public GameObject pauseScreen, optionsScreen, decisionScreen;


    public Slider musicVolSlider, sfxVolSlider;

    public string mainMenu, levelSelect;

    public Image fadeImage;

    private void Start()
    {
        Debug.Log("STARTED CANVAS");
        if (fadeImage != null)
        {
            StartCoroutine(FadeFromBlack());
        }
    }
    private IEnumerator FadeFromBlack()
    {
        float fadeDuration = 2f;

        fadeImage.gameObject.SetActive(true);
        Color color = fadeImage.color;

      //yield return new WaitForSeconds(.3f);

        float t = 0;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 0f);
        fadeImage.gameObject.SetActive(false);
    }

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
