using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonActivator : MonoBehaviour
{
    public GameObject rejillas;

    private void OnCollisionEnter(Collision activation)
    {
        if (activation.gameObject.CompareTag("ClonePuddle"))
        {
            rejillas.SetActive(false);
        }
    }
}
