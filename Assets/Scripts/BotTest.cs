using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BotTest : MonoBehaviour
{
    private TeamManager teams;
    private PlayerMovement playerMovement;
    private Skill skill;

    public enum WanderTendency { MapCenter, EnemyTeamCenter, RandomEnemy, None };
    [SerializeField] private WanderTendency wanderTend = WanderTendency.None;
    [SerializeField] private float wanderTendStrength = 0.5f;
    [Space]
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

    [SerializeField] private Vector2 playerChaseOffsetMinMax = new Vector2(3,6);
    //[SerializeField] private float playerChaseOffsetRnd = 0.5f;
    [SerializeField] private float playerChaseOffsetChangeSpd = 1f;
    [SerializeField] private float maxStrafe = 2f;
    [SerializeField] private float strafeChangeSpd = 1f;


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
    private float currPlayerChaseOffset;
    private float currStrafe;

    void Awake()
    {
        teams = FindObjectOfType<TeamManager>();
        playerMovement = GetComponent<PlayerMovement>();
        skill = GetComponent<DefaultShootSkill>();


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
        currPlayerChaseOffset += playerChaseOffsetChangeSpd * ExtensionMethods.randNegPos();
        currPlayerChaseOffset = Mathf.Clamp(currPlayerChaseOffset, playerChaseOffsetMinMax.x, playerChaseOffsetMinMax.y);
        currStrafe += strafeChangeSpd * ExtensionMethods.randNegPos();
        currStrafe = Mathf.Clamp(currStrafe, -maxStrafe, maxStrafe);


        var moveObstAdjust = obstacleAhead();
        var possibleMaxAdjust = (90 * rays) / 2; //not accurate???
        moveObstAdjust = Mathf.Clamp(moveObstAdjust, -possibleMaxAdjust, possibleMaxAdjust);
        // inverted, when big then small
        if (moveObstAdjust > 0)
            moveObstAdjust = possibleMaxAdjust - moveObstAdjust;
        if (moveObstAdjust < 0)
            moveObstAdjust =-possibleMaxAdjust + moveObstAdjust;
        moveObstAdjust = ExtensionMethods.remap(moveObstAdjust, -possibleMaxAdjust, possibleMaxAdjust, -avoidObstacleStrength, avoidObstacleStrength);



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


                Vector2 wanderGeneralDir = moveDir;
                if (wanderTend == WanderTendency.MapCenter)
                    wanderGeneralDir = (Vector3.zero - transform.position).normalized;
                if (wanderTend == WanderTendency.RandomEnemy)
                {
                    var target = teams.getRandomEnemy(gameObject);
                    if(target)
                        wanderGeneralDir = (target.transform.position - transform.position).normalized;
                }


                // Go to a weigthed value of tendency and random wander
                moveDir = Vector2.Lerp(moveDir, wanderGeneralDir, wanderTendStrength);

                moveDir.Normalize();

                // Obstacle avoidance always overrides all
                moveDir = ExtensionMethods.RotatePointAroundPivot(moveDir, Vector2.zero, moveObstAdjust);

                lookDir = moveDir;
                playerMovement.setRot(lookDir);
                playerMovement.setMove(moveDir * wanderSpeed);

                skill.SetAttacking(false);
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
                // basically when chase try not to move to player but to point that is from player to bot, normalized n distance away... 
                var targetPos = target.position;
                var targetOffset = transform.position - targetPos;
                targetOffset = targetOffset.normalized;
                targetOffset *= currPlayerChaseOffset; //Random.Range(playerChaseOffsetMinMax.x, playerChaseOffsetMinMax.y);

                // strafe left and right from current chase position (if i nrange)
                var dist = Vector2.Distance(targetPos, transform.position);
                if (dist > playerChaseOffsetMinMax.x && dist < playerChaseOffsetMinMax.y)
                {
                    targetOffset += new Vector3(targetOffset.y, -targetOffset.x, 0) * currStrafe; //* Random.Range(-maxStrafe, maxStrafe);
                }


                moveDir = targetPos + targetOffset - transform.position;
                moveDir.Normalize();

                skill.SetAttacking(true);

                moveDir = ExtensionMethods.RotatePointAroundPivot(moveDir, Vector2.zero, moveObstAdjust);

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
