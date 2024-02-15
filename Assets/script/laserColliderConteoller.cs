using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class laserColliderConteoller : MonoBehaviour
{
    [SerializeField]
    private GameObject start;

    [SerializeField] 
    private GameObject end;

    [SerializeField]
    private GameObject scale;

    [SerializeField]
    private CapsuleCollider collider;

    private void Update()
    {
        float dis = Vector3.Distance(start.transform.localPosition, end.transform.localPosition);

        collider.height = dis / scale.transform.localScale.y;
        collider.center = new Vector3(0.0f, collider.height / 2, 0.0f);
        collider.radius = scale.transform.localScale.y / 15;
    }
}
