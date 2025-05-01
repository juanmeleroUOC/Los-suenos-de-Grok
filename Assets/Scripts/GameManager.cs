using System.Collections;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public UnityEvent GamePaused;
    public UnityEvent GameResumed;

    public static GameManager instance;

    private Vector3 respawnPosition;

    public GameObject deathEffect;

    public int levelEndMusic;

    public string levelToLoad;

    public Checkpoint activeCheckpoint;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        respawnPosition = PlayerMovement.instance.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseUnpause();
        }
    }


    public void Respawn()
    {
        StartCoroutine(RespawnCoroutine());
        HealthManager.instance.PlayerKilled();
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

    public void PauseUnpause()
    {
        if (UIManager.instance.pauseScreen.activeInHierarchy)
        {
            UIManager.instance.pauseScreen.SetActive(false);
            Time.timeScale = 1f;

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } else
        { //Estamos en pantalla pausa
            UIManager.instance.pauseScreen.SetActive(true);

            UIManager.instance.CloseOptions();

            Time.timeScale = 0f;

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

    }

   public IEnumerator LevelEndWaiter()
    {
       // AudioManager.instance.PlayMusic(levelEndMusic);
       PlayerMovement.instance.stopMove = true;
       yield return new WaitForSeconds(3f);

       SceneManager.LoadScene(levelToLoad);


    }

}
