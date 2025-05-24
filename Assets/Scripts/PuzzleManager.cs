using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PuzzleManager : MonoBehaviour
{
    [Header("Plataformas en orden correcto")]
    public List<Transform> orderedPlatforms;

    [Header("Objeto a mover cuando se resuelva el puzzle")]
    public GameObject objectToMove;

    [Header("Puntos de movimiento")]
    public Transform endPointAnimationBus;

    [Header("Audios")]
    public AudioClip puzzleSolvedClip;
    public AudioClip busFinalPlatform;
    public AudioClip puzzleFailedClip;
    public AudioSource audioSource;

    private List<Transform> currentSequence = new List<Transform>();
    private Dictionary<Transform, Vector3> originalRotations = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Collider> platformColliders = new Dictionary<Transform, Collider>();
    private bool isChecking = false;

    void Start()
    {
        foreach (Transform platform in orderedPlatforms)
        {
            originalRotations[platform] = platform.eulerAngles;

            BoxCollider box = platform.GetComponent<BoxCollider>();
            if (box != null)
            {
                platformColliders[platform] = box;
            }
            else
            {
                Debug.LogWarning("Falta BoxCollider en plataforma: " + platform.name);
            }
        }

        // Si no se asignó un AudioSource, crear uno por defecto
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    public void RegisterPlatformHit(Transform platform)
    {
        if (isChecking) return;
        if (currentSequence.Contains(platform)) return;

        currentSequence.Add(platform);

        if (platformColliders.ContainsKey(platform))
            platformColliders[platform].enabled = false;

        platform.DORotate(originalRotations[platform] + new Vector3(0, 180, 0), 0.4f)
                .SetEase(Ease.OutSine);

        if (currentSequence.Count == orderedPlatforms.Count)
        {
            isChecking = true;
            Invoke(nameof(CheckSequence), 0.5f);
        }
    }

    void CheckSequence()
    {
        bool isCorrect = true;

        for (int i = 0; i < orderedPlatforms.Count; i++)
        {
            if (currentSequence[i] != orderedPlatforms[i])
            {
                isCorrect = false;
                break;
            }
        }

        if (isCorrect)
        {
            Debug.Log("¡Puzzle resuelto!");

            foreach (var col in platformColliders.Values)
            {
                col.enabled = false;
            }

            // Reproducir sonido de éxito
            if (puzzleSolvedClip != null)
            {
                audioSource.PlayOneShot(puzzleSolvedClip);
            }

            // Mover el objeto y reproducir sonido asociado
            if (objectToMove != null && endPointAnimationBus != null)
            {
                if (busFinalPlatform != null)
                {
                    audioSource.PlayOneShot(busFinalPlatform);
                }

                objectToMove.transform.DOMove(endPointAnimationBus.position, 4.7f).SetEase(Ease.InOutQuad);
            }
        }
        else
        {
            Debug.Log("Orden incorrecto. Reiniciando...");

            // Reproducir sonido de fallo
            if (puzzleFailedClip != null)
            {
                audioSource.PlayOneShot(puzzleFailedClip, 0.3f);
            }

            foreach (Transform platform in orderedPlatforms)
            {
                platform.DORotate(originalRotations[platform], 0.4f)
                        .SetEase(Ease.InOutSine);

                if (platformColliders.ContainsKey(platform))
                    platformColliders[platform].enabled = true;
            }
        }

        currentSequence.Clear();
        isChecking = false;
    }
}
