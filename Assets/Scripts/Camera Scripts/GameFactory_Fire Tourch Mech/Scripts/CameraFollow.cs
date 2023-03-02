using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] Vector3 offset;
    [SerializeField] Vector2 clampAxis = new Vector2(60, 60);
    
    [SerializeField] float follow_smoothing = 5;
    [SerializeField] float rotate_Smoothing = 5;
    [SerializeField] float senstivity = 60;


    float rotX, rotY;
    bool cursorLocked = false;
    Transform cam;

    public bool lockedTarget;

    void Start(){
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        cam = Camera.main.transform;
    }
    void Update()
    {
        
        Vector3 target_P= target.position + offset;
        transform.position = Vector3.Lerp(transform.position, target_P, follow_smoothing * Time.deltaTime);

        
        if(!lockedTarget) CameraTargetRotation(); else LookAtTarget();
        
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(cursorLocked){
                Cursor.visible= true;
                Cursor.lockState = CursorLockMode.None;
            }else{
                Cursor.visible= false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        
    }

    void CameraTargetRotation(){
        Vector2 mouseAxis = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        rotX += (mouseAxis.x * senstivity) * Time.deltaTime;
        rotY -= (mouseAxis.y * senstivity) * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, clampAxis.x, clampAxis.y);

        Quaternion localRotation = Quaternion.Euler(rotY, rotX, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, localRotation, Time.deltaTime * rotate_Smoothing);
    }

    void LookAtTarget(){
        transform.rotation = cam.rotation;
        Vector3 r = cam.eulerAngles;
        rotX = r.y;
        rotY = 1.8f;
    }
}
