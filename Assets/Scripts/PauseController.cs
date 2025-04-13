using UnityEngine;
using UnityEngine.Events;
public class Pause : MonoBehaviour
{

    public UnityEvent GamePaused;
    public UnityEvent GameResumed;

    private bool _isPaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                Time.timeScale = 0;
                GamePaused.Invoke();
            } else
            {
                Time.timeScale = 1;
                GameResumed.Invoke();
            }
        }
    }
}
