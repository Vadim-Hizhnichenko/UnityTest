using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(LineRenderer))]
public class PlayerController : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator anim;
    private LineRenderer lineRender;


    [SerializeField] private GameObject clickMarker;
    [SerializeField] private Transform visualObjectParent;




    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        lineRender = GetComponent<LineRenderer>();

        lineRender.startWidth = 0.15f;
        lineRender.endWidth = 0.15f;
        lineRender.positionCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ClickToMove();
        }

        if (Vector3.Distance(agent.destination, transform.position) <= agent.stoppingDistance)
        {
            clickMarker.SetActive(false);
            anim.SetBool("isRunning", false);
            clickMarker.transform.SetParent(transform);
        }
        else if( agent.hasPath)
        {
            DrawPath();
        }
    }

    private void ClickToMove()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            SetDestination(hit.point);
        }


    }

    private void SetDestination(Vector3 target)
    {
        anim.SetBool("isRunning", true);
        clickMarker.transform.SetParent(visualObjectParent);
        clickMarker.SetActive(true);
        clickMarker.transform.position = target; 
        agent.SetDestination(target);
    }

    // Draw path player will he destination
    void DrawPath()
    {
        lineRender.positionCount = agent.path.corners.Length;
        lineRender.SetPosition(0, transform.position);

        if (agent.path.corners.Length < 2)
        {
            return;
        }

        for (int i = 1; i < agent.path.corners.Length; i++)
        {
            Vector3 pointPosition = new Vector3(agent.path.corners[i].x , agent.path.corners[i].y, agent.path.corners[i].z);
            lineRender.SetPosition(i, pointPosition);
        }
    }
}
