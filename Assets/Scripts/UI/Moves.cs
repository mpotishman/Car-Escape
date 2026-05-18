using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves : MonoBehaviour
{
    public int incrementAmount = 1; // The amount to increment the value on each key press
    private int currentValue = 0;

    private void Update()
    {
        // Check for key presses and increment the value accordingly
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            currentValue += incrementAmount;
            Debug.Log("Value incremented. Current value: " + currentValue);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            currentValue += incrementAmount;
            Debug.Log("Value incremented. Current value: " + currentValue);
        }
    }
}
