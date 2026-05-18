using UnityEngine;
using TMPro;                        // TextMeshPro — better text rendering than the old UI.Text

// --- SETUP ---
// 1. Create an empty GameObject in your scene, name it "MoveCounter"
// 2. Drag this script onto it
// 3. Create a UI Text (TextMeshPro) element in your Canvas to display the count
// 4. Drag that Text object into the "moveCountText" slot in the Inspector
public class MoveCounter : MonoBehaviour
{
    // [SerializeField] lets us assign this in the Inspector without making it fully public
    // TextMeshProUGUI is the component type for UI text using TextMeshPro
    [SerializeField] private TextMeshProUGUI moveCountText;

    // "private" means only this script can read/change moveCount
    // "int" is a whole number — no decimals needed for a move counter
    private int moveCount = 0;

    // Update() is called by Unity every single frame
    private void Update()
    {
        // GetKeyDown() fires ONCE on the frame the key is first pressed
        // (unlike GetKey() which fires every frame it's held down)
        // We check all movement keys — any one counts as a move
        if (Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.UpArrow)    || Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.DownArrow)  || Input.GetKeyDown(KeyCode.S))
        {
            // "++" increments moveCount by 1 — same as writing moveCount = moveCount + 1
            moveCount++;

            // Call our helper method to refresh the on-screen text
            UpdateDisplay();
        }
    }

    // Private helper — only called from within this script
    private void UpdateDisplay()
    {
        // "!= null" checks that a Text object was actually assigned in the Inspector
        // If not, we skip rather than crash
        if (moveCountText != null)
            moveCountText.text = "Moves: " + moveCount;
    }

    // "public" so OTHER scripts can call this to read the move count
    // "int" means it returns a whole number
    // "=>" is shorthand for a one-line method that just returns something
    public int GetMoveCount() => moveCount;

    // Called by car scripts whenever the player presses a movement key
    public void RegisterMove()
    {
        moveCount++;
        UpdateDisplay();
    }

    // Public so the coin/level system can reset the counter when a new level loads
    public void ResetCount()
    {
        moveCount = 0;
        UpdateDisplay();
    }
}
