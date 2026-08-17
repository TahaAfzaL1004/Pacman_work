using UnityEngine;
using UnityEngine.AI;

public enum GhostState
{
    Chase,
    Frightened,
    Eaten
}

public enum GhostPersonality
{
    Blinky,   // Direct chase
    Pinky     // Ambush (ahead of player)
}

public class GhostAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    
    [Header("Visuals")]
    public GameObject normalModel;
    public GameObject vulnerableModel;
    
    [Header("Personality")]
    public GhostPersonality personality = GhostPersonality.Blinky;
    
    [Header("Settings")]
    public float normalSpeed = 3.5f;
    public float frightenedSpeed = 2f;
    public float eatenSpeed = 5f;
    public Transform ghostHouse;
    
    [Header("Timers")]
    public float frightenedDuration = 10f;
    
    private GhostState currentState;
    private float stateTimer;
    
    void Start()
{
    if (agent == null)
        agent = GetComponent<NavMeshAgent>();
    
    player = GameObject.FindGameObjectWithTag("Player")?.transform;
    
    if (player == null)
    {
        Debug.LogError("CRITICAL: Ghost cannot find GameObject with tag 'Player'!");
    }
    else
    {
        Debug.Log("Ghost successfully locked onto Player: " + player.name);
    }
    
    ShowNormalModel();
    EnterState(GhostState.Chase);
}
    
   void Update()
    {
        if (player == null) return;
        
        switch (currentState)
        {
            case GhostState.Chase:
                if (agent.isOnNavMesh)
                {
                    agent.SetDestination(GetChaseTarget());
                    agent.speed = normalSpeed;
                }
                break;
                
            case GhostState.Frightened:
                if (agent.isOnNavMesh)
                {
                    // Only calculate a new escape route if it arrived or has no path
                    // We check !pathPending to ensure we don't interrupt its calculation
                    if (!agent.pathPending && (!agent.hasPath || agent.remainingDistance < 0.5f))
                    {
                        // Calculate a point 10 units away from the player
                        Vector3 fleeDirection = (transform.position - player.position).normalized;
                        Vector3 fleeTarget = transform.position + fleeDirection * 10f;
                        
                        // Force the target to snap to the closest valid NavMesh floor
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(fleeTarget, out hit, 10f, NavMesh.AllAreas))
                        {
                            agent.SetDestination(hit.position);
                        }
                    }
                    agent.speed = frightenedSpeed;
                }
                
                stateTimer -= Time.deltaTime;
                if (stateTimer <= 0) EnterState(GhostState.Chase);
                break;
                
            case GhostState.Eaten:
                if (ghostHouse != null && agent.isOnNavMesh)
                {
                    agent.SetDestination(ghostHouse.position);
                    agent.speed = eatenSpeed;
                    
                    // Strict distance check. No !agent.hasPath shortcuts.
                    if (Vector3.Distance(transform.position, ghostHouse.position) < 1.5f)
                    {
                        EnterState(GhostState.Chase);
                    }
                }
                break;
        }
    }
    
    Vector3 GetChaseTarget()
    {
        switch (personality)
        {
            case GhostPersonality.Blinky:
                // Direct chase - go straight to player
                return player.position;
                
            case GhostPersonality.Pinky:
                // Ambush - go 3 tiles ahead of player
                Vector3 playerDir = player.forward;
                return player.position + playerDir * 3f;
                
            default:
                return player.position;
        }
    }
    
    void EnterState(GhostState newState)
    {
        currentState = newState;
        stateTimer = frightenedDuration;
        
        if (newState == GhostState.Chase || newState == GhostState.Eaten)
            ShowNormalModel();
    }
    
   void ShowNormalModel()
    {
        normalModel.SetActive(true);
        vulnerableModel.SetActive(false);
    }
    
    public void BecomeFrightened()
    {
        if (currentState != GhostState.Eaten)
        {
            currentState = GhostState.Frightened;
            stateTimer = frightenedDuration;
            normalModel.SetActive(false);
            vulnerableModel.SetActive(true);
        }
    }
    
    public void GetEaten()
    {
        EnterState(GhostState.Eaten);
        // Do not disable the models here. EnterState(GhostState.Eaten) 
        // automatically calls ShowNormalModel() so the ghost remains visible on its return trip.
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentState == GhostState.Frightened)
            {
                // Ghost is vulnerable. Player eats ghost.
                GetEaten();
                
                // Use SendMessage to tell the GameManager to add points. 
                // This compiles perfectly even if the GameManager doesn't exist in your scene yet.
                GameObject gameManager = GameObject.Find("GameManager");
                if (gameManager != null)
                {
                    gameManager.SendMessage("AddGhostScore", SendMessageOptions.DontRequireReceiver);
                }
            }
            else if (currentState == GhostState.Chase)
            {
                // Ghost is deadly. Ghost kills Player.
                // Triggers the "Die" method on whatever script your friend attached to Pac-Man.
                other.gameObject.SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}