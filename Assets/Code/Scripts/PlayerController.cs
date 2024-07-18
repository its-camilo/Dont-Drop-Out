using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerController : MonoBehaviour
{
    public Queue<GameObject> cloneQueue = new Queue<GameObject>();
    public GameObject clone;
    public GameObject spawn;
    bool grounded;
    float speedMovement = 30f;
    float speedRotation = 110f;
    float gravity = 60f;
    float jumpForce = 20f;
    Vector3 jumpVector = Vector3.zero;
    CharacterController cc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        spawn = GameObject.Find("Spawn");
    }

    private void Start()
    {
        transform.SetPositionAndRotation(spawn.transform.position, new Quaternion(0,1,0,1));
    }

    // Update is called once per frame
    void Update()
    {
        LayerMask mask = LayerMask.GetMask("Jumpable");

        if (Physics.Raycast(transform.position, Vector3.down, 0.9f, mask))
        {
            grounded = true;
        }

        if (Input.GetKeyDown(KeyCode.Q)) 
        {
            Respawn(true);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Respawn(false);
        }

        if (Input.GetKey(KeyCode.W))
        {
            cc.Move(speedMovement * Time.deltaTime * transform.forward);
        }

        if (Input.GetKey(KeyCode.S))
        {
            cc.Move(-speedMovement * Time.deltaTime * transform.forward);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(speedRotation * Time.deltaTime * Vector3.up);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(speedRotation * Time.deltaTime * Vector3.down);
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded == true)
        {
            grounded = false;
            jumpVector.y = jumpForce;
        }

        jumpVector.y -= gravity * Time.deltaTime;
        cc.Move(jumpVector * Time.deltaTime);
    }

    void Respawn(bool cloner)
    {
        if (cloner == false)
        {
            cc.enabled = false;
            while(cloneQueue.Count>0)
            {
                Destroy(cloneQueue.Dequeue());
            }
            transform.SetPositionAndRotation(spawn.transform.position, new Quaternion(0, 1, 0, 1));
            cc.enabled = true;
        }
        if (cloner == true)
        {
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
            transform.SetPositionAndRotation(spawn.transform.position, new Quaternion(0, 1, 0, 1));
            cc.enabled = true;
        }

    }
}