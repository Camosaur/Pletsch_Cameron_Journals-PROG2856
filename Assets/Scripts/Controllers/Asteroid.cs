using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    public float moveSpeed;
    public float arrivalDistance;
    public float maxFloatDistance;

    Vector2 direction = Vector2.zero;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(pickNewTargets());
    }

    // Update is called once per frame
    void Update()
    {
        AsteroidMovement();
    }

    public void AsteroidMovement()
    {

        //Normalize the movment and find its magnitude this frame
        direction = Vector2.Normalize(direction) * moveSpeed * Time.deltaTime;

        //Add the movement to the position
        transform.position += (Vector3)direction;
    }

    IEnumerator pickNewTargets()
    {
        //Declare a vector2 representing the destination point
        Vector2 destination = Vector2.zero;

        //Repeat forever
        while (true) {

            //Generate a new direction of travel and save it
            direction = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;

            //Using the direction, get the new desinaton point and save it
            destination = (direction * maxFloatDistance) + (Vector2)transform.position;

            //Wait until you are within arrivalDistance of that point
            while (Vector2.Distance(transform.position, destination) > arrivalDistance) { 
                //If you are not within arrivalDistance of the point, pause and check again next frame.
                yield return null;
            }
            
        }

    }
}
