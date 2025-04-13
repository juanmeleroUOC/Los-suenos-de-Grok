using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public int levelToChange;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //si jugador llega a x sitio entonces cambiar de nivel
        SceneManager.LoadScene(levelToChange);
    }
}
