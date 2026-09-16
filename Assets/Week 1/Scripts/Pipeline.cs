using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pipeline : MonoBehaviour
{
    Coroutine drawing;


    void Update()
    {
        //If mouse was pressed THIS FRAME, start/reset the coroutine
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            drawing = StartCoroutine(DrawAndCalculate());
        }
    }

    IEnumerator DrawAndCalculate() {
        //Initialize a "TotalMag" float variable
        float totalMag = 0;
        Vector2 mousePos;

        //While true loop...
        while (true)
        {
            //Store mouse position
            mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            //Wait 0.1 seconds
            yield return new WaitForSeconds(0.1f);

            //If the mouse is not held down, print TotalMag and then exit the coroutine
            //yield break;
            if (!Mouse.current.leftButton.isPressed) {
                Debug.Log(totalMag);
                yield break;
            }

            //Draw the line
            Debug.DrawLine(mousePos, Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()), Random.ColorHSV(), 99999999);

            //Find the vector between the current mouse position and the previous one
            Vector2 between = mousePos - (Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            //Add that vectors magnatude to TotalMag
            totalMag += Mathf.Abs(Mathf.Sqrt(Mathf.Pow(between.x, 2) + Mathf.Pow(between.x, 2)));
        }
    }
}
