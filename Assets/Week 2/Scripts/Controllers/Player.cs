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

    public float cornerBombDist;

    public Transform targetPos;
    public float warpRatio;

    public float radarRange = 5;

    public float unitsPerSec = 1;

    public float maxSpeed = 5;

    public float timeTillMaxSpeed = 1;

    public float decelerationTime = 1;

    public Vector2 directionSave = Vector2.zero;

    public int circlePoints = 5;

    public float radius = 1;

    public GameObject powerUpPrefab;
    

    void Start()
    {
       //Start the coroutine that manages input handling and cooldowns for bomb placement
       StartCoroutine(verbWithCooldown(3));

        //Call enemyRadar
        enemyRadar(radius, circlePoints);

        //Call enemyRadar
        SpawnPowerups(radius, circlePoints);

    }

    private void Update()
    {
        //Test Week2 Task 3
        //WarpPlayer(targetPos, warpRatio);

        //Test Week2 Task 4
        DetectAsteroids(radarRange, asteroidTransforms);

        //Test Week2 Task 2
        if (Keyboard.current.cKey.wasPressedThisFrame) {
            SpawnBombOnRandomCorner(cornerBombDist);
        }

        //Move the player with arrow key input
        PlayerMovement();

    }

    #region Week2
    IEnumerator verbWithCooldown(int cooldownTime)
    {
        while (true)
        {
            //Wait until the player presses B
            while (!Keyboard.current.bKey.wasPressedThisFrame)
            {
                yield return null;
            }

            //Spawn the bombs
            SpawnBombTrail(transform.up, bombTrailSpacing, numberOfTrailBombs);

            //Wait for some time- the cooldown!
            yield return new WaitForSeconds(cooldownTime);
        }
    }

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

    public void SpawnBombOnRandomCorner(float inDistance) {

        //Choose a random corner by adding Transform.up * +-1 + Transform.right * +-1 and then normalizing it
        Vector2 direction = Vector2.zero;

        //Randomize both axes
        Vector2 topBottom = randomSign() * transform.up;
        Vector2 leftright = randomSign() * transform.right;

        //Add and normalize the vector to get a corner
        direction = topBottom + leftright;
        direction = fakeNormalize(direction);


        //Call SpawnBombAtOffset at that vector * inDistance
        SpawnBombAtOffset(direction * inDistance);

    }


    //In class
    Vector2 fakeNormalize(Vector2 v) {

        return new Vector2((v.x / Mathf.Abs(v.magnitude)), (v.y / Mathf.Abs(v.magnitude)));
    }

    int randomSign() {

        //Choose either 0 or 1
        int choice = UnityEngine.Random.Range(0, 2);

        //If it's 0, make it -1 instead
        if (choice == 0)
        {
            choice = -1;
        }

        //Return 1 or -1
        return choice;
    
    }

    public void WarpPlayer(Transform target, float ratio) {

        //Set the position of the player to a point in between it and the enemy, determined by a ratio.
        transform.position = Vector3.Lerp((Vector2)target.position + Vector2.up*3, target.position, ratio);

    }

    public void DetectAsteroids(float inMaxRange, List<Transform> inAsteroids) {

        Vector2 radarLine = new Vector2();

        //Loop through each asteroid
        foreach (Transform asteroid in inAsteroids) {
            //Check the distance- skip this iteration if the asteroid is not within range
            if (Vector2.Distance(asteroid.position, transform.position) > inMaxRange) continue;

            //Subtract the player's position from the asteroid's positon to get the vector from the player's position to that asteroid's position
            radarLine = asteroid.position - transform.position;

            //Normalize the vector, then multiply it by 2.5
            radarLine = fakeNormalize(radarLine) * 2.5f;

            //Draw the line from the player's position to the new vector + the player's positon
            Debug.DrawLine(transform.position, (Vector2)transform.position + radarLine);

        }
    
    }

    #endregion

    #region Movement
    public void PlayerMovement() {
        
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

    }

    private bool getInputAndMove() {

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

    #endregion

    public void enemyRadar(float radius, int circlePoints) { 
    
        StartCoroutine(enemyRadarRuntime(radius, circlePoints));
    }

    IEnumerator enemyRadarRuntime(float radius, int circlePoints) {

        //Declare the local variables I will need
        Color shieldColor = Color.red;
        List<float> angleList = new List<float>();
        List<Vector2> pointList = new List<Vector2>();
        float angleStep;

        //Calc the list of angles
        angleStep = 360 / circlePoints;

        for (int i = 0; i < circlePoints; i++)
        {
            angleList.Add(angleStep * i);
        }


        //Calc the list of points
        foreach (float angle in angleList)
        {
            pointList.Add(new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius);
        }

        //Start the infinte cycle
        while (true)
        {
            //-Determine the color of the shield
            if (Vector2.Distance(enemyTransform.position, transform.position) >= radius) { 
                shieldColor = Color.red;
            }
            else
            {
                shieldColor = Color.green;
            }

            //-Draw the lines
            for (int index = 1; index < pointList.Count; index++)
            {

                //Set start position equal to the star before this one
                Vector2 startPosition = pointList[index - 1] + (Vector2)transform.position;

                //Set end position equal to this star
                Vector2 endPosition = pointList[index] + (Vector2)transform.position;

                //Draw the line
                Debug.DrawLine(startPosition,endPosition, shieldColor);
            }
            
            Debug.DrawLine(pointList[0] + (Vector2)transform.position, pointList[pointList.Count - 1] + (Vector2)transform.position, shieldColor);

            //-Wait
            yield return null;
        }
    
    }

    public void SpawnPowerups(float radius, int numberOfPowerups)
    {
        //Declare the local variables I will need
        List<float> angleList = new List<float>();
        List<Vector2> pointList = new List<Vector2>();
        float angleStep;

        //Calc the list of angles
        angleStep = 360 / circlePoints;

        for (int i = 0; i < circlePoints; i++)
        {
            angleList.Add(angleStep * i);
        }


        //Calc the list of points
        foreach (float angle in angleList)
        {
            pointList.Add(new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius);
        }

        //Instantiate a powerUp at each point on the list
        foreach (Vector2 point in pointList) {

            Instantiate(powerUpPrefab, point + (Vector2)transform.position, Quaternion.identity);
        
        }
    }
}
