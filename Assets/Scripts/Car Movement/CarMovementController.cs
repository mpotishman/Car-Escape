using UnityEngine;
using UnityEngine.UI;

public class CarMovementController : MonoBehaviour
{
    public Text messageText;

    private bool hasMovedRedCar = false;
    private bool hasMovedYellowCar = false;

    private void Start()
    {
        ShowMessage("Press the left or right arrow keys to move the red cars.");
    }

    private void Update()
    {
        if (!hasMovedRedCar && (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            hasMovedRedCar = true;
            ShowMessage("Nice! Now press A or D to move the yellow cars.");
        }

        if (hasMovedRedCar && !hasMovedYellowCar && (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)))
        {
            hasMovedYellowCar = true;
            ShowMessage("Great! Move the cars out of the way to escape!");
        }
    }

    private void ShowMessage(string message)
    {
        if (messageText != null)
            messageText.text = message;
    }
}
