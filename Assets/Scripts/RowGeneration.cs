using TMPro;
using UnityEngine;

public class RowGeneration : MonoBehaviour
{
    public TMP_InputField input;

    public void generate()
    {
        //validate input
        if (input.text == null)
        {
            return;
        }
        int squares = int.Parse(input.text);

        //Initialize starting values (i.e. position)
        Vector2 midPoint = Vector2.zero;

        Vector2 posChange = new Vector2(1, 0);
        if (squares < 0)
        {
            //If I get a negtive input, reverse the direction it's going

            posChange *= -1;

            //...And make the input positive
            squares *= -1;


        }

        Debug.Log("Called");
        //Repeat this ??? times
        for (int i = 0; i < squares; i++)
        {
            Debug.Log("In loop");
            //Draw the square with a random color (you didn't say it had to be white!!)
            drawSquare(midPoint, 1, Random.ColorHSV(), 99999999);

            //Increment the x position by 1 
            midPoint += posChange;
        }


    }

    void drawSquare(Vector2 midPoint, float sideSize, Color color, float timeStay)
    {

        float halfSide = sideSize / 2;

        //Find the corners
        Vector2 topLeft = new Vector2(midPoint.x - halfSide, midPoint.y + halfSide);

        Vector2 topRight = new Vector2(midPoint.x + halfSide, midPoint.y + halfSide);

        Vector2 bottomLeft = new Vector2(midPoint.x - halfSide, midPoint.y - halfSide);

        Vector2 bottomRight = new Vector2(midPoint.x + halfSide, midPoint.y - halfSide);


        //Draw the lines
        Debug.DrawLine(topLeft, topRight, color, timeStay);
        Debug.DrawLine(bottomRight, topRight, color, timeStay);
        Debug.DrawLine(topLeft, bottomLeft, color, timeStay);
        Debug.DrawLine(bottomLeft, bottomRight, color, timeStay);


    }
}
