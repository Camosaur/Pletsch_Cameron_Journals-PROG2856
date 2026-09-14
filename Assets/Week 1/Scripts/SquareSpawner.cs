using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SquareSpawner : MonoBehaviour
{
    public float sideSize = 1; //The size of each of the square's sides

    void Update()
    {
        //Find the mouse position in world space!
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());


        //Draw the transparent square
        drawSquare(mousePos, sideSize, Color.yellow, 0);

        //Change the size when you scroll the mouse wheel
        if (Mouse.current.scroll.ReadValue() != Vector2.zero)
        {

            sideSize += Mouse.current.scroll.ReadValue().y / 2;

        }

        //When clicked, draw a white square
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            drawSquare(mousePos, sideSize, Color.white, 99999999);
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
