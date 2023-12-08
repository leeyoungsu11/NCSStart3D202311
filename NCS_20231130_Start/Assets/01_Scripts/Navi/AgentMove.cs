using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class AgentMove : MonoBehaviour
{
    public Camera cam;
    public Transform goal;
    NavMeshAgent agent;

    float x;
    float z;
    Vector3 mVec;

    int Climb = 4;
    int jump = 2;

    Coroutine climbCor = null;
    
    float climbSpeed = 5;
    float jumpSpeed = 15;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }
    // Update is called once per frame
    void Update()
    {
        

        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");

        mVec.x = x;
        mVec.z = z;

        transform.Translate(mVec.normalized * Time.deltaTime * 10, Space.World);
        //transform.Translate(mVec.normalized * Time.deltaTime * 10, Space.Self);

        //transform.position += mVec * 5 * Time.deltaTime;
        
        
        transform.LookAt(transform.position + mVec);


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))//카메라부터 광선
            {
                goal.position = hit.point;//골의 위치를 해당 광선으로 설정
                agent.SetDestination(goal.position);//에이전트 목표지점을 골로 변경
            }
        }

        if(agent.isOnOffMeshLink)
        {
            OffMeshLinkData linkdata = agent.currentOffMeshLinkData;
            if (linkdata.offMeshLink != null && linkdata.offMeshLink.area == Climb)
            {
                climbCor = StartCoroutine(cliCor(linkdata));
            }
        }
    }

    IEnumerator cliCor(OffMeshLinkData linkData)
    {
        agent.isStopped = true;
        agent.updateRotation = false;

        Vector3 start = linkData.startPos;
        Vector3 end = linkData.endPos;
        Vector3 lookPos = end;

        float climbTime = Mathf.Abs(end.y - start.y) / climbSpeed;
        float currentTime = 0;
        float percent = 0;

        while (percent < 1)
        {
            currentTime += Time.deltaTime;
            percent = currentTime / climbTime;
            transform.position = Vector3.Lerp(start, end, percent);
            lookPos.y = transform.position.y;
            transform.LookAt(lookPos);
            yield return null;
        }
        agent.CompleteOffMeshLink();
        agent.isStopped = false;
        agent.updateRotation = true;
        //yield return new WaitForSeconds(3);
    }

    
}
