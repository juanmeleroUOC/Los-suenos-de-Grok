using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{

    public static CameraController instance;

    public CinemachineBrain cmBrain;

    private void Awake()
    {
        instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
