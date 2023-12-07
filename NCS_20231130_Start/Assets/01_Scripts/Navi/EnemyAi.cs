using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public Transform target;
    MeshRenderer mesh;
    NavMeshAgent agent;
    float dist;
    Vector3 pos;
    Vector3 tarpos;
    Vector3 OnePos = Vector3.zero;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mesh = GetComponent<MeshRenderer>();
        //Mt = GetComponent<Material>();
    }

    void Update()
    {
        pos = this.transform.position;
        tarpos = target.transform.position;

        dist = Vector3.Distance(tarpos, pos); // ==거리차이
        //(pos - tarpos).sqrMagnitude > 25 //==거리차이의 제곱 루트안씌운거 루트를 안씌운 제곱상태이기떄문에 원하는거리 5*5해서 25인것
        //(pos - tarpos).Magnitude > 25 == 정확한 거리차이

        if (/*dist < 15*/ (pos - tarpos).sqrMagnitude > 25)
        {
            agent.isStopped = false;
            mesh.material.color = Color.red;
            agent.SetDestination(target.position);
        }
        else //if(/*dist < 1 || dist > 15*/ (pos - tarpos).sqrMagnitude < 25)
        {
            mesh.material.color = Color.white;
            //agent.SetDestination(OnePos);
            agent.isStopped = true;
        }

    }
}
