using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Vector2 moveInput;
    private Vector2 rotInput;
    private float velocity;
    private Vector2 lastRotInput;

    [SerializeField] private Vector2 startRot;
    [SerializeField] private float inputStartRotThresh;
    [SerializeField] private PIDController torquePID;
    private Rigidbody2D rb;
    private PlayerStats stats;
    private PlayerVisuals visuals;
    private EffectManager effects;

    [SerializeField] private float accForce;
    [SerializeField] private float dashForce = 2;
    [SerializeField] private float dashCooldown = 0.5f;

    // used fields just for debugging purposes
    private float breathing;
    private int counter;
    private int breathSpeed;
    private float correction;

    private float dashTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
        visuals = GetComponentInChildren<PlayerVisuals>();
        effects = FindObjectOfType<EffectManager>();

        // Set init rotation
        lastRotInput = startRot;
        transform.up = startRot;
    }

    private void Update()
    {
        dashTimer -= Time.deltaTime;
        if(dashTimer <= 0 && visuals)
            visuals.SetMainColor();
    }

    private void FixedUpdate()
    {
        // TODO: move a lot to update
        velocity = rb.velocity.magnitude;
        float div1 = Vector2.Dot(lastRotInput, moveInput);
        float div2 = lastRotInput.magnitude * moveInput.magnitude;
        float cos = div1 / div2;
        float rad = Mathf.Acos(cos);
        //a = rad * Mathf.Rad2Deg;

        //float acc = (a > 120) ? (backVelocity) : accForce;
        float acc = accForce - rad;
        //rb.AddForce(moveInput * acc, ForceMode2D.Impulse);
        rb.AddForce(moveInput * accForce, ForceMode2D.Impulse);

        
        // TODO: instead add torque for physics! OR smooth visually
        RotateToRightStick(); //  TODO: only if joystick fully pressed change look dir?

        //breathSpeed = GetBreathSpeed();
        //counter = (counter + 1) % breathSpeed;

        // TODO: take rotInput directly to shoot bullets, don't wait for physical rotation
    }

    // TODO: refactor out into input script
    private void OnA()
    {
        print("hit a");
    }
    private void OnLeftTrigger()
    {
        if(dashTimer <= 0)
        {
            dashTimer = dashCooldown;
            visuals.SetDashUsedColor();

            // if not using left stick to move, dash to look direction or right stick
            Vector2 dashDir;
            if (moveInput != Vector2.zero)
                dashDir = moveInput;
            else
                dashDir = lastRotInput;

            rb.AddForce(dashDir * dashForce, ForceMode2D.Impulse);


            effects.DoDashPartic(transform.position, dashDir);
        }
    }
    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    private void OnRotate(InputValue value)
    {
        rotInput = value.Get<Vector2>();
    }


    // bot setters
    public void SetMove(Vector2 val)
    {
        moveInput = val;
    }
    public void SetRot(Vector2 val)
    {
        rotInput = val;
    }
    public Vector2 GetLastRot()
    {
        return lastRotInput;
    }

    private void RotateToRightStick()
    {
        // Always rotate to last input dir
        if (rotInput.magnitude > inputStartRotThresh) // 0 is immediety input, but weird cause snaps to L/R/U/D
            lastRotInput = rotInput;

        float desiredRot = Mathf.Atan2(lastRotInput.y, lastRotInput.x) * Mathf.Rad2Deg + 90f; // 90 to convert the transform rot to the input rot
        //desiredRot = addBreathing(desiredRot);
        
        float actualRot = transform.eulerAngles.z; // 0 to 360, but should be 180 to -180 like desiredRot
        actualRot -= 180;

        if (desiredRot > 180)
            desiredRot -= 360;

        // Look how far needs to rotate for ideal pos
        float dist = Mathf.Abs(actualRot - desiredRot);

        // Always rotate around the fastest side
        if (dist > 180)
        {
            dist -= 180;

            // Account for jump from 180 to -180
            if ((desiredRot > 90 && actualRot < -90) || (desiredRot < -90 && actualRot > 90))
                dist -= 180;
        }

        // Always apply rotation so that player looks to last direction that was input
        correction = torquePID.Update(0, dist, Time.deltaTime);
        //correction = addBreathing(correction);
        //correction = Mathf.Clamp(correction, 0, maxRotSpd);
        rb.AddTorque(correction * Mathf.Sign(actualRot - desiredRot));

        //probably outdated:
        Debug.DrawRay(transform.position, rotInput * 10f, Color.blue); //Desired look dir
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(desiredRot, Vector3.forward) * Vector2.up * 5f, Color.red); //Desired look dir (the same as above, but using float instead of vector)
        Debug.DrawRay(transform.position, Quaternion.AngleAxis(correction, Vector3.forward) * Vector2.up * 5f, Color.yellow); //Correction
        //print(correction);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var dmgObj = collision.GetComponent<DamagingObject>();
        if (dmgObj && dmgObj.damageOnContact)
        {
            stats.ReduceHealth(dmgObj.damage);
            Vector2 pushDir = (Vector2)transform.position - collision.ClosestPoint(transform.position); // maybe instead solid collider? so that I can get hit point... but then player  can't really go "into" spikes
            rb.AddForce(pushDir * dmgObj.knockback, ForceMode2D.Impulse);
        }
    }

    private void OnReady(InputValue value)
    {
        Debug.Log("Replay Match!");
        FindObjectOfType<GameStateManager>().Replay();
    }

    private void OnLeave(InputValue value)
    {
        Debug.Log("Return to Start Screen.");
        FindObjectOfType<GameStateManager>().Restart();
    }
}
