using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public int attackPower = 1;

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision);
        if (collision.gameObject.TryGetComponent(out HpbarController hpController))
        {
            hpController.Damege(attackPower);
        }
    }

}