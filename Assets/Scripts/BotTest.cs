using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotTest : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private Launcher launcher;

    [SerializeField] private float chasingSpeed = 0.75f; //percentage of maxSpeed in PlayerMovement
    [SerializeField] private float wanderSpeed = 0.5f; //percentage of maxSpeed in PlayerMovement
    [SerializeField] private float wanderDeviation = 0.1f;
    [SerializeField] private int wanderChangeMuch = 500;
    [Space]
    [SerializeField] private float lookAgroMaxDist = 10;
    [SerializeField] private float lookAgroFallow = 2.5f; // ray gets smaller to sides so can see less
    [SerializeField] private float lookAgroFoV = 135;
    [SerializeField] private int rays = 10;
    [SerializeField] private float stopChaseDist = 8;
    [Space]
    [SerializeField] private float reactionDelay; // TODO: time to wait until doing some actions
    [SerializeField] private float maxRandAimOffset; 


    // CONCEPT 
    // simple wander, look in a direction too and check if player near in sight (or got shot)
    // then chase player and shoot

    //TODO: add object avoidance via raycast, or just use an A* implementation


    private bool isChasing; //else wandering
    private bool gotHitAndNotInRange;
    private Vector2 moveDir;
    private Vector2 lookDir;
    private Transform playerToChase;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        launcher = GetComponent<Launcher>();

        FindObjectOfType<TeamManager>().addToEmptyOrSmallestTeam(gameObject);
    }

    void Update()
    {
        // Is wandering (enum?)
        if (!isChasing)
        {
            Transform playerSeen = playerInSight();
            if (!playerSeen)
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

                lookDir = moveDir;
                playerMovement.setRot(lookDir);
                playerMovement.setMove(moveDir * wanderSpeed);
            }
            else // instead of wandering chase now
            {
                playerToChase = playerSeen;
                isChasing = true;
            }
        }
        else
        {
            if(playerToChase && !playerToChase.GetComponent<PlayerStats>().enabled)
                isChasing = false;
            
            if (gotHitAndNotInRange && (playerToChase && Vector2.Distance(playerToChase.position, transform.position) < stopChaseDist))
                gotHitAndNotInRange = false;

            if (gotHitAndNotInRange ||(playerToChase && Vector2.Distance(playerToChase.position, transform.position) < stopChaseDist))
            {
                moveDir = playerToChase.position - transform.position;
                moveDir.Normalize();

                launcher.setShooting(true);

                lookDir = playerToChase.position + (Vector3)(Random.insideUnitCircle.normalized * maxRandAimOffset) - transform.position;
                playerMovement.setRot(lookDir);
                playerMovement.setMove(moveDir * chasingSpeed);
            }
            else //stop chasing
            {
                isChasing = false;
            }
        }
    }


    private Transform playerInSight()
    {
        Vector2 pointA = transform.position;

        for(int i = -rays/2; i < rays/2; i++)
        {
            Vector2 rayDir = ExtensionMethods.RotatePointAroundPivot(lookDir, pointA, i * (lookAgroFoV / 2) / (rays / 2));
            float lookDist = lookAgroMaxDist - Mathf.Abs(i) * lookAgroFallow;

            RaycastHit2D[] rayHit = Physics2D.RaycastAll(pointA, rayDir, lookDist);

            Debug.DrawLine(pointA, pointA + rayDir * lookDist, Color.white);

            // if one of the rays is very close to a block, check if player is between, then kill
            for (int j = 0; j < rayHit.Length; j++)
            {
                if (rayHit[j].collider.GetComponent<PlayerStats>() && !rayHit[j].collider.GetComponent<BotTest>())
                    return rayHit[j].transform;
            }
        }

        return null;
    }


    public void gotHit()
    {
        if (!isChasing)
        {
            isChasing = true;

            gotHitAndNotInRange = true; // if hit once only stop chasing when he was once in range

            foreach (PlayerStats p in FindObjectsOfType<PlayerStats>())
                if (!p.GetComponent<BotTest>())
                    playerToChase = p.transform;

            // TODO: find out who hit this bot (currently takes a random player)
        }
    }


}
