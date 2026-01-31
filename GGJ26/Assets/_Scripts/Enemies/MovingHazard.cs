using UnityEngine;

public class MovingHazard : Hazard
{
    [Header("Movement")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool loop = false;
    [SerializeField] private float waitTimeAtPoint = 0f;
    
    [Header("Rotation")]
    [SerializeField] private bool rotateWhileMoving = true;
    [SerializeField] private float rotationSpeed = 360f;
    [SerializeField] private Vector3 rotationAxis = Vector3.forward;

    private int currentWaypointIndex = 0;
    private bool movingForward = true;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    private void Update()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f)
            {
                isWaiting = false;
            }
            return;
        }

        MoveTowardsWaypoint();
        
        if (rotateWhileMoving)
        {
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
        }
    }

    private void MoveTowardsWaypoint()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        float distanceThisFrame = moveSpeed * Time.deltaTime;
        
        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetWaypoint.position, 
            distanceThisFrame
        );

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.01f)
        {
            OnWaypointReached();
        }
    }

    private void OnWaypointReached()
    {
        if (waitTimeAtPoint > 0f)
        {
            isWaiting = true;
            waitTimer = waitTimeAtPoint;
        }

        if (movingForward)
        {
            currentWaypointIndex++;
            
            if (currentWaypointIndex >= waypoints.Length)
            {
                if (loop)
                {
                    currentWaypointIndex = 0;
                }
                else
                {
                    currentWaypointIndex = waypoints.Length - 2;
                    movingForward = false;
                }
            }
        }
        else
        {
            currentWaypointIndex--;
            
            if (currentWaypointIndex < 0)
            {
                currentWaypointIndex = 1;
                movingForward = true;
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.yellow;
        
        for (int i = 0; i < waypoints.Length - 1; i++)
        {
            if (waypoints[i] != null && waypoints[i + 1] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }
        }

        if (loop && waypoints[waypoints.Length - 1] != null && waypoints[0] != null)
        {
            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }

        Gizmos.color = Color.red;
        foreach (var waypoint in waypoints)
        {
            if (waypoint != null)
            {
                Gizmos.DrawWireSphere(waypoint.position, 0.3f);
            }
        }
    }
}

