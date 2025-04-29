using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DecisionsManager : MonoBehaviour
{
    private Dictionary<string, int> takenDecisions = new Dictionary<string, int>();
    public string nextLevel;

    private void Start()
    {
        foreach (var decision in takenDecisions)
        {
            if (PlayerPrefs.HasKey(decision.Key))
            {
                int option = PlayerPrefs.GetInt(decision.Key);
                takenDecisions[decision.Key] = option;
              //  Debug.Log($"Loaded saved decision: {decision.Key} -> Option {option}");
            }
        }
    }

    public void StoreDecision(string decisionID, int option)
    {

        PlayerPrefs.SetInt(decisionID, option);
        PlayerPrefs.Save();  
      //  Debug.Log($"Decision saved: {decisionID} -> Option {option}");
        GetAllDecisions();
        SceneManager.LoadScene(nextLevel);
        Time.timeScale = 1f;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
