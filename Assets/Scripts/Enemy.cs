using System.Collections;
using Unity.VisualScripting;
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

    bool isBouncing = false;

    bool collisionStarted;

    public float bounceforce = 10;

    public void Start()
    {
        StartCoroutine(EnemyMovementCycle());

        //Detect when a collision with the enemy starts
        StartCoroutine(onCollisionCheck());
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
        //Check if this ship should bounce. If so, start the bounce coroutine
        if (collisionStarted)
        {
            StartCoroutine(bounce());
        }

        //Normalize the movment and find its magnitude this frame
        direction = Vector2.Normalize(direction) * unitsPerSec * Time.deltaTime;

        //Rotate given the last direction of movement
        if (direction.normalized != Vector2.zero)
        {
            transform.up = (Vector3)direction.normalized;
        }

        //Add the movement to the position
        transform.position += (Vector3)direction;

    }

    public IEnumerator EnemyMovementCycle() {

        //Go on forever
        while (true) {

            //Wait until you are not bouncing
            while (isBouncing) {
                yield return null;
            }

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

    public IEnumerator bounce()
    {
        //---DOT PRODUCT

        //Find the direction to the enemy
        Vector3 directionToTarget = (target.position - transform.position).normalized;

        //Find out if the enemy is behind you or not
        bool otherShipIsInFront = Vector3.Dot(directionToTarget, transform.up) >= 0;

        //---COMPUTE THE BOUNCE FORCE

        //Declare local variables
        float forceSpeed;
        Vector2 forceDirection;

        //Set the speed of the force. Bounce farther if the ship is behind you
        if (!otherShipIsInFront)
        {

            //Set the force to max speed
            forceSpeed = bounceforce; ;

        }
        else
        {

            //Set the force to 1/2 max speed
            forceSpeed = bounceforce / 2;

        }

        //---CALCULATE DIRECTION OF THE FORCE

        //Find the direction from the enemy to use as the normal
        Vector3 N = (transform.position - target.position).normalized;
        //Get the ship's current direction
        Vector3 D;
        if (direction.magnitude != 0)
        {
            D = direction.normalized;
        }
        else
        {
            //If the ship is stationary, assume it was going straight into the enemy so that it will be pushed away
            D = -target.up;
        }

        //Set the direction of the force
        forceDirection = D - 2 * (Vector3.Dot(N, D)) * N;

        //---CALCULATE MOTION AND DECELERATION FOR THE BOUNCE FORCE
        float deceleration = forceSpeed / decelerationTime;

        while (forceSpeed != 0)
        {

            //Decelerate
            if (forceSpeed > 0.1f)
            {
                forceSpeed -= deceleration * Time.deltaTime;
            }
            else
            {
                forceSpeed = 0;
            }

            //Normalize the movment and find its magnitude this frame
            forceDirection = Vector2.Normalize(forceDirection) * forceSpeed * Time.deltaTime;

            //Move the ship
            transform.position += (Vector3)forceDirection;

            //Wait until next frame
            yield return null;

        }
    }

    IEnumerator onCollisionCheck()
    {
        bool isColliding = false;
        while (true)
        {

            collisionStarted = false;

            if (Vector2.Distance(transform.position, target.position) < 1)
            {
                //This ship is colliding
                if (!isColliding)
                {
                    collisionStarted = true;
                }
                isColliding = true;
            }
            else
            {
                //This ship is not colliding
                isColliding = false;
            }

            yield return null;

        }
    }

}
