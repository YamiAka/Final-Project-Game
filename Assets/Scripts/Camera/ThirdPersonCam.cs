using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public Transform combatLookAt;

    public GameObject thirdPersonCam;
    public GameObject combatCam;
    public GameObject topDownCam;

    public CameraStyle currentStyle;

    public enum CameraStyle
    {
        Basic,
        Combat,
        Topdown
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


    void Update()
    {
        //Switch Style
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchCamera(CameraStyle.Basic);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchCamera(CameraStyle.Combat);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchCamera(CameraStyle.Topdown);

        // Rotate Orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Rotate Player Object
        if(currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (inputDir != Vector3.zero)
            {
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
            }
        }
        else if(currentStyle == CameraStyle.Combat)
        {
            Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
            orientation.forward = dirToCombatLookAt.normalized;

            playerObj.forward = dirToCombatLookAt.normalized;
        }
    }

    void SwitchCamera(CameraStyle newStyle)
    {
        combatCam.SetActive(false);
        thirdPersonCam.SetActive(false);
        topDownCam.SetActive(false);

        if(newStyle == CameraStyle.Basic) thirdPersonCam.SetActive(true);
        if(newStyle == CameraStyle.Combat) combatCam.SetActive(true);
        if(newStyle == CameraStyle.Topdown) topDownCam.SetActive(true);

        currentStyle = newStyle;
    }
}
