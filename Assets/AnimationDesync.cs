using System.Collections;
using UnityEngine;

public class AnimationDesync : MonoBehaviour
{
    public string animationStateName = "punchat";

    [Tooltip("Retraso en segundos antes de comenzar la animación. Si es 0, empieza de inmediato. Si es negativo, se elige un valor aleatorio entre 0 y 3.")]
    public float delaySeconds = -1f;

    [Tooltip("Factor de velocidad: >1 hace la animación más lenta (p.ej. 2 significa mitad de velocidad), <1 más rápida.")]
    [Min(0.01f)]
    public float speedFactor = 1f;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Ajustar velocidad de la animación según el factor (factor>1 -> más lenta)
        animator.speed = 1f / speedFactor;

        if (delaySeconds == 0f)
        {
            animator.enabled = true;
            return;
        }

        animator.enabled = false;
        float delay = delaySeconds < 0f ? Random.Range(0f, 3f) : delaySeconds;
        StartCoroutine(StartAnimationWithDelay(delay));
    }

    IEnumerator StartAnimationWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        animator.enabled = true;
        animator.Play(animationStateName, 0, 0f);
    }
}
