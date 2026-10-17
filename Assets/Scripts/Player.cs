using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    public Transform target;
    public GameObject bombPrefab;

    public int numberOfTrailBombs;
    public int bombTrailSpacing;

    public float unitsPerSec = 1;

    public float maxSpeed = 5;

    public float timeTillMaxSpeed = 1;

    public float decelerationTime = 1;

    public Vector2 direction = Vector2.zero;

    bool collisionStarted;

    public float bounceforce = 10;


    void Start()
    {
        //Start the coroutine that manages input handling and cooldowns for bomb placement
        //StartCoroutine(verbWithCooldown(3));

        //Move the player with arrow key input
        StartCoroutine(PlayerMovement());

        //Detect when a collision with the enemy starts
        StartCoroutine(onCollisionCheck());

    }

    private void Update()
    {

        

    }

    #region Week2
    //IEnumerator verbWithCooldown(int cooldownTime)
    //{
    //    while (true)
    //    {
    //        //Wait until the player presses B
    //        while (!Keyboard.current.bKey.wasPressedThisFrame)
    //        {
    //            yield return null;
    //        }

    //        //Spawn the bombs
    //        SpawnBombTrail(transform.up, bombTrailSpacing, numberOfTrailBombs);

    //        //Wait for some time- the cooldown!
    //        yield return new WaitForSeconds(cooldownTime);
    //    }
    //}

    public void SpawnBombTrail(Vector3 inOffset, float inBombSpaceing, int inNumberOfBombs) {
        //Initialize the totalOffset
        Vector2 totalOffset = inOffset;

        //Initialize a direction variable by normalizing inOffset
        Vector2 direction = fakeNormalize(inOffset);


        //Create a for loop that iterates NumberOfBombs times. 
        for (int i = 0; i < numberOfTrailBombs; i++)
        {

            //Call SpawnBombAtOffset with totalOffset
            SpawnBombAtOffset(totalOffset);

            //Increase the magnitude of totalOffest by direction * inBombSpaceing
            totalOffset += direction * inBombSpaceing;

        }
    }

    public void SpawnBombAtOffset(Vector3 inOffset)
    {
        /*Description: Instantiate a bomb at “inOffset” away from the player’s position.
         * 
         * Testing: Create a public Vector2 variable for the Player class called bombOffset.
         * In Update, call the SpawnBombAtOffset method and pass in the bombOffset variable when the B key is pressed.
         * */

        Instantiate(bombPrefab, transform.position + inOffset, Quaternion.identity);
    }


    //In class
    Vector2 fakeNormalize(Vector2 v)
    {

        return new Vector2((v.x / Mathf.Abs(v.magnitude)), (v.y / Mathf.Abs(v.magnitude)));
    }

    #endregion

    #region Movement
    public IEnumerator PlayerMovement() {
        while (true)
        {


            //Get the acceleration
            float acceleration = maxSpeed / timeTillMaxSpeed;

            //get the deceleration
            float deceleration = maxSpeed / decelerationTime;

            //Take input
            bool hasInputed = getInput();
            if (hasInputed)
            {
                //If the player should be accelerating, and they are below max velocity
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
                //If the player should be decelerating, and they are above a cutoff velocity
                if (unitsPerSec > 0.1f)
                {
                    unitsPerSec -= deceleration * Time.deltaTime;
                }
                else
                {
                    unitsPerSec = 0;
                }
            }

            //Check if this ship should bounce. If so, start the bounce coroutine
            if (collisionStarted)
            {
                StartCoroutine(bounce());
            }

            //Normalize the movment and find its magnitude this frame
            direction = Vector2.Normalize(direction) * unitsPerSec * Time.deltaTime;

            //Add the movement to the position
            transform.position += (Vector3)direction;

            //Rotate given the last direction of movement
            if (direction.normalized != Vector2.zero)
            {
                transform.up = (Vector3)direction.normalized;
            }

            yield return null;
        }

    }

    private bool getInput() {

        //Make a vector to represent the movement this frame
        Vector2 movement = Vector2.zero;

        //Make a boolean to represent weather the player gave input this frame
        bool hasInputed = false;


        //LEFT
        if (Keyboard.current.leftArrowKey.isPressed)
        {
            //Move the player this frame
            movement += Vector2.left;

            //Change the bool
            hasInputed = true;
        }

        //RIGHT
        if (Keyboard.current.rightArrowKey.isPressed)
        {
            //Move the player this frame
            movement += Vector2.right;

            //Change the bool
            hasInputed = true;
        }

        //UP
        if (Keyboard.current.upArrowKey.isPressed)
        {
            //Move the player this frame
            movement += Vector2.up;

            //Change the bool
            hasInputed = true;
        }

        //DOWN
        if (Keyboard.current.downArrowKey.isPressed)
        {
            //Move the player this frame
            movement += Vector2.down;

            //Change the bool
            hasInputed = true;
        }

        

        //If you have gotten input, overite the direction
        if (hasInputed)
        {
            direction = movement;
        }

        //Indicate whether the player has given input this frame
        return hasInputed;

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
            forceSpeed = bounceforce;

        }
        else {

            
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
        
        while (forceSpeed != 0) {

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

    IEnumerator onCollisionCheck() {
        bool isColliding = false;
        while (true) {

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

    #endregion

    #region commentedOut
    //public void enemyRadar(float radius, int circlePoints) { 

    //    StartCoroutine(enemyRadarRuntime(radius, circlePoints));
    //}

    //IEnumerator enemyRadarRuntime(float radius, int circlePoints) {

    //    //Declare the local variables I will need
    //    Color shieldColor = Color.red;
    //    List<float> angleList = new List<float>();
    //    List<Vector2> pointList = new List<Vector2>();
    //    float angleStep;

    //    //Calc the list of angles
    //    angleStep = 360 / circlePoints;

    //    for (int i = 0; i < circlePoints; i++)
    //    {
    //        angleList.Add(angleStep * i);
    //    }


    //    //Calc the list of points
    //    foreach (float angle in angleList)
    //    {
    //        pointList.Add(new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius);
    //    }

    //    //Start the infinte cycle
    //    while (true)
    //    {
    //        //-Determine the color of the shield
    //        if (Vector2.Distance(enemyTransform.position, transform.position) >= radius) { 
    //            shieldColor = Color.red;
    //        }
    //        else
    //        {
    //            shieldColor = Color.green;
    //        }

    //        //-Draw the lines
    //        for (int index = 1; index < pointList.Count; index++)
    //        {

    //            //Set start position equal to the star before this one
    //            Vector2 startPosition = pointList[index - 1] + (Vector2)transform.position;

    //            //Set end position equal to this star
    //            Vector2 endPosition = pointList[index] + (Vector2)transform.position;

    //            //Draw the line
    //            Debug.DrawLine(startPosition,endPosition, shieldColor);
    //        }

    //        Debug.DrawLine(pointList[0] + (Vector2)transform.position, pointList[pointList.Count - 1] + (Vector2)transform.position, shieldColor);

    //        //-Wait
    //        yield return null;
    //    }

    //}

    //public void SpawnPowerups(float radius, int numberOfPowerups)
    //{
    //    //Declare the local variables I will need
    //    List<float> angleList = new List<float>();
    //    List<Vector2> pointList = new List<Vector2>();
    //    float angleStep;

    //    //Calc the list of angles
    //    angleStep = 360 / circlePoints;

    //    for (int i = 0; i < circlePoints; i++)
    //    {
    //        angleList.Add(angleStep * i);
    //    }


    //    //Calc the list of points
    //    foreach (float angle in angleList)
    //    {
    //        pointList.Add(new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius);
    //    }

    //    //Instantiate a powerUp at each point on the list
    //    foreach (Vector2 point in pointList) {

    //        Instantiate(powerUpPrefab, point + (Vector2)transform.position, Quaternion.identity);

    //    }
    //}
    #endregion

}






