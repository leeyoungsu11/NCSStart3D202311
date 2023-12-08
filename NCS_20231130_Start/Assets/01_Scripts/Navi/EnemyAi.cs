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

    [Tooltip("�þ߰�")]
    [Range(0,360)]
    [SerializeField]
    float viewAngle = 0;
    [Tooltip("�þ߰Ÿ�")]
    [SerializeField]
    float viewDistance = 0;

    [Tooltip("Ÿ�� ����")]
    [SerializeField]
    LayerMask targetMask;

    [Tooltip("��ֹ� ����")]
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
        #region �þ߰�
        
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

        #region �ܼ� ����
        ///ȥ�ڵ��ƴٴϴٰ�
        ///���ݰŸ� �ȿ� ������
        ///�����÷��̾���̿� ���� ���θ��� ������ ������ üũ�� �ƹ��͵� ���ٸ� �÷��̾� �i�ư���

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
        //            //�÷��̾�, �� ���̿� ������ ����
        //        }
        //    }
        //}
        #endregion

        #region �׳� �������...
        ////dist = Vector3.Distance(tarpos, pos); // ==�Ÿ�����
        ////(pos - tarpos).sqrMagnitude > 25 //==�Ÿ������� ���� ��Ʈ�Ⱦ���� ��Ʈ�� �Ⱦ��� ���������̱⋚���� ���ϴ°Ÿ� 5*5�ؼ� 25�ΰ�
        ////(pos - tarpos).Magnitude > 25 == ��Ȯ�� �Ÿ�����

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
                if (targetAngle <= viewAngle * 0.5 && Physics.Raycast(transform.position, targetDir, viewDistance, obstacleMask) == false) // ���� ��ġ���� ���̸� ������ ��ֹ��� ������
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
