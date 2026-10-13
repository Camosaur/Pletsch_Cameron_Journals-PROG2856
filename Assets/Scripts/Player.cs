using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;

    public int numberOfTrailBombs;
    public int bombTrailSpacing;

    public float unitsPerSec = 1;

    public float maxSpeed = 5;

    public float timeTillMaxSpeed = 1;

    public float decelerationTime = 1;

    public Vector2 directionSave = Vector2.zero;

    public GameObject powerUpPrefab;

    bool isBouncing = false;
    

    void Start()
    {
        //Start the coroutine that manages input handling and cooldowns for bomb placement
        //StartCoroutine(verbWithCooldown(3));

        //Move the player with arrow key input
        StartCoroutine(PlayerMovement());

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
            //Check if this ship should bounce. If so, start the bounce coroutine
            if (Vector2.Distance(transform.position, enemyTransform.position) < 1) {
                
                StartCoroutine(bounce());

            }


            //Get the acceleration
            float acceleration = maxSpeed / timeTillMaxSpeed;

            //get the deceleration
            float deceleration = maxSpeed / decelerationTime;

            if (getInputAndMove())
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

            yield return null;
        }

    }

    private bool getInputAndMove() {

        //Make a vector to represent the movement this frame
        Vector2 movement = Vector2.zero;

        //Make a boolean to represent weather the player gave input this frame
        bool hasInputed = false;

        //Only take input if you are not bouncing
        if (!isBouncing) {

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

        }


        //If you have gotten input, overite the direction
        if (hasInputed)
        {
            directionSave = movement;
        }

        //Normalize the movment and find its magnitude this frame
        directionSave = Vector2.Normalize(directionSave) * unitsPerSec * Time.deltaTime;

        //Add the movement to the position
        transform.position += (Vector3)directionSave;

        //Rotate given the last direction of movement
        if (directionSave.normalized != Vector2.zero) {
            transform.up = (Vector3)directionSave.normalized;
        }
        

        //Indicate whether the player has given input this frame
        return hasInputed;

    }

    public IEnumerator bounce()
    {
        isBouncing = true;

        yield return new WaitForSeconds(2);

        isBouncing = false;
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
