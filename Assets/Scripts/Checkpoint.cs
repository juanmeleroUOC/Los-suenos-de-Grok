using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public GameObject cpOn, cpOff;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameManager.instance.SetSpawnPoint(transform.position);

            Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();
            for (int i = 0; i < allCheckpoints.Length; i++)
            {
                allCheckpoints[i].cpOff.SetActive(true); 
                allCheckpoints[i].cpOn.SetActive(false);
            }

            cpOff.SetActive(false);
            cpOn.SetActive(true);
        }
    }
}
