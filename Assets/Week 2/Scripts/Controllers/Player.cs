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

    //Velocity
    private float unitsPerSec = 0;

    //Max Speed
    public float maxUnitsPerSec = 5;

    //Time since input pressed
    private float accelationTime;

    //Time to get to max speed
    public float maxAccelerationTime = 0;

    //
    public Vector2 velocity = Vector2.zero;

    public float speed;
    

    void Start()
    {
       //Start the coroutine that manages input handling and cooldowns for bomb placement
       StartCoroutine(verbWithCooldown(3));
        
       
        
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

        //Check speed
        speed = velocity.magnitude;

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

    public void PlayerMovement() {
  
            //Make a vector to represent the movement this frame
            Vector2 accel = Vector2.zero;

        //Has the player pressed an input?
        bool playerMoved = false;

            //LEFT
            if (Keyboard.current.leftArrowKey.isPressed)
            {
                //Direction
                accel += Vector2.left;

                //Player has pressed input
                playerMoved = true;

            }

            //RIGHT
            if (Keyboard.current.rightArrowKey.isPressed)
            {
                //Direction
                accel += Vector2.right;

                //Player has pressed input
                playerMoved = true; 
            }

            //UP
            if (Keyboard.current.upArrowKey.isPressed)
            {
                //Direction
                accel += Vector2.up;

                //Player has pressed input
                playerMoved = true;
            }

            //DOWN
            if (Keyboard.current.downArrowKey.isPressed)
            {
                //Direction
                accel += Vector2.down;

                //Player has pressed input
                playerMoved = true;


            }



        //Check if the player moved and the speed is within it's intended parameters. Only change velocity if so it needs to change.
        if (playerMoved && speed <= maxUnitsPerSec)
        {
            //Normalize the acceleration input and find its magnitude this frame
            accel = Vector2.Normalize(accel) * (maxUnitsPerSec / maxAccelerationTime) * Time.deltaTime;
        }
        else if(!playerMoved && speed >= 0.01) {

            //Decelerate if the player did not move
            accel = (velocity*-1) * (maxUnitsPerSec / maxAccelerationTime) * Time.deltaTime;
        }

        //Change in velocity
        velocity += accel;

        //Make sure the velocity is clamped
        velocity = Vector2.ClampMagnitude(velocity, maxUnitsPerSec);
      

        //Add the velocity to the position
        transform.position += (Vector3)velocity * Time.deltaTime;

    }
   
}
