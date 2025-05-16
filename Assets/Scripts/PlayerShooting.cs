using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    Animator anim;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootCooldown = 1f;

    private float lastShootTime;
    private PlayerPowerUp playerPowerUp;
    Coroutine fadeCoroutine;

    void Awake()
    {
        playerPowerUp = GetComponent<PlayerPowerUp>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        //Si apreto el botón Fire1, el personaje tiene el poder y el cooldown ya pasó, puedo disparar
        if (Input.GetButtonDown("Fire1") && playerPowerUp.CanShoot() && Time.time > lastShootTime + shootCooldown)
        {
            anim.SetTrigger("Shoot");
            //Fade In lo que hace es que, poco a poco, sube el peso de la layer que tiene la parte de arriba del cuerpo
            FadeIn();
            //corutina para disparar
            StartCoroutine("Shoot");
      
            lastShootTime = Time.time;
        }
    }

    public IEnumerator Shoot()
    {
        //0.4 segundos para esperar a que la mano este en el momento exacto para spawnear la pelota
        yield return new WaitForSeconds(0.4f);
        var newBullet = Instantiate(projectilePrefab).GetComponent<Bullet>();
        newBullet.transform.position = firePoint.position;
        newBullet.player = this.gameObject;

        //Si jugador está corriendo, lanzamiento más rápido
        if (anim.GetBool("IsMoving"))
        {
            newBullet.SetDirection(transform.forward, 1.7f);
        }
        else
        {
            newBullet.SetDirection(transform.forward, 1f);
        }
        // 0.15 segundos y fade out, para que el peso de la layer vuelva a la normalidad
        yield return new WaitForSeconds(0.15f);
        FadeOut();

    }

    public void FadeIn(float duration = 0.2f)
    {
        StartFade(1f, duration);
    }

    public void FadeOut(float duration = 0.2f)
    {
        StartFade(0f, duration);
    }

    void StartFade(float targetWeight, float duration)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeLayerWeight(2, targetWeight, duration));
    }

    IEnumerator FadeLayerWeight(int layer, float target, float duration)
    {
        float start = anim.GetLayerWeight(layer);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float weight = Mathf.Lerp(start, target, time / duration);
            anim.SetLayerWeight(layer, weight);
            yield return null;
        }

        anim.SetLayerWeight(layer, target);
    }

}
