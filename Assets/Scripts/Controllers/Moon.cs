using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon : MonoBehaviour
{
    public Transform planetTransform;

    public float angle = 0;
    public float radius = 1;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        OrbitalMotion(radius, speed, planetTransform);
    }

    public void OrbitalMotion(float radius, float speed, Transform target)
    {

        //Change position to based on the angle- relative to the target
        transform.position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius + target.position;

        //Iterate the angle by speed
        angle += speed *360 * Time.deltaTime;
        if (angle >= 360) { 
            angle = 0;
        }
    }
}
