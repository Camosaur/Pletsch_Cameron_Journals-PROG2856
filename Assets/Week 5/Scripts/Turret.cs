using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering;

public class Turret : MonoBehaviour
{
    public float angularSpeed = 90;

    public Transform target;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        float deltaAngle = ComputeShortestAngle(transform.up, directionToTarget);

        float rotateDirection = Mathf.Sign(deltaAngle);

        float angleStep = angularSpeed * Time.deltaTime;

        if (angleStep < Mathf.Abs(deltaAngle)) {
            //Rotate yourself at angularSpeed
            transform.Rotate(0, 0, angularSpeed * Time.deltaTime * rotateDirection);
        }
        else
        {
            transform.Rotate(0, 0, deltaAngle);
        }

        

        

        float dotProduct = Vector3.Dot(directionToTarget, transform.up);

        if(dotProduct >= 0)
        {
            Debug.DrawLine(transform.position, transform.position + transform.up * 6, Color.rebeccaPurple);
        }


    }

    private float ComputeShortestAngle(Vector3 a, Vector3 b) {

        float angleA = Mathf.Atan2(a.y, a.x) * Mathf.Rad2Deg;
        float angleB = Mathf.Atan2(b.y, b.x) * Mathf.Rad2Deg;

        return Mathf.DeltaAngle(angleA, angleB);
    }
}
