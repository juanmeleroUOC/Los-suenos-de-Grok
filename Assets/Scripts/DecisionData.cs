using UnityEngine;

[CreateAssetMenu(fileName = "Nueva decision", menuName = "Decision")]
public class DecisionData : ScriptableObject
{
    public string decisionID; 
    public string textoDecision; 
    public string opcion1;
    public string opcion2;
}