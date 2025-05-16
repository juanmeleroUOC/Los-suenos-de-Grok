using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    [SerializeField]
    private bool hasFirePower = false;
    
    public void GrantFirePower()
    {
        //Gana el poder
        hasFirePower = true;
        //UIManager.instance.ballPowerUp.enabled = true
       UIManager.instance.ballPowerUp.gameObject.SetActive(true);
    }

    public void LoseFirePower()
    {
        //Pierde el poder
        hasFirePower = false;
        UIManager.instance.ballPowerUp.gameObject.SetActive(false);
    }

    public bool CanShoot()
    {
        //Si tengo el poder puedo disparar
        return hasFirePower;
    }
}
