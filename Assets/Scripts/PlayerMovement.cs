using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    public float rotationSpeed;

    [SerializeField]
    public float jumpSpeed;

    [SerializeField]
    private float JumpHorizontalSpeed;

    [SerializeField]
    public float jumpButtonGracePeriod;

    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private float doubleJumpMultiplier = 0.7f;


    private CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;
    private float? lastGroundedTime;
    private float? jumpButtonPressedTime;

    private Animator animator;

    private bool isJumping;
    private bool isGrounded;
    private bool isSliding;
    private Vector3 slopeSlideVelocity;

    private float timeInAir = 0f;
    [SerializeField]
    private float minAirTimeForSlide = 0.5f; // medio segundo en el aire antes de permitir slide

    private bool hasDoubleJumped = false;

    public static PlayerMovement instance;


    //para el knockback cuando se hace da�o
    public bool isKnocking;
    public float knockbackLength = .5f;
    private float knockbackCounter;
    public Vector2 knockbackPower;
    private Vector3 knockbackDirection;

    public GameObject[] playerPieces;

    public float bounceForce = 8f;

    public bool stopMove;
    public Vector3 CurrentVelocity { get; private set; }
    private Vector3 lastPosition;



    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        
        animator = GetComponent<Animator>();

       characterController = GetComponent<CharacterController>();
       originalStepOffset = characterController.stepOffset;
        
    }
    // Update is called once per frame
    void Update()
    {
        animator.ResetTrigger("DoubleJump");

        if (!isKnocking && !stopMove)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movementDirection = new Vector3(horizontal, 0, vertical);

            float inputMagnitude = Mathf.Clamp01(movementDirection.magnitude);

            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.JoystickButton10))
                && !isJumping && isGrounded) // solo sprint cuando está en el suelo
            {
                inputMagnitude = 2;
            }
            animator.SetFloat("Input Magnitude", inputMagnitude, 0.05f, Time.deltaTime);

            movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection; //se mueve acorde a la c�mara
            movementDirection.Normalize(); //evitar que se mueva m�s r�pido en diagonal

            ySpeed += Physics.gravity.y * 2.5f * Time.deltaTime;

            SetSlopeSlideVelocity();

            if (slopeSlideVelocity == Vector3.zero)
            {
                isSliding = false;
            }

            if (characterController.isGrounded)
            {
                timeInAir = 0f;
                lastGroundedTime = Time.time;
            }
            else
            {
                timeInAir += Time.deltaTime;
            }


            if (Input.GetButtonDown("Jump"))
            {
                jumpButtonPressedTime = Time.time;
            }

            if (Time.time - lastGroundedTime <= jumpButtonGracePeriod) // Est� en el suelo
            {

                if (slopeSlideVelocity != Vector3.zero) //Se est� deslizando 
                {
                    isSliding = true;
                }

                characterController.stepOffset = originalStepOffset;

                if (!isSliding)
                {
                    ySpeed = -0.5f;
                }

                animator.SetBool("IsGrounded", true);
                animator.SetBool("IsFalling", false);
                animator.ResetTrigger("JumpTrigger");
                isGrounded = true;
                isJumping = false;
                hasDoubleJumped = false;
                //animator.SetBool("IsJumping", false);

                if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod && !isSliding)  //comprobar condiciones para hacer saltar o no
                {
                    ySpeed = jumpSpeed;
                    // animator.SetBool("IsJumping", true);
                    animator.SetTrigger("JumpTrigger");
                    isJumping = true;
                    jumpButtonPressedTime = null;
                    lastGroundedTime = null;

                }
            }
            else
            {
                characterController.stepOffset = 0;
                animator.SetBool("IsGrounded", false);
                isGrounded = false;

                if ((isJumping && ySpeed < 0) || ySpeed < -2) //si est� cayendo por salto o cambio de superficie
                {
                    if (timeInAir >= minAirTimeForSlide || isJumping)
                    {
                        animator.SetBool("IsFalling", true);
                    }
                }


                if (Time.time - jumpButtonPressedTime <= jumpButtonGracePeriod)
                {
                    if (!isJumping) // aún no se ha hecho un salto ( cayendo desde altura)
                    {
                        ySpeed = jumpSpeed;
                        animator.SetTrigger("JumpTrigger");
                        isJumping = true;
                        jumpButtonPressedTime = null;
                    }
                    else if (!hasDoubleJumped) // ya se ha hecho un salto mientras cae ahora doble salto
                    {
                        ySpeed = jumpSpeed * doubleJumpMultiplier;
                        animator.SetTrigger("DoubleJump");
                        hasDoubleJumped = true;
                        jumpButtonPressedTime = null;
                    }
                }
            }





            if (movementDirection != Vector3.zero) //se est� moviendo
            {
                animator.SetBool("IsMoving", true);
                Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed);
            }
            else
            {
                animator.SetBool("IsMoving", false);
            }

            if (!isGrounded && !isSliding)
            {
                Vector3 velocity = movementDirection * inputMagnitude * JumpHorizontalSpeed;
                velocity.y = ySpeed;

                characterController.Move(velocity * Time.deltaTime);
            }

            if (isSliding)
            {
                Vector3 velocity = slopeSlideVelocity;
                velocity.y = ySpeed;

                characterController.Move(velocity * Time.deltaTime);
            }
        }

        if (isKnocking)
        {
            knockbackCounter -= Time.deltaTime;

            if (knockbackDirection != Vector3.zero)
            {
                characterController.Move(knockbackDirection * Time.deltaTime * knockbackPower.x); 
            }
            else
            {
                Vector3 defaultKnockbackDirection = -transform.forward * knockbackPower.x + Vector3.up * knockbackPower.y;
                characterController.Move(defaultKnockbackDirection * Time.deltaTime * knockbackPower.x); 
            }

            // Desactivamos el knockback despu�s de que haya pasado el tiempo
            if (knockbackCounter <= 0)
            {
                isKnocking = false;
            }
        }

        if (stopMove) {
            characterController.Move(Vector3.zero);
            animator.SetFloat("Input Magnitude", 0f);
            animator.SetBool("IsMoving", false);
        }

        CurrentVelocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    private void SetSlopeSlideVelocity()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hitInfo, 5))
        {
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (angle >= characterController.slopeLimit)
            {
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, ySpeed,0), hitInfo.normal);
                return;
            }
        }

        if (isSliding) //Parar gradualmente la animaci�n
        {
            slopeSlideVelocity -= slopeSlideVelocity * Time.deltaTime * 3;

            if(slopeSlideVelocity.magnitude > 1)
            {
                return;
            }
        }
        slopeSlideVelocity = Vector3.zero;
    }

    private void OnAnimatorMove()
    {
        if (isGrounded && !isSliding)
        {
            Vector3 velocity = animator.deltaPosition;
            velocity.y = ySpeed * Time.deltaTime;

            characterController.Move(velocity);
        }
       
    }
    private void ResetAnimatorSpeed()
    {
        animator.SetFloat("JumpSpeedMultiplier", 1f);
    }


    public void Knockback(Vector3 direction)
    {
        // Almacenar la direcci�n de knockback
        knockbackDirection = direction;

        // Resetear el contador de knockback
        isKnocking = true;
        knockbackCounter = knockbackLength;

        Debug.Log("Knocked back in direction: " + knockbackDirection);
    }

    public void Bounce()
    {
        ySpeed = bounceForce;
        isJumping = true;
        isGrounded = false;
        animator.SetTrigger("JumpTrigger");
        animator.SetBool("IsGrounded", false);
        animator.SetBool("IsFalling", false);

    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // No empujar si no hay Rigidbody o si es kinematic
        if (body == null || body.isKinematic)
            return;

        // Solo empujar en horizontal
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
        body.linearVelocity = pushDir * 2.0f; // puedes ajustar la fuerza
    }


}

