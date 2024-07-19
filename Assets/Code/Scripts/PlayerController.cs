using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    public Queue<GameObject> cloneQueue = new Queue<GameObject>();
    public GameObject clone;
    public GameObject spawn;
    public GameObject checkpoint;
    bool grounded;
    float speedMovement = 12f;
    float speedRotation = 110f;
    float gravity = 40f;
    float jumpForce = 20f;
    Vector3 jumpVector = Vector3.zero;
    CharacterController cc;
    bool wasGrounded;
    [SerializeField] private PassLevel passLevel;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        spawn = GameObject.Find("Spawn");
    }

    private void Start()
    {
        transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
    }

    void Update()
    {
        LayerMask jumpableMask = LayerMask.GetMask("Jumpable", "checkpoint");
        LayerMask checkpointMask = LayerMask.GetMask("checkpoint");
        LayerMask waterMask = LayerMask.GetMask("Water");
        wasGrounded = grounded;

        if (Physics.Raycast(transform.position, Vector3.down, 0.9f, waterMask))
        {
            AudioManager.Instance.PlayDamage();
            Respawn(1);
        }

        if (Physics.Raycast(transform.position, Vector3.down, 0.35f, jumpableMask) || 
            Physics.Raycast(new Vector3(transform.position.x + 0.65f, transform.position.y, transform.position.z), Vector3.down, 0.35f, jumpableMask) ||
            Physics.Raycast(new Vector3(transform.position.x - 0.65f, transform.position.y, transform.position.z), Vector3.down, 0.35f, jumpableMask) || 
            Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.65f), Vector3.down, 0.35f, jumpableMask) ||
            Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - 0.65f), Vector3.down, 0.35f, jumpableMask) )
        {
            jumpVector.y = 0;
            grounded = true;
        }
        else
        {
            grounded=false;
        }

        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.9f, checkpointMask))
        {
            spawn.transform.SetPositionAndRotation(new Vector3(hit.transform.position.x, hit.transform.position.y + 2f, hit.transform.position.z), checkpoint.transform.rotation);
        }

        if (grounded && !wasGrounded)
        {
            AudioManager.Instance.PlayFall();
        }

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            AudioManager.Instance.PlayClone();
            Respawn(2);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            AudioManager.Instance.PlayUnclone();
            Respawn(3);
        }

        if (Input.GetKey(KeyCode.W))
        {
            cc.Move(speedMovement * Time.deltaTime * transform.forward);
        }
        if (Input.GetKey(KeyCode.S))
        {
            cc.Move(- speedMovement * Time.deltaTime * transform.forward);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(speedRotation * Time.deltaTime * Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(speedRotation * Time.deltaTime * Vector3.down);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded is true)
        {
            grounded = false;
            jumpVector.y = jumpForce;
            AudioManager.Instance.PlayJumpPlastic();
        }

        jumpVector.y -= gravity * Time.deltaTime;
        cc.Move(jumpVector * Time.deltaTime);
    }

    void Respawn(int typeRespawn)
    {
        switch(typeRespawn)
        {
            case 1:
                cc.enabled = false;
                transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
                cc.enabled = true;
            break;

            case 2:
                cc.enabled = false;
                if (cloneQueue.Count < 10)
                {
                    cloneQueue.Enqueue(Instantiate(clone, transform.position, Quaternion.identity));
                }
                else
                {
                    Destroy(cloneQueue.Dequeue());
                    cloneQueue.Enqueue(Instantiate(clone, transform.position, Quaternion.identity));
                }
                transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
                cc.enabled = true;
            break;

            case 3:
                cc.enabled = false;
                while (cloneQueue.Count > 0)
                {
                    Destroy(cloneQueue.Dequeue());
                }
                transform.SetPositionAndRotation(spawn.transform.position, spawn.transform.rotation);
                cc.enabled = true;
            break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag is "Finish")
        {
            passLevel.FinishLevel();
        }
    }
}
