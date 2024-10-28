using System.Collections;
using UnityEngine;



public class Coworker : MonoBehaviour
{

    public float speed;
    public LayerMask unwalkableMask;
    public float minTimeTask;
    public Animator animator;

    public Grid aStarGrid;
    public PointManager pointManager;
    private Transform currentPoint;

    Vector2 destination;

    Vector2[] path;
    int targetIndex;
    CoworkerStateMachine stateMachine;

    [SerializeField]
    private CoworkerState state;
    float timer = 0, timeWait;
    bool switchPath = false, canSwitchTask = true, arrived = false;
    bool canPlayAnimation = false, canMove = true;
    string currentTask;

    Rigidbody2D rb;


    private void Start()
    {
        //PathRequestManager.RequestPath(transform.position, destination.position, OnPathForward);

    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stateMachine = GetComponent<CoworkerStateMachine>();
    }

    private void Update()
    {
        state = stateMachine.GetState();
        if (canPlayAnimation)
        {
            canPlayAnimation = false;
            int rand = UnityEngine.Random.Range(0, 10);
            if (rand < 3)
            {
                //print("SippingCoffee");
                animator.Play("Coffee");
            }
            else if (rand < 6)
            {
                //print("CheckingWatch");
                animator.Play("Watch");
            }
        }

        timer += Time.deltaTime;
        if (timer > timeWait)
        {
            //Get a new state
            CoworkerState newState = (CoworkerState)(UnityEngine.Random.Range(0, CoworkerStateMachine.numOfStates));

            timeWait = minTimeTask;
            //If we are allowed to switch, make an attempt
            if (canSwitchTask)
            {
                //Try to update state
                if (stateMachine.TryUpdateState(newState))
                {
                    //If successful, make sure we stay in that state for longer
                    //timeWait += UnityEngine.Random.Range(0, 2);
                }

            }
            timer = 0;
        }

        if (arrived)
        {
            animator.Play("Idle");
        }



        if (switchPath)
        {


            PathRequestManager.RequestPath(transform.position, destination, OnPathForward, aStarGrid);
        }



    }


    IEnumerator WaitToArrive()
    {
        while (true)
        {
            if (arrived)
            {
                canSwitchTask = true;

                //currentPoint.LeavePoint(specificPoint.position);
                break;
            }
            yield return null;
        }
        yield return null;
    }
    public void GoToDestination()
    {
        StopCoroutine(currentTask);

        //Select a random destination
        currentPoint = pointManager.GetRandomPoint();
        //specificPoint = currentPoint.getPoint();

        destination = currentPoint.position;
        currentTask = "WaitToArrive";
        arrived = false;
        switchPath = true;
        canSwitchTask = false;
        StartCoroutine(currentTask);



    }



    IEnumerator Wandering()
    {
        while (true)
        {
            //Vector2 random = new Vector2(UnityEngine.Random.Range((transform.position.x - aStarGrid.gridWorldSize.x) / 2, (aStarGrid.gridWorldSize.x - transform.position.x) / 2), 
            //    UnityEngine.Random.Range((transform.position.y - aStarGrid.gridWorldSize.y) / 2, (aStarGrid.gridWorldSize.y - transform.position.y) /2 ));
            Vector2 random = new Vector2(UnityEngine.Random.Range(-15, 15), UnityEngine.Random.Range(-15, 15));
            destination = (Vector2)transform.position + random;
            switchPath = true;

            yield return new WaitForSeconds(2.0f);
        }
    }
    public void Wander()
    {
        StopCoroutine(currentTask);
        currentTask = "Wandering";
        canSwitchTask = true;
        StartCoroutine(currentTask);
    }

    IEnumerator Talking()
    {
        while (true)
        {
            yield return null;
        }
    }

    public void StartTalking()
    {
        StopCoroutine(currentTask);
        currentTask = "Talking";
        arrived = true;
        canSwitchTask = true;
        StartCoroutine(currentTask);
    }

    IEnumerator Working()
    {
        while (true)
        {
            if (arrived)
            {
                print("Arrived!");
                //yield return new WaitForSeconds(minTimeTask);
                break;
            }
            yield return null;
        }
        yield return null;
    }
    public void StartWorking()
    {
        StopCoroutine(currentTask);
        currentTask = "Working";

        StartCoroutine(currentTask);
    }

    IEnumerator Idling()
    {
        ////change this to occasionally play different animations like sipping coffee and checking watch
        //while (true)
        //{
        //    canPlayAnimation = true;
        //    if (canSwitchTask)
        //    {
        //        canSwitchTask = false;
        //        yield return new WaitForSeconds(2.0f);
        //        break;
        //    }


        //}
        //canSwitchTask = true;

        yield return new WaitForSeconds(2f);
        canSwitchTask = true;
        canPlayAnimation = false;
    }

    public void Idle()
    {
        StopCoroutine(currentTask);

        currentTask = "Idling";
        arrived = false;
        canSwitchTask = true;
        canMove = false;
        canPlayAnimation = true;
        StartCoroutine(currentTask);
    }


    public void OnPathForward(Vector2[] newPath, bool pathSuccess)
    {
        if (pathSuccess)
        {
            arrived = false;
            path = newPath;
            animator.Play("Walk");
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");

        }
    }

    //IEnumerator ChangeTask()
    //{

    //}

    IEnumerator FollowPath()
    {
        Vector2 currentWayPoint = path[0];



        while (true)
        {

            Vector2 direction = currentWayPoint - (Vector2)transform.position;
            if (direction.magnitude < 1)
            {

                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    rb.linearVelocity = Vector2.zero;
                    arrived = true;
                    yield break;
                }


                currentWayPoint = path[targetIndex];
            }
            else
            {
                Vector2 difference = currentWayPoint - (Vector2)transform.position;

                if (Physics2D.Raycast(transform.position, difference, difference.magnitude + 1, unwalkableMask))
                {
                    switchPath = true;
                    targetIndex = 0;
                    yield break;

                }
            }

            //transform.position = Vector2.MoveTowards(transform.position, currentWayPoint, speed);

            //print((currentWayPoint - (Vector2)transform.position));
            //rb.AddForce((currentWayPoint - (Vector2)transform.position) * speed);
            rb.linearVelocity = direction.normalized * speed;
            //Look at waypoint
            Vector3 look = transform.InverseTransformPoint(currentWayPoint);
            float Angle = Mathf.Atan2(look.y, look.x) * Mathf.Rad2Deg - 90;
            transform.Rotate(0, 0, Angle);

            yield return null;

        }
    }


    private void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector2.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
