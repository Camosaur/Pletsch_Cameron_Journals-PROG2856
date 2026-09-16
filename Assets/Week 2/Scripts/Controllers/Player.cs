using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Transform enemyTransform;
    public GameObject bombPrefab;
    public List<Transform> asteroidTransforms;

    public int numberOfTrailBombs;
    public int bombTrailSpacing;
    

    void Start()
    {
       //Start the coroutine that manages input handling and cooldowns
       StartCoroutine(verbWithCooldown(3));

        
    }

    private void Update()
    {
        //Test Task 2
        if (Keyboard.current.cKey.wasPressedThisFrame) { 
            
        }
    }

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

        //Call SpawnBombAtOffset at that vector * inDistance
    
    }


    //In class
    Vector2 fakeNormalize(Vector2 v) {

        return new Vector2((v.x / Mathf.Abs(v.magnitude)), (v.y / Mathf.Abs(v.magnitude)));
    }

}
