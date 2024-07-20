using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlPuddleClone : MonoBehaviour
{
    public float ray_length = 0.5f;
    bool grounded;
    bool wasGrounded;

    private SkinnedMeshRenderer smr;
    private bool isBlendshapeChanging = false;

    void Start()
    {
        smr = GetComponent<SkinnedMeshRenderer>();
    }

    void Update()
    {
        LayerMask jumpableMask = LayerMask.GetMask("Jumpable","button");
        LayerMask waterMask = LayerMask.GetMask("Water");
        wasGrounded = grounded;

        if (Physics.Raycast(transform.position, Vector3.down, ray_length, jumpableMask) ||
            Physics.Raycast(new Vector3(transform.position.x + 1f, transform.position.y, transform.position.z), Vector3.down, ray_length, jumpableMask) ||
            Physics.Raycast(new Vector3(transform.position.x - 1f, transform.position.y, transform.position.z), Vector3.down, ray_length, jumpableMask) ||
            Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z + 1f), Vector3.down, ray_length, jumpableMask) ||
            Physics.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z - 1f), Vector3.down, ray_length, jumpableMask)
            )
        {
            grounded = true;
        }
        if (grounded && !wasGrounded)
        {
            AudioManager.Instance.PlayFall();
            if (!isBlendshapeChanging)
            {
                StartCoroutine(ChangeBlendshapeGradually());
            }
        }
        if (Physics.Raycast(transform.position, Vector3.down, 0.9f, waterMask))
        {
            AudioManager.Instance.PlayDamage();
            // Destroy itself after 1 second
            Destroy(gameObject, 1f);
        }
    }

    IEnumerator ChangeBlendshapeGradually()
    {
        isBlendshapeChanging = true;
        float elapsedTime = 0f;
        float duration = 2f;
        int blendShapeIndex = smr.sharedMesh.GetBlendShapeIndex("Puddle");

        if (blendShapeIndex != -1)
        {
            float startValue = smr.GetBlendShapeWeight(blendShapeIndex);
            float endValue = 100f;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;
                float currentValue = Mathf.Lerp(startValue, endValue, t);
                smr.SetBlendShapeWeight(blendShapeIndex, currentValue);
                yield return null;
            }

            smr.SetBlendShapeWeight(blendShapeIndex, endValue);
        }
        else
        {
            Debug.LogError("Blend shape not found: 'Cube'");
        }
        isBlendshapeChanging = false;
    }
}
