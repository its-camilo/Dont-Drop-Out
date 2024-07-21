using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Referencias a objetos
    public GameObject cubeClone;
    public GameObject puddleClone;
    public GameObject spawn;
    public GameObject finjuego;
    [SerializeField] private PassLevel passLevel;
    [SerializeField] private Dialogue dialogue;

    // Componentes
    private CharacterController cc;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private Material blopMaterial;
    private float targetPuddleValue = 100f;

    // Parámetros de movimiento
    public float maxSpeed = 12f;
    public float acceleration = 30f;
    public float deceleration = 30f;
    public float speedRotation = 120f;
    public float gravity = 40f;
    public float jumpForce = 20f;

    // Variables de estado
    private bool grounded;
    private bool wasGrounded;
    private bool inButton;
    private float damage = 0f;
    private float currentSpeed = 0f;
    private float bonusSpeed = 1f;
    private float airControlFactor = 0.8f;
    private float jumpGravityReduction = 0.6f;
    private bool isJumping = false;
    private bool canJump = false;
    private bool enableGravity = true;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 movement = Vector3.zero;
    private Vector3 lastMoveDirection = Vector3.zero;
    private Vector3 jumpDirection = Vector3.zero;
    private Vector3 jumpVector = Vector3.zero;

    private Coroutine storedChangeToNormalShapeKeyCoroutine = null;
    private Coroutine storedChangeToPuddleShapeKeyCoroutine = null;
    private bool stopShapeKeyCoroutine = false;
    // Sistema de clones
    public Queue<GameObject> cloneQueue = new Queue<GameObject>();

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        blopMaterial = skinnedMeshRenderer.materials[0];
    }

    private void Start()
    {
        ResetPosition();
        StartCoroutine(ChangeTextureOffsetIndefinitely(new Vector2(0.05f, 0.02f)));
    }

    private void Update()
    {
        CheckGrounded();
        HandleInput();
        ApplyMovement();
        CheckWaterCollision();
        CheckCheckpoint();
    }

    void CheckGrounded()
    {
        LayerMask jumpableMask = LayerMask.GetMask("Jumpable", "checkpoint");
        wasGrounded = grounded;

        grounded = Physics.Raycast(transform.position, Vector3.down, 0.6f, jumpableMask) || 
                   Physics.Raycast(new Vector3(transform.position.x + 0.6f, transform.position.y, transform.position.z), Vector3.down, 0.6f, jumpableMask) ||
                   Physics.Raycast(new Vector3(transform.position.x - 0.6f, transform.position.y, transform.position.z), Vector3.down, 0.6f, jumpableMask) || 
                   Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.6f), Vector3.down, 0.6f, jumpableMask) ||
                   Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.6f), Vector3.down, 0.6f, jumpableMask);

        if (grounded && enableGravity)
        {
            jumpVector.y = 0;
        }


        if (grounded && !wasGrounded)
        {
            enableGravity = true;
            AudioManager.Instance.PlayFall();
            // if (changingShapeKeyCoroutine != null){
            //     StopCoroutine(changingShapeKeyCoroutine);
            //     changingShapeKeyCoroutine = null;
            // }
            if (storedChangeToNormalShapeKeyCoroutine != null)
            {
                StopCoroutine(storedChangeToNormalShapeKeyCoroutine);
                storedChangeToNormalShapeKeyCoroutine = null;
            }
            storedChangeToPuddleShapeKeyCoroutine = StartCoroutine(ChangeToPuddleShapeKey(targetPuddleValue, 0.2f));
        }
        else if (!grounded && wasGrounded)
        {
            // Inicia la coroutine para cambiar la ShapeKey "Puddle" cuando el personaje está cayendo
            storedChangeToNormalShapeKeyCoroutine = StartCoroutine(ChangeToNormalShapeKey(0.5f));
        }
    }

    void HandleInput()
    {
         // Movimiento
        Vector3 newMoveDirection = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newMoveDirection += transform.forward;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newMoveDirection -= transform.forward;
        }

        if (newMoveDirection != Vector3.zero)
        {
            newMoveDirection.Normalize();
            if (grounded)
            {
                lastMoveDirection = newMoveDirection;
                jumpDirection = newMoveDirection;
            }
        }

        // Move torwards the new direction
        if (newMoveDirection.sqrMagnitude > moveDirection.sqrMagnitude) {
            moveDirection = Vector3.Lerp(moveDirection, newMoveDirection, acceleration * Time.deltaTime);
        } else {
            moveDirection = Vector3.Lerp(moveDirection, newMoveDirection, deceleration * Time.deltaTime);
        }

        // Rotación
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(speedRotation * Time.deltaTime * Vector3.up);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(speedRotation * Time.deltaTime * Vector3.down);
        }

        // Salto
        if ((Input.GetKeyDown(KeyCode.Space)))
        {
            // Can jump for 0.2 seconds
            StartCoroutine(SetCanJumpToFalse(0.2f));
        }
        isJumping = !grounded;
        if (canJump && !isJumping) 
        {   
            StartCoroutine(Jump());
            storedChangeToNormalShapeKeyCoroutine = StartCoroutine(ChangeToNormalShapeKey(0.5f));
        }
        // Comprobar si se mantiene presionada la tecla de salto

        // Clonación y respawn
        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            AudioManager.Instance.PlayClone();
            Respawn(2);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.Instance.PlayClone();
            Respawn(3);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.Instance.PlayUnclone();
            Respawn(4);
        }
    }

    void ApplyMovement()
    {
         // Aceleración/desaceleración en función de la 
        if (moveDirection.magnitude > 0)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed,  maxSpeed, acceleration * Time.deltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0, deceleration * Time.deltaTime);
        }
        
        // Calcular el factor de control
        float controlFactor = 1f;
        if (!grounded)
        {
            // Si está en el aire y la dirección de movimiento es diferente a la dirección del salto
            if (Vector3.Dot(moveDirection, jumpDirection) < 0.99f)
            {
                controlFactor = airControlFactor;
            }
        }

        // Movimiento horizontal
        movement = moveDirection * currentSpeed * controlFactor * bonusSpeed * Time.deltaTime;
        // Gravedad
        float currentGravity = gravity;
        if (!grounded && jumpVector.y < 0 && Input.GetKey(KeyCode.Space))
        {
            currentGravity *= jumpGravityReduction;
        }
        jumpVector.y -= currentGravity * Time.deltaTime;
        movement += jumpVector * Time.deltaTime;

        // Aplicar movimiento
        cc.Move(movement);
    }

    private IEnumerator Jump()
    {   
        enableGravity = false;
        canJump = false;
        // wait 0.1 seconds before allowing the next jump
        yield return new WaitForSeconds(0.1f);
        AudioManager.Instance.PlayJumpPlastic();
        jumpVector.y = jumpForce;
        jumpDirection = lastMoveDirection;
        yield return null;

    }

    void CheckWaterCollision()
    {
        LayerMask waterMask = LayerMask.GetMask("Water");
        if (Physics.Raycast(transform.position, Vector3.down, 0.9f, waterMask))
        {
            // add damage
            damage += 1;
            bonusSpeed = 0.5f;
            if (Mathf.RoundToInt(damage) % 10 == 0) AudioManager.Instance.PlayDamage();
            if (damage >= 30) Respawn(1);
        }
        else {
            bonusSpeed = 1f;
            // reduce damage
            if (damage > 0) damage -= 0.2f;
            else damage = 0;
        }
    }

    void CheckCheckpoint()
    {
        LayerMask checkpointMask = LayerMask.GetMask("checkpoint");
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.9f, checkpointMask))
        {
            spawn.transform.SetPositionAndRotation(new Vector3(hit.transform.position.x, hit.transform.position.y + 2f, hit.transform.position.z), hit.transform.rotation);
        }
    }

    void Respawn(int typeRespawn)
    {
        cc.enabled = false;
        switch (typeRespawn)
        {
            case 1:
                ResetPosition();
                break;
            case 2:
                CloneAndRespawn(cubeClone);
                break;
            case 3:
                CloneAndRespawn(puddleClone);
                break;
            case 4:
                ClearClones();
                ResetPosition();
                break;
        }
        cc.enabled = true;
    }

    void CloneAndRespawn(GameObject cloneType)
    {
        if (cloneQueue.Count < 10)
        {
            cloneQueue.Enqueue(Instantiate(cloneType, transform.position, Quaternion.identity));
        }
        else
        {
            Destroy(cloneQueue.Dequeue());
            cloneQueue.Enqueue(Instantiate(cloneType, transform.position, Quaternion.identity));
        }
        ResetPosition();
    }

    void ClearClones()
    {
        while (cloneQueue.Count > 0)
        {
            Destroy(cloneQueue.Dequeue());
        }
    }

    void ResetPosition()
    {
        transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Collided with " + other.gameObject.name);
        if (other.gameObject.CompareTag("Finish"))
        {
            print("Finish");
            passLevel.FinishLevel();
        }

        if (other.gameObject.CompareTag("FinJuego") && SceneManager.GetActiveScene().name == "Level2")
        {
            finjuego.SetActive(true);
        }

        if (SceneManager.GetActiveScene().name == "Level3" && other.gameObject.name == "redButton1")
        {
            inButton = true;
        }
    }

    private IEnumerator SetCanJumpToFalse(float time)
    {
        canJump = true;
        yield return new WaitForSeconds(time);
        canJump = false;
    }

    private IEnumerator ChangeToPuddleShapeKey(float targetValue, float duration)
    {
        float startValue = skinnedMeshRenderer.GetBlendShapeWeight(0); // Suponiendo que el índice de la ShapeKey "Puddle" es 0
        float time = 0f;

        while (time < duration)
        {
            float value = Mathf.Lerp(startValue, targetValue, time / duration);
            skinnedMeshRenderer.SetBlendShapeWeight(0, value); // Suponiendo que el índice de la ShapeKey "Puddle" es 0
            time += Time.deltaTime;
            yield return null;
        }
        //skinnedMeshRenderer.SetBlendShapeWeight(0, targetValue); // Suponiendo que el índice de la ShapeKey "Puddle" es 0
    }
    private IEnumerator ChangeToNormalShapeKey(float duration)
    {
        float startValue = skinnedMeshRenderer.GetBlendShapeWeight(0); // Suponiendo que el índice de la ShapeKey "Puddle" es 0
        float time = 0f;

        while (time < duration)
        {
            float value = Mathf.Lerp(startValue, 0, time / duration);
            skinnedMeshRenderer.SetBlendShapeWeight(0, value); // Suponiendo que el índice de la ShapeKey "Puddle" es 0
            time += Time.deltaTime;
            yield return null;
        }
    }
    private IEnumerator ChangeTextureOffsetIndefinitely(Vector2 changeRate)
    {
        while (true)
        {
            blopMaterial.mainTextureOffset += changeRate * Time.deltaTime;
            yield return null;
        }
    }
}