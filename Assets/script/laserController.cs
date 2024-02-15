using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class laserController : MonoBehaviour
{
    [SerializeField]
    private int attackPower;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.TryGetComponent(out EnemyHpbarController enemyHP))
        {
            enemyHP.Damage(attackPower);
        }
    }
}
