using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FinalManager : MonoBehaviour
{
    [System.Serializable]
    public class DecisionClips
    {
        public string decisionKey;
        public List<GameObject> clips_0;
        public List<GameObject> clips_1;
    }

    public List<DecisionClips> decisions;
    public float fadeDuration = 1.5f;
    public float displayDuration = 2f;

    [Header("Créditos")]
    public GameObject creditsObject; // Objeto con el texto de créditos
    public float creditsDuration = 5f;

    void Start()
    {
        StartCoroutine(PlayFinalSequence());
    }

    IEnumerator PlayFinalSequence()
    {
        foreach (DecisionClips decision in decisions)
        {
            int value = PlayerPrefs.GetInt(decision.decisionKey, 0);
            List<GameObject> clips = (value == 0) ? decision.clips_0 : decision.clips_1;

            foreach (GameObject obj in clips)
            {
                yield return StartCoroutine(ShowClip(obj));
            }
        }

        // Mostrar créditos
        if (creditsObject != null)
        {
            yield return StartCoroutine(ShowClip(creditsObject, creditsDuration));
        }

        // Cargar el menú principal
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator ShowClip(GameObject obj, float customDisplayDuration = -1f)
    {
        float duration = (customDisplayDuration > 0f) ? customDisplayDuration : displayDuration;

        SetAlpha(obj, 0f);
        obj.SetActive(true);

        // Fade in
        yield return StartCoroutine(FadeObject(obj, 0f, 1f));

        // Esperar
        yield return new WaitForSeconds(duration);

        // Fade out
        yield return StartCoroutine(FadeObject(obj, 1f, 0f));

        obj.SetActive(false);
    }

    IEnumerator FadeObject(GameObject obj, float from, float to)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            float alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            SetAlpha(obj, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetAlpha(obj, to);
    }

    void SetAlpha(GameObject obj, float alpha)
    {
        foreach (var image in obj.GetComponentsInChildren<Image>())
        {
            Color c = image.color;
            c.a = alpha;
            image.color = c;
        }

        foreach (var text in obj.GetComponentsInChildren<Text>())
        {
            Color c = text.color;
            c.a = alpha;
            text.color = c;
        }

        foreach (var tmp in obj.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            Color c = tmp.color;
            c.a = alpha;
            tmp.color = c;
        }

        foreach (var sprite in obj.GetComponentsInChildren<SpriteRenderer>())
        {
            Color c = sprite.color;
            c.a = alpha;
            sprite.color = c;
        }

        foreach (var meshText in obj.GetComponentsInChildren<TextMesh>())
        {
            Color c = meshText.color;
            c.a = alpha;
            meshText.color = c;
        }
    }
}
