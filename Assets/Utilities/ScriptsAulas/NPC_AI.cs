using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class NPC_AI : MonoBehaviour
{
    [Header("Componentes")]
    public List<Transform> wayPoints;
    private UnityEngine.AI.NavMeshAgent navMeshAgent;
    private Animator anim;
    public LayerMask playerLayer;
    public GameObject hitbox;


    [Header("Variables")]
    public int currentWaypointIndex = 0;
    public float speed = 2f;
    private bool isPlayerDetected = false;
    private bool onRadious;


    void Start()
    {
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
        navMeshAgent.speed = speed;

    }


    void Update()
    {
        if (!isPlayerDetected)
        {
            Walking();
        }
        else
        {
            StopWalking();
            anim.SetTrigger("Attack");
        }
    }

    private void Walking()
    {
        if (wayPoints.Count == 0)
        {
            return;
        }
        //Definir a distancia que o gigante pare no wayponit
        float distanceToWaypoint = Vector3.Distance(
            wayPoints[currentWaypointIndex].position,
            transform.position);

        if (distanceToWaypoint <= 2)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Count;
        }
        navMeshAgent.SetDestination(wayPoints[currentWaypointIndex].position);
        anim.SetBool("Move", true);
        onRadious = false;
    }

    private void StopWalking()
    {
        navMeshAgent.isStopped = true;
        anim.SetBool("Move", false);
        onRadious = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Player Detectado");
        isPlayerDetected = true;
        hitbox.SetActive(true);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            Debug.Log("Player saiu do raio de interacao");
        isPlayerDetected = false;
        navMeshAgent.isStopped = false;
        hitbox.SetActive(false);
    }


}
