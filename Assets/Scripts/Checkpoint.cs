using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public GameObject cpOn, cpOff;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.instance.activeCheckpoint == this)
                return;

            GameManager.instance.SetSpawnPoint(transform.position);

            Checkpoint[] allCheckpoints = Object.FindObjectsByType<Checkpoint>(FindObjectsSortMode.None);
            for (int i = 0; i < allCheckpoints.Length; i++)
            {
                allCheckpoints[i].cpOff.SetActive(true);
                allCheckpoints[i].cpOn.SetActive(false);
            }

            cpOff.SetActive(false);
            cpOn.SetActive(true);

            GameManager.instance.activeCheckpoint = this;
        }
    }
}
