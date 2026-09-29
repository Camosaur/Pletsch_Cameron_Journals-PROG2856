using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class Enemy : MonoBehaviour
{
    public float unitsPerSec = 1;

    public float maxSpeed = 5;

    public float timeTillMaxSpeed = 1;

    public float decelerationTime = 1;

    public Vector2 direction = Vector2.zero;
    public bool isThrusting = false;

    public Transform target;

    public void Start()
    {
        StartCoroutine(EnemyMovementCycle());
    }

    public void Update()
    {
        EnemyMovement();
    }

    public void EnemyMovement()
    {

        //Get the acceleration
        float acceleration = maxSpeed / timeTillMaxSpeed;

        //get the deceleration
        float deceleration = maxSpeed / decelerationTime;

        if (isThrusting)
        {
            //If the enemy should be accelerating, and they are below max velocity
            if (unitsPerSec < maxSpeed)
            {

                unitsPerSec += acceleration * Time.deltaTime;

            }
            else
            {
                unitsPerSec = maxSpeed;
            }
        }
        else
        {
            //If the enemy should be decelerating, and they are above a cutoff velocity
            if (unitsPerSec > 0.1f)
            {
                unitsPerSec -= deceleration * Time.deltaTime;
            }
            else
            {
                unitsPerSec = 0;
            }
        }

        //Move the enemy
        Move();

    }

    private void Move()
    {
        //Normalize the movment and find its magnitude this frame
        direction = Vector2.Normalize(direction) * unitsPerSec * Time.deltaTime;

        //Add the movement to the position
        transform.position += (Vector3)direction;

    }

    public IEnumerator EnemyMovementCycle() {

        //Go on forever
        while (true) {

            //Set isThrusting to true
            isThrusting = true;

            //Calculate direction to the player
            //Set Direction variable to that vector
            direction =  target.position - transform.position;

            //Wait for an interval
            yield return new WaitForSeconds(3);

            //Set isThrusting to false
            isThrusting = false;

            //Wait for an interval
            yield return new WaitForSeconds(1);

        }
    
    }

}
