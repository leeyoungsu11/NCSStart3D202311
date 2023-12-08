using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public Transform target;
    public Animator animator;
    //public Transform RayTr;

    public List<Transform> MAPos;

    MeshRenderer mesh;
    NavMeshAgent agent;
    //float dist;
    //Vector3 pos;
    Vector3 tarpos;
    Vector3 OnePos = Vector3.zero;

    [Tooltip("시야각")]
    [Range(0,360)]
    [SerializeField]
    float viewAngle = 0;
    [Tooltip("시야거리")]
    [SerializeField]
    float viewDistance = 0;

    [Tooltip("타겟 설정")]
    [SerializeField]
    LayerMask targetMask;

    [Tooltip("장애물 설정")]
    [SerializeField]
    LayerMask obstacleMask;

    Coroutine movCor;
    List<Collider> targetList = new List<Collider>();

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        mesh = GetComponent<MeshRenderer>();
        //animator = GetComponent<Animator>();
        //Mt = GetComponent<Material>();
        StartCoroutine(MoveAi());
    }

    void Update()
    {
        tarpos = this.transform.position - target.transform.position;
        #region 시야각
        
        if (CheckSight())
        {

            agent.SetDestination(target.position);
            animator.SetBool("isTarget", true);
            agent.isStopped = false;
        }
        else
        {
            //agent.isStopped = true;
            //animator.SetBool("isTarget", false);
            //agent.velocity = Vector3.zero;
        }
        #endregion

        #region 단순 감지
        ///혼자돌아다니다가
        ///공격거리 안에 들어오면
        ///나와플레이어사이에 벽이 가로막고 있지는 않은지 체크후 아무것도 없다면 플레이어 쫒아가기

        //if (tarpos.sqrMagnitude <= 25)
        //{
        //    RaycastHit hit;
        //    if(Physics.Raycast(RayTr.position, tarpos,out hit))
        //    {
        //        if(hit.transform.CompareTag("Player"))
        //        {
        //            agent.SetDestination(target.position);
        //            animator.SetBool("isTarget", true);
        //            agent.isStopped = false;
        //        }
        //        else
        //        {
        //            //플레이어, 적 사이에 뭔가가 있음
        //        }
        //    }
        //}
        #endregion

        #region 그냥 따라오는...
        ////dist = Vector3.Distance(tarpos, pos); // ==거리차이
        ////(pos - tarpos).sqrMagnitude > 25 //==거리차이의 제곱 루트안씌운거 루트를 안씌운 제곱상태이기떄문에 원하는거리 5*5해서 25인것
        ////(pos - tarpos).Magnitude > 25 == 정확한 거리차이

        //if (/*dist < 15*/ (tarpos).sqrMagnitude > 25)
        //{
        //    agent.isStopped = false;
        //    //mesh.material.color = Color.red;
        //    agent.SetDestination(target.position);
        //    animator.SetBool("isTarget",true);
        //}
        //else //if(/*dist < 1 || dist > 15*/ (pos - tarpos).sqrMagnitude < 25)
        //{
        //    //mesh.material.color = Color.white;
        //    //agent.SetDestination(OnePos);
        //    agent.isStopped = true;
        //    animator.SetBool("isTarget", false);
        //    agent.velocity = Vector3.zero;
        //}
        #endregion
    }
    bool DrawRay;
    Vector3 rightDir = Vector3.zero;
    Vector3 leftDir = Vector3.zero;

    void OnDrawGizmos()
    {
        if(DrawRay == false)
        {
            return;
        }

        Gizmos.DrawWireSphere(transform.position, viewDistance);
        rightDir = DebugDir(transform.eulerAngles.y + viewAngle*0.5f);
        leftDir = DebugDir(transform.eulerAngles.y - viewAngle*0.5f);
    }
    Vector3 DebugDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }    
    public bool CheckSight()
    {
        targetList.Clear();
        Collider[] cols = Physics.OverlapSphere(transform.position, viewDistance, targetMask);
        if (cols.Length > 0)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                Vector3 targetDir = (cols[i].transform.position - transform.position).normalized;
                float targetAngle = Mathf.Acos(Vector3.Dot(transform.forward, targetDir)) * Mathf.Rad2Deg;
                if (targetAngle <= viewAngle * 0.5 && Physics.Raycast(transform.position, targetDir, viewDistance, obstacleMask) == false) // 나의 위치에서 레이를 쐈을때 장애물이 없을때
                {
                    targetList.Add(cols[i]);
                }
            }
            if (targetList.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }

    IEnumerator MoveAi()
    {
        if(!CheckSight())
        {
            if (MAPos.Count > 0)
            {
                animator.SetBool("isTarget", true);
                agent.isStopped = false;
                int posNum = 0;
                while (true)
                {
                    Debug.Log("M");
                    agent.SetDestination(MAPos[posNum].position);
                    posNum++;
                    if (posNum == MAPos.Count)
                    {
                        posNum = 0;
                    }
                    yield return new WaitForSeconds(2.0f);
                }
            }

        }
        
        
    }
}
