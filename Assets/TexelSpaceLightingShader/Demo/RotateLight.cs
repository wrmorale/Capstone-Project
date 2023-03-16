using UnityEngine;

public class RotateLight : MonoBehaviour
{
    public float rotationSpeed = 5f;

    public void Update()
    {
        var a = this.transform.eulerAngles;
        a.y += this.rotationSpeed * Time.deltaTime;
        this.transform.eulerAngles = a;
    }
}