using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.UI;

/** This code is from https://github.com/DANCH0U/Unity3D-Target-Lock-System/blob/main/TargetLock.cs 
 *  and has been modified to fit our project 
 **/

public class TargetLock : MonoBehaviour
{ 
    [Header("Objects")]
    [Space]
    [SerializeField] private Camera mainCamera;            // your main camera object.
    [SerializeField] private CinemachineFreeLook cinemachineFreeLook; //cinemachine free lock camera object.
    [SerializeField] private CinemachineTargetGroup targetGroup;
    [Space]
    [Header("UI")]
    [SerializeField] private Image aimIcon;  // ui image of aim icon u can leave it null.
    [Space]
    [Header("Settings")]
    [Space]
    [SerializeField] private string enemyTag; // the enemies tag.
    [SerializeField] private Vector2 targetLockOffset;
    [SerializeField] private float minDistance; // minimum distance to stop rotation if you get close to target
    [SerializeField] private float maxDistance;
    
    public bool isTargeting;
    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction lockonAction;
    private InputAction cameraAction;

    private float initMinRotation;
    private float initMaxRotation;
    private float minRotation = -45f;
    private float maxRotation = 45f;

    
    private float maxAngle;
    private Transform currentTarget;
    private float mouseX;
    private float mouseY;

    void Start()
    {
        maxAngle = 90f; // always 90 to target enemies in front of camera.

        controller  = gameObject.GetComponent<CharacterController>();
        playerInput = gameObject.GetComponent<PlayerInput>();

        // cinemachineFreeLook.m_XAxis.m_InputAxisName = "";
        // cinemachineFreeLook.m_YAxis.m_InputAxisName = "";
        initMinRotation = cinemachineFreeLook.m_XAxis.m_MinValue;
        initMaxRotation = cinemachineFreeLook.m_XAxis.m_MaxValue;

        lockonAction = playerInput.actions["LockOn"];
        cameraAction = playerInput.actions["Camera"];

        isTargeting = false;
    }

    void Update()
    {
        if (!isTargeting)
        {
            Vector2 input = cameraAction.ReadValue<Vector2>();
            mouseX = input.x;
            mouseY = input.y;
        }
        else
        {
            NewInputTarget(currentTarget);
            // check distance to target,
            // if too far set isTargeting to flase;
        }

        if (aimIcon) 
            aimIcon.gameObject.SetActive(isTargeting);

        //Debug.Log("inputX: " + mouseX + " and inputY: " + mouseY);
        // cinemachineFreeLook.m_XAxis.m_InputAxisValue = mouseX;
        // cinemachineFreeLook.m_YAxis.m_InputAxisValue = mouseY;

        if (lockonAction.triggered)
        {
            AssignTarget();
        }
    }

    private void AssignTarget()
    {
        if (isTargeting)
        {
            isTargeting = false;
            targetGroup.RemoveMember(currentTarget);
            Debug.Log("removed: " + currentTarget);
            // cinemachineFreeLook.m_XAxis.m_MinValue = initMinRotation;
            // cinemachineFreeLook.m_XAxis.m_MaxValue = initMaxRotation;
            cinemachineFreeLook.m_YAxis.m_MaxSpeed = 1f;
            cinemachineFreeLook.m_XAxis.m_MaxSpeed = 180f;
            currentTarget = null;
            return;
        }

        GameObject closest = ClosestTarget();
        if (closest)
        {
            isTargeting = true;
            currentTarget = closest.transform;
            targetGroup.AddMember(currentTarget, 0.7f, 1f);
            // cinemachineFreeLook.m_YAxis.m_MaxSpeed = 0f;
            // cinemachineFreeLook.m_XAxis.m_MaxSpeed = 0f;
            Debug.Log("current target: "+ currentTarget);
        }
    }

    private void NewInputTarget(Transform target) // sets new input value.
    {
        if (!currentTarget) return;

        Vector3 viewPos = mainCamera.WorldToViewportPoint(target.position);
        
        if(aimIcon)
            aimIcon.transform.position = mainCamera.WorldToScreenPoint(target.position);

        if ((target.position - transform.position).magnitude < minDistance) return;
        if ((target.position - transform.position).magnitude > maxDistance) {
            AssignTarget(); 
            return;
        }
        // mouseX = (viewPos.x - 0.5f + targetLockOffset.x) * 3f;              // you can change the [ 3f ] value to make it faster or  slower
        // mouseY = (viewPos.y - 0.5f + targetLockOffset.y) * 3f;              // don't use delta time here.
        Debug.Log(controller.transform.eulerAngles.y);
        // cinemachineFreeLook.m_XAxis.m_MinValue = minRotation;
        // cinemachineFreeLook.m_XAxis.m_MaxValue = maxRotation;
    }


    private GameObject ClosestTarget() // this is modified func from unity Docs ( Gets Closest Object with Tag ). 
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(enemyTag);
        GameObject closest = null;
        float distance = maxDistance;
        float currAngle = maxAngle;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.magnitude;
            if (curDistance < distance)
            {
                Debug.Log("In range.");
                Vector3 viewPos = mainCamera.WorldToViewportPoint(go.transform.position);
                Vector2 newPos = new Vector3(viewPos.x - 0.5f, viewPos.y - 0.5f);
                Debug.Log(Vector3.Angle(diff.normalized, mainCamera.transform.forward) < maxAngle);
                if (Vector3.Angle(diff.normalized, mainCamera.transform.forward) < maxAngle)
                {
                    Debug.Log("in View");
                    closest = go;
                    currAngle = Vector3.Angle(diff.normalized, mainCamera.transform.forward.normalized);
                    distance = curDistance;
                }
            }
        }
        Debug.Log(closest);
        return closest;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}