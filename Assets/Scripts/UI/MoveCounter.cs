using UnityEngine;
using TMPro;

public class MoveCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moveCountText;

    private int moveCount = 0;
    private CoinManager coinManager;

    private void Start()
    {
        coinManager = FindAnyObjectByType<CoinManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)  || Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            moveCount++;
            UpdateDisplay();
            coinManager?.OnMoveMade(moveCount);
        }
    }

    private void UpdateDisplay()
    {
        if (moveCountText != null)
            moveCountText.text = "Moves: " + moveCount;
    }

    public int GetMoveCount() => moveCount;

    public void RegisterMove()
    {
        moveCount++;
        UpdateDisplay();
    }

    public void ResetCount()
    {
        moveCount = 0;
        UpdateDisplay();
    }
}