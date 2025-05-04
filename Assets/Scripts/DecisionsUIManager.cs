using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DecisionsUIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text decisionText;
    public Button option1Button;
    public Button option2Button;

    [Header("Logic")]
    public DecisionsManager manager;
    public DecisionData decisionData; 

    private void Start()
    {
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(option1Button.gameObject);

        ShowDecision();
    }

    public void ShowDecision()
    {
        if (decisionData == null)
        {
            Debug.LogWarning("No decision data assigned to DecisionsUIManager.");
            return;
        }

       // manager.GetAllDecisions();
        decisionText.text = decisionData.textoDecision;
        option1Button.GetComponentInChildren<TMP_Text>().text = decisionData.opcion1;
        option2Button.GetComponentInChildren<TMP_Text>().text = decisionData.opcion2;
    }

    public void SelectOption1() => manager.StoreDecision(decisionData.decisionID, 0);
    public void SelectOption2() => manager.StoreDecision(decisionData.decisionID, 1);
}
