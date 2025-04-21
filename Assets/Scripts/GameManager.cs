using System.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public UnityEvent GamePaused;
    public UnityEvent GameResumed;

    private bool _isPaused;

    public static GameManager instance;

    private Vector3 respawnPosition;

    public GameObject deathEffect;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
      respawnPosition = PlayerMovement.instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //Pausar
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                Time.timeScale = 0;
                GamePaused.Invoke();
            }
            else
            {
                Time.timeScale = 1;
                GameResumed.Invoke();
            }
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked; //centrar el cursor en el centro de la pantalla
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    // corutina para el respawn y gestión de animación del fade 
    public IEnumerator RespawnCoroutine()
    {
        PlayerMovement.instance.gameObject.SetActive(false);

        CameraController.instance.cmBrain.enabled = false;

        UIDeadManager.instance.fadeToBlack = true;

        Instantiate(deathEffect, PlayerMovement.instance.transform.position + new Vector3(0f,1f,0f), PlayerMovement.instance.transform.rotation);

        yield return new WaitForSeconds(1f);

        UIDeadManager.instance.fadeFromBlack = true;


        PlayerMovement.instance.transform.position = respawnPosition;
        CameraController.instance.cmBrain.enabled = true;
        PlayerMovement.instance.gameObject.SetActive(true);

        HealthManager.instance.ResetHealth();
    }

    public void SetSpawnPoint(Vector3 newSpawnPoint)
    {
        respawnPosition = newSpawnPoint;
    }
}
