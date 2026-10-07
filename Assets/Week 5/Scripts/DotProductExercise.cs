using UnityEngine;

public class DotProductExercise : MonoBehaviour
{
    public float redAngle = 0;
    public float blueAngle = 0;

    public Vector2 redVector;
    public Vector2 blueVector;

    public float dotProd;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        redVector = new Vector2(Mathf.Cos(redAngle * Mathf.Deg2Rad), Mathf.Sin(redAngle * Mathf.Deg2Rad));

        blueVector = new Vector2(Mathf.Cos(blueAngle * Mathf.Deg2Rad), Mathf.Sin(blueAngle * Mathf.Deg2Rad));

        dotProd = redVector.x * redVector.y + blueVector.x * blueVector.y;

        if (dotProd <= float.Epsilon) { 
            dotProd = 0;
        }
    }
}
