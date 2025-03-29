using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    public float jumpSpeed;
    public float jumpButtonGracePeriod;

    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    [SerializeField]
    private Transform cameraTransform;

    private void Start()
    {
         
       characterController = GetComponent<CharacterController>();
       originalStepOffset = characterController.stepOffset;
        
    }
    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementDirection = new Vector3(horizontal, 0, vertical);

        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;  //controlar la magnitud del movimiento dependiendo de lo que se mueva el joystick 

        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection; //se mueve acorde a la cámara

        movementDirection.Normalize(); //evitar que se mueva más rápido en diagonal

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            lastGroundedTime = Time.time;
        }


        if (Input.GetButtonDown("Jump"))
        {
            jumpButtonPressedTime = Time.time;
        }

        if (Time.time - lastGroundedTime <= jumpButtonGracePeriod) 
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed =-0.5f;
           
            if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
            {
                ySpeed = jumpSpeed;
                jumpButtonPressedTime = null;
                lastGroundedTime = null;

            }
        } 
        else
        {
            characterController.stepOffset = 0;
        }
        

        Vector3 velocity = movementDirection * magnitude;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero) //se está moviendo
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked; //centrar el cursor en el centro de la pantalla
        } else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
