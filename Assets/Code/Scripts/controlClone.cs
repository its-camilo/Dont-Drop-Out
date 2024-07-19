using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlClone : MonoBehaviour
{
    bool grounded;
    bool wasGrounded;
    public GameObject clone;
    void Update()
    {
        LayerMask jumpableMask = LayerMask.GetMask("Jumpable");
        LayerMask waterMask = LayerMask.GetMask("Water");
        wasGrounded = grounded;

        if (Physics.Raycast(transform.position, Vector3.down, 0.9f, jumpableMask))
        {
            grounded = true;
        }
        if (grounded && !wasGrounded)
        {
            AudioManager.Instance.PlayFall();
        }
        if (Physics.Raycast(transform.position, Vector3.down, 0.9f, waterMask))
        {
            AudioManager.Instance.PlayDamage();
            Destroy(clone);
        }
    }
}
