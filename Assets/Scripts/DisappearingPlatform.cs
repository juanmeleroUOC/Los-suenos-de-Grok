using UnityEngine;
using DG.Tweening;
using System.Collections;

public class DisappearingPlatform : MonoBehaviour
{
    public GameObject platformVisual;
    public float delayOffset = 0f;

    private Vector3 originalRotation;

    void Start()
    {
        if (platformVisual == null)
        {
            Debug.LogError("No se asign� platformVisual en " + gameObject.name);
            return;
        }

        originalRotation = platformVisual.transform.localEulerAngles;
        StartCoroutine(PlatformCycle());
    }

    IEnumerator PlatformCycle()
    {
        yield return new WaitForSeconds(delayOffset);

        while (true)
        {
            // Mostrar
            platformVisual.SetActive(true);
            yield return new WaitForSeconds(5f); // Tiempo estable antes del tambaleo

            // Tambaleo m�s largo y visible
            platformVisual.transform.DOKill();
            platformVisual.transform.DOLocalRotate(
                new Vector3(originalRotation.x + 5f, originalRotation.y, originalRotation.z),
                0.2f // M�s lento
            )
            .SetEase(Ease.InOutSine)
            .SetLoops(15, LoopType.Yoyo) // M�s repeticiones
            .OnComplete(() => platformVisual.transform.localEulerAngles = originalRotation);

            yield return new WaitForSeconds(0.2f * 6); // Duraci�n total del tambaleo

            // Ocultar
            platformVisual.SetActive(false);
            yield return new WaitForSeconds(5f);
        }
    }
}
