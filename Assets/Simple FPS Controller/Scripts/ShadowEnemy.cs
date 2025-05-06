using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Renderer), typeof(Collider), typeof(NavMeshAgent))]
public class ShadowEnemy : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
    public Transform player;

    private Renderer rend;
    private Collider col;
    private NavMeshAgent agent;
    private float lastAttackTime;

    void Start()
    {
        rend = GetComponent<Renderer>();
        col = GetComponent<Collider>();
        agent = GetComponent<NavMeshAgent>();

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }
    }

    void Update()
    {
    agent.SetDestination(player.position);
    // Handle visibility
    if (IsInDarkness())
    {
        SetVisible(true);

        // Attack and move only in darkness
        if (player != null)
        {
            TryAttack();
        }
    }
    else
    {
        SetVisible(false);

    }
    }

    bool IsInDarkness()
    {
    Vector3[] directions = new Vector3[]
    {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };

    foreach (Vector3 dir in directions)
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, dir); // start slightly above ground
        if (Physics.Raycast(ray, out RaycastHit hit, 10f))
        {
            if (hit.collider.CompareTag("LightSource"))
            {
                // Detected light source in this direction
                return false;
            }
        }
    }

    return true;
    }

    void SetVisible(bool visible)
    {
        // Toggle visibility by enabling/disabling renderer
        if (rend != null)
            rend.enabled = visible;

        // OPTIONAL: You could also toggle shadows or transparency here
    }

    //void OnTriggerEnter(Collider other)
    //{
     //   if (other.CompareTag("Player"))
       // {
         //   Debug.Log("Player touched by enemy — instant death!");

           // PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            //if (playerHealth != null)
            //{
              //  playerHealth.Kill();
            //}
        //}
    //}

    void TryAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange, playerLayer);
        foreach (Collider hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Debug.Log("Attacking player!");
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                playerHealth.Kill();
                lastAttackTime = Time.time;
                // TODO: Implement health system here
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
