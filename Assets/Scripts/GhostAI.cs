using UnityEngine;
using UnityEngine.AI;

public enum GhostState
{
    Scatter,
    Chase,
    Frightened,
    Eaten
}

public class GhostAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    
    [Header("Visuals")]
    public GameObject normalModel;      // Drag Blue Ghost model here
    public GameObject vulnerableModel;    // Drag Vulnerable Ghost model here
    public Material normalMaterial;     // Optional: for material swap
    public Material vulnerableMaterial; // Optional: for material swap
    
    [Header("Waypoints")]
    public Transform scatterCorner;
    public Transform ghostHouse;
    
    [Header("Settings")]
    public float normalSpeed = 3.5f;
    public float frightenedSpeed = 2f;
    public float eatenSpeed = 5f;
    
    [Header("Timers")]
    public float scatterDuration = 7f;
    public float chaseDuration = 20f;
    public float frightenedDuration = 10f;
    
    private GhostState currentState;
    private float stateTimer;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Make sure normal model shows, vulnerable hides
        ShowNormalModel();
        
        EnterState(GhostState.Scatter);


        void Start()
{
    if (agent == null)
        agent = GetComponent<NavMeshAgent>();
    
    player = GameObject.FindGameObjectWithTag("Player")?.transform;
    
    Debug.Log("Normal model active: " + normalModel.activeSelf);
    Debug.Log("Vulnerable model active: " + vulnerableModel.activeSelf);
    
    ShowNormalModel();
    
    Debug.Log("After ShowNormalModel - Normal: " + normalModel.activeSelf);
    Debug.Log("After ShowNormalModel - Vulnerable: " + vulnerableModel.activeSelf);
    
    EnterState(GhostState.Scatter);
}
    }
    
    void Update()
    {
        stateTimer -= Time.deltaTime;
        
        switch (currentState)
        {
            case GhostState.Scatter:
                agent.SetDestination(scatterCorner.position);
                if (stateTimer <= 0) EnterState(GhostState.Chase);
                break;
                
            case GhostState.Chase:
                agent.SetDestination(player.position);
                if (stateTimer <= 0) EnterState(GhostState.Scatter);
                break;
                
            case GhostState.Frightened:
                // Run away from player
                Vector3 fleeDirection = transform.position - player.position;
                Vector3 fleeTarget = transform.position + fleeDirection.normalized * 5f;
                agent.SetDestination(fleeTarget);
                if (stateTimer <= 0) EnterState(GhostState.Chase);
                break;
                
            case GhostState.Eaten:
                agent.SetDestination(ghostHouse.position);
                // Eyes only - hide body, show eyes or just move fast
                if (Vector3.Distance(transform.position, ghostHouse.position) < 0.5f)
                {
                    EnterState(GhostState.Chase);
                }
                break;
        }
    }
    
    void EnterState(GhostState newState)
    {
        currentState = newState;
        
        switch (newState)
        {
            case GhostState.Scatter:
                agent.speed = normalSpeed;
                stateTimer = scatterDuration;
                ShowNormalModel();
                break;
                
            case GhostState.Chase:
                agent.speed = normalSpeed;
                stateTimer = chaseDuration;
                ShowNormalModel();
                break;
                
            case GhostState.Frightened:
                agent.speed = frightenedSpeed;
                stateTimer = frightenedDuration;
                ShowVulnerableModel();
                break;
                
            case GhostState.Eaten:
                agent.speed = eatenSpeed;
                // When eaten, ghost turns into "eyes" - hide both models
                // or show a special "eyes only" model
                normalModel.SetActive(false);
                vulnerableModel.SetActive(false);
                break;
        }
    }
    
    void ShowNormalModel()
    {
        normalModel.SetActive(true);
        vulnerableModel.SetActive(false);
    }
    
    void ShowVulnerableModel()
    {
        normalModel.SetActive(false);
        vulnerableModel.SetActive(true);
    }
    
    public void BecomeFrightened()
    {
        if (currentState != GhostState.Eaten)
        {
            EnterState(GhostState.Frightened);
        }
    }
    
    public void GetEaten()
    {
        EnterState(GhostState.Eaten);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentState == GhostState.Frightened)
            {
                GetEaten();
               // GameManager.Instance?.AddScore(200);
            }
            else if (currentState != GhostState.Eaten)
            {
               // other.GetComponent<PacmanController>()?.Die();
            }
        }
    }
}