using DG.Tweening;
using UnityEngine;

public class FloatEffectAnimation : MonoBehaviour
{
    public float floatAmplitude = 0.25f; // Altura del movimiento
    public float floatDuration = 1.5f;   // Tiempo para subir/bajar


    void Start()
    {
        transform.DOLocalMoveY(transform.localPosition.y + floatAmplitude, floatDuration)
           .SetEase(Ease.InOutSine)
           .SetLoops(-1, LoopType.Yoyo);
    }

}
