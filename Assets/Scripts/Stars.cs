using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    public List<Transform> starTransforms;
    public float drawingTime;

    //private Vector3 currentPosition;
    public Transform currentPosition;
    public Vector3 startPosition;
    public Vector3 endPosition;




    void Start()
    {
        StartCoroutine(DrawConstellation());
    }

    private IEnumerator DrawConstellation()
    {
        //Repeat forever...
        while (true) {
            
            //Iterate through the list starting at the second star
            for (int index = 1; index < starTransforms.Count; index++) { 
                
                //Set start position equal to the star before this one
                startPosition = starTransforms[index-1].position;

                //Set end position equal to this star
                endPosition = starTransforms[index].position;

                //Draw the line and wait for it to finish drawing
                yield return StartCoroutine(DrawLine());
            }
        }
    }

    private IEnumerator DrawLine() 
    {
        //Calculate the units per sec so that it gets to the point in time
        float moveSpeed = Vector2.Distance(startPosition, endPosition) / drawingTime;

        //Calculate the direction to the end point
        Vector2 direction =  endPosition - startPosition; 

        //Set the curentPosition to the start point
        currentPosition.position = startPosition;

        //While you are not at the end point, move
        while (Vector2.Distance(currentPosition.position, endPosition) > 0.5f) {
            
            //Normalize the movment and find its magnitude this frame
            direction = Vector2.Normalize(direction) * moveSpeed * Time.deltaTime;

            //Add the movement to the position
            currentPosition.position += (Vector3)direction;

            //Wait a frame
            yield return null;
        }
    }


}
