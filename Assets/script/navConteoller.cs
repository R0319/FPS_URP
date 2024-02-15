using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class navConteoller : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    [SerializeField]
    private float moveSpeed;

    private Transform playerTran;
    public void setAup(Transform playerTran)
    {
        this.playerTran = playerTran;

        if (TryGetComponent(out navMeshAgent))
        {
            navMeshAgent.speed = moveSpeed;
        }
    }

    private void Update()
    {
        if (navMeshAgent == null)
        {
            return;
        }

        navMeshAgent.destination = playerTran.position;
    }
}
