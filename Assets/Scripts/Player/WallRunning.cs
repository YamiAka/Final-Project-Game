using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunforce;
    public float wallJumpUpForce;
    public float wallJumpSideForce;
    public float wallClimbSpeed;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode upwardsRunKey = KeyCode.LeftShift;
    public KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool upwardsRunning;
    private bool downwardsRunning;
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;
    private bool wallLeft;
    private bool wallRight;

    [Header("ExitWall")]
    private bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    [Header("Gravity")]
    public bool useGravity;
    public float gravityCounterForce;

    [Header("References")]
    public Transform orientation;
    private PlayerController pc;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pc = GetComponent<PlayerController>();
    }

    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    void FixedUpdate()
    {
        if(pc.wallrunning)
            WallRunMovement();
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallCheckDistance, whatIsWall);
    }

    private bool AbovrGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);

    }

    private void StateMachine()
    {
        //Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        upwardsRunning = Input.GetKey(upwardsRunKey);
        downwardsRunning = Input.GetKey(downwardsRunKey);

        //State Wallrunning
        if((wallLeft || wallRight) && verticalInput > 0 && AbovrGround() && !exitingWall)
        {
            //Start Wallruning
            if(!pc.wallrunning)
                StartWallRun();

            //Wallrun Timer
            if(wallRunTimer > 0)
                wallRunTimer -= Time.deltaTime;

            if(wallRunTimer <= 0 && pc.wallrunning)
            {
                exitingWall = true;
                exitWallTimer = exitWallTime;
            }


            //Wall Jump
            if(Input.GetKeyDown(jumpKey)) WallJump();
            
        }

        //Exiting Wall
        else if (exitingWall)
        {
            if (pc.wallrunning)
                StopWallRun();
            
            if(exitWallTimer > 0)
                exitWallTimer -= Time.deltaTime;

            if (exitWallTimer <= 0)
                exitingWall = false;
        }

        //State None
        else
        {
            if (pc.wallrunning)
                StopWallRun();
        }
    }

    private void StartWallRun()
    {
        pc.wallrunning = true;

        wallRunTimer = maxWallRunTime;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
    }

    private void WallRunMovement()
    {
        rb.useGravity = useGravity;
        

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        if((orientation.forward - wallForward).magnitude > (orientation.forward - -wallForward).magnitude)
            wallForward = -wallForward;

        //Forward Force
        rb.AddForce(wallForward * wallRunforce, ForceMode.Force);

        //Up - Down Forces
        if (upwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, wallClimbSpeed, rb.velocity.z);
        if (downwardsRunning)
            rb.velocity = new Vector3(rb.velocity.x, -wallClimbSpeed, rb.velocity.z);

        //Push to Wall
        if (!(wallLeft && horizontalInput > 0) && !(wallRight && horizontalInput < 0))
            rb.AddForce(-wallNormal * 100, ForceMode.Force);

        //Weak Gravity
        if (useGravity)
            rb.AddForce(transform.up * gravityCounterForce, ForceMode.Force);

    }

    private void StopWallRun()
    {
        pc.wallrunning = false;
    }

    private void WallJump()
    {
        //Enter Exitign Wall State
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 wallNormal = wallRight ? rightWallHit.normal : leftWallHit.normal;

        Vector3 forceToApply = transform.up * wallJumpUpForce + wallNormal * wallJumpSideForce;

        //Add Force & Reset Vel
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
