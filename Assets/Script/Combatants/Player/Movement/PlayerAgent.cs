using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAgent : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;

    public void MoveToPositon(Vector3 positon)
    {
        agent.SetDestination(positon);
    }

    public void ClearPoint()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void SetAgentSpeed(float speed)
    {
        agent.speed = speed;
    }

    public void TeleportPlayer(Transform spawnpoint)
    {
        agent.Warp(spawnpoint.position);
    }
}
