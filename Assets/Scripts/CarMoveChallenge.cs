using DG.Tweening;
using UnityEngine;

public class CarMoveChallenge : MonoBehaviour
{
    [Header("Punto final del recorrido")]
    public Transform endPoint; 

    [Header("Velocidades y tiempos")]
    public float moveDuration = 2f;          
    public float maxRandomDelay = 10f;       

    private Vector3 startPoint; 

    private void Start()
    {
        startPoint = transform.position;

        InvokeNextMove();
    }

    void InvokeNextMove()
    {
        float delay = Random.Range(0f, maxRandomDelay);
        Invoke(nameof(StartMovement), delay);
    }

    void StartMovement()
    {
        transform.DOMove(endPoint.position, moveDuration)
                 .SetEase(Ease.Linear)
                 .OnComplete(() =>
                 {
                     transform.position = startPoint;

                     InvokeNextMove();
                 });
    }
}