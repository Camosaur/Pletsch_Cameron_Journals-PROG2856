using UnityEngine;

public class AddVectors : MonoBehaviour
{
    public Transform redTransform;
    public Transform blueTransform;

    void Update()
    {
        // Exercise: Visualizing Vector Addition
        // Create a new Vector2 called "rPlusB" which adds rTransform's position to bTransform's position.​

        // When the B key is held down, draw a blue line from the origin to bTransform's position.​
        // When the R key is held down, draw a red line from the origin to rTransform's position.​
        // When the R and B keys are both held down, draw a magenta from the origin to rPlusB.​

        // Exercise: Calculating Magnitude
        // Calculate the magnitude of bPlusR using the mathematical formula for calculating the length of a vector – Pythagorean Theroem.​
        // Output the magnitude to the console using either Debug.Log or print.
    }
}
