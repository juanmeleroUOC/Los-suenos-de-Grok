using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DecisionsManager : MonoBehaviour
{
    private Dictionary<string, int> takenDecisions = new Dictionary<string, int>();
    public string nextLevel;

    public Image fadeImage;

    private void Start()
    {
        foreach (var decision in takenDecisions)
        {
            if (PlayerPrefs.HasKey(decision.Key))
            {
                int option = PlayerPrefs.GetInt(decision.Key);
                takenDecisions[decision.Key] = option;
            }
        }
    }

    public void StoreDecision(string decisionID, int option)
    {

        PlayerPrefs.SetInt(decisionID, option);
        PlayerPrefs.Save();
        StartCoroutine(FadeAndLoadScene());
    }

    private IEnumerator FadeAndLoadScene()
    {
        fadeImage.gameObject.SetActive(true);
      // UIManager.instance.CloseDecisionScreen();

        Color color = fadeImage.color;
        float duration = 0.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float alpha = Mathf.Clamp01(elapsed / duration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, 1f);

        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.LoadScene(nextLevel);
    }

    public int GetDecision(string decisionID)
    {
        return takenDecisions.ContainsKey(decisionID) ? takenDecisions[decisionID] : -1;
    }

    public Dictionary<string, int> GetAllDecisions()
    {
      /* foreach (var pair in takenDecisions)
        {
            Debug.Log($"[GetAllDecisions] --- Decision ID: {pair.Key} -> Option: {pair.Value}");
        } */
        return takenDecisions;
    }
}
