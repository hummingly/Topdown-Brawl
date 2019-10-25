using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotTest : MonoBehaviour
{
    private TeamManager teams;
    private PlayerMovement playerMovement;
    private Launcher launcher;

    [SerializeField] private float chasingSpeed = 0.75f; //percentage of maxSpeed in PlayerMovement
    [SerializeField] private float wanderSpeed = 0.5f; //percentage of maxSpeed in PlayerMovement
    [SerializeField] private float wanderDeviation = 0.1f;
    [SerializeField] private int wanderChangeMuch = 500;
    [Space]
    [SerializeField] private float lookAgroMaxDist = 10;
    [SerializeField] private float lookAgroFalloff = 2.5f; // ray gets smaller to sides so can see less
    [SerializeField] private float lookAgroFoV = 135;
    [SerializeField] private int rays = 10;
    [SerializeField] private float stopChaseDist = 8;
    [Space]
    [SerializeField] private float reactionDelay; // TODO: time to wait until doing some actions
    [SerializeField] private float maxRandAimOffset;

    [Space]

    [SerializeField] private float avoidObstacleStrength = 90;
    [SerializeField] private float obstacleLookMaxDistance = 5;
    [SerializeField] private float obstacleLookFalloff = 2;
    [SerializeField] private float obstaclelookAgroFoV = 135;
    [SerializeField] private int obstacleRays = 5;

    // CONCEPT 
    // simple wander, look in a direction too and check if player near in sight (or got shot)
    // then chase player and shoot

    //TODO: add object avoidance via raycast, or just use an A* implementation


    private bool isChasing; //else wandering
    private bool gotHitAndNotInRange;
    private Vector2 moveDir;
    private Vector2 lookDir;
    private Transform target;
    private List<GameObject> possibleTargets = new List<GameObject>();

    void Awake()
    {
        teams = FindObjectOfType<TeamManager>();
        playerMovement = GetComponent<PlayerMovement>();
        launcher = GetComponent<Launcher>();


        // if hasn't been added bcz testing in dev scene
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "gameplayDEV")//FindObjectOfType<TeamManager>().getTeamOf(gameObject) == -1)
        {
            FindObjectOfType<TeamManager>().addToEmptyOrSmallestTeam(gameObject);
            GetComponentInChildren<PlayerVisuals>().initColor(FindObjectOfType<TeamManager>().getColorOf(gameObject));
        }
    }

    void Start()
    {
        var myTeam = teams.getTeamOf(gameObject);
        var allEntities = FindObjectsOfType<PlayerMovement>();

        foreach (PlayerMovement pm in allEntities)
        {
            if (teams.getTeamOf(pm.gameObject) != myTeam)
                possibleTargets.Add(pm.gameObject);
        }
    }

    void Update()
    {
        var moveAdjust = obstacleAhead();
        var possibleMaxAdjust = (90 * rays) / 2; //not accurate???
        moveAdjust = Mathf.Clamp(moveAdjust, -possibleMaxAdjust, possibleMaxAdjust);
        // inverted, when big then small
        if (moveAdjust > 0)
            moveAdjust = possibleMaxAdjust - moveAdjust;
        if (moveAdjust < 0)
            moveAdjust =-possibleMaxAdjust + moveAdjust;
        moveAdjust = ExtensionMethods.remap(moveAdjust, -possibleMaxAdjust, possibleMaxAdjust, -avoidObstacleStrength, avoidObstacleStrength);
        //print(moveAdjust);




        // Is wandering (enum?)
        if (!isChasing)
        {
            Transform enemySeen = enemyInSight();

            // Wander around
            if (!enemySeen)
            {
                if (Random.Range(0, wanderChangeMuch) == 1)
                {
                    moveDir = Random.insideUnitCircle.normalized;
                }
                else
                {
                    moveDir += Random.insideUnitCircle * wanderDeviation;
                    moveDir.Normalize();
                }

                launcher.setShooting(false);

                moveDir = ExtensionMethods.RotatePointAroundPivot(moveDir, Vector2.zero, moveAdjust);
                lookDir = moveDir;
                playerMovement.setRot(lookDir);
                playerMovement.setMove(moveDir * wanderSpeed);
            }
            else // instead of wandering chase now
            {
                target = enemySeen;
                isChasing = true;
            }
        }
        else // Chase and shoot
        {
            if(target && !target.GetComponent<PlayerMovement>().enabled)
                isChasing = false;
            
            if (gotHitAndNotInRange && (target && Vector2.Distance(target.position, transform.position) < stopChaseDist))
                gotHitAndNotInRange = false;

            if (gotHitAndNotInRange ||(target && Vector2.Distance(target.position, transform.position) < stopChaseDist))
            {
                moveDir = target.position - transform.position;
                moveDir.Normalize();

                launcher.setShooting(true);

                moveDir = ExtensionMethods.RotatePointAroundPivot(moveDir, Vector2.zero, moveAdjust);

                lookDir = target.position + (Vector3)(Random.insideUnitCircle.normalized * maxRandAimOffset) - transform.position;
                playerMovement.setRot(lookDir);
                playerMovement.setMove(moveDir * chasingSpeed);
            }
            else //stop chasing
            {
                isChasing = false;
            }
        }
    }


    private Transform enemyInSight()
    {
        Vector2 pointA = transform.position;

        for(int i = -rays/2; i < rays/2; i++)
        {
            Vector2 rayDir = ExtensionMethods.RotatePointAroundPivot(lookDir, pointA, i * (lookAgroFoV / 2) / (rays / 2));
            float lookDist = lookAgroMaxDist - Mathf.Abs(i) * lookAgroFalloff;

            var bulletLayerIgnored = ~(1 << LayerMask.NameToLayer("Ignore Bullets"));

            RaycastHit2D[] rayHit = Physics2D.RaycastAll(pointA, rayDir, lookDist, bulletLayerIgnored);
            rayHit = rayHit.OrderBy(h => h.distance).ToArray();

            //Debug.DrawLine(pointA, pointA + rayDir * lookDist, Color.red);

            // look at the first ray that doesn't hit myself (so look at the first wall or enemy)
            for (int j = 0; j < rayHit.Length; j++)
            {
                if(rayHit[j].transform != transform)
                {
                    var entityThere = rayHit[j].collider.GetComponent<PlayerMovement>();

                    if (entityThere && possibleTargets.Contains(entityThere.gameObject)) //!rayHit[j].collider.GetComponent<BotTest>())
                        return rayHit[j].transform;

                    break;
                }
            }
        }

        return null;
    }

    private float obstacleAhead()
    {
        float steerDir = 0;

        Vector2 pointA = transform.position;
        
        for (int i = -obstacleRays / 2; i < obstacleRays / 2; i++)
        {
            Vector2 rayDir = ExtensionMethods.RotatePointAroundPivot(lookDir, pointA, i * (obstaclelookAgroFoV / 2) / (obstacleRays / 2));
            float lookDist = obstacleLookMaxDistance - Mathf.Abs(i) * obstacleLookFalloff;

            var bulletLayerIgnored = ~(1 << LayerMask.NameToLayer("Ignore Bullets"));

            RaycastHit2D[] rayHit = Physics2D.RaycastAll(pointA, rayDir, lookDist, bulletLayerIgnored);
            rayHit = rayHit.OrderBy(h => h.distance).ToArray();

            Debug.DrawLine(pointA, pointA + rayDir * lookDist, Color.white);

            // look at the first ray that doesn't hit myself (so look at the first wall or enemy)
            for (int j = 0; j < rayHit.Length; j++)
            {
                if (rayHit[j].transform != transform)
                {
                    steerDir += Vector2.SignedAngle(lookDir, rayHit[j].normal);

                    break;
                }
            }
        }

        return steerDir;
        //return Mathf.Clamp(steerDir, -1, 1); //TODO: more nuisance to how many obstacles in each dir and where to steer to
    }



    public void gotHit(GameObject hitBy)
    {
        if (!isChasing)
        {
            isChasing = true;

            gotHitAndNotInRange = true; // if hit once only stop chasing when he was once in range

            target = hitBy.transform;
        }
    }

    public void stopChasing()
    {
        isChasing = false;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        moveDir = Random.insideUnitCircle.normalized;
    }
}
