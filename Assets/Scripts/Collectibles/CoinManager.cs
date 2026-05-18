using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{

    // initialise levelChanger variables
    private LevelChangerScipt levelChanger;

    // define variable that is in the inspector that holds the number of coins
    [SerializeField] private TextMeshProUGUI coinCounter;

    // create variable that holds each of the coins
    [Header("Coins")]
    [SerializeField] private GameObject[] coins = new GameObject[3];

    // define a map for moves to coins
    [SerializeField] private int[] moveThresholds = { 10, 20 };
    // if moves > 10, max = 2
    // if moves > 20, max = 1  
    // if moves > 30, max = 0

    // define variables for current coins collected, max coins possible
    private int coinsCollected = 0;
    private int maxCoins = 3;

    private void Start(){
        // get the level changer object
        levelChanger = FindAnyObjectByType<LevelChangerScipt>();

        coinCounter.text = "Coins: " + coinsCollected;
    }

    // define a public function OnMoveMade, that checks how many thresholds have passed 
    // based on current move count, updates maxcoins and disables any coins that are now
    // over the limit
    public void OnMoveMade(int currentMoves){
        int thresholdCrossed = 0;
        foreach (int threshold in moveThresholds){
            if (currentMoves > threshold)
                thresholdCrossed++;
        }
        maxCoins = 3 - thresholdCrossed;

        // now disable coins based on which arent available any more
        for (int i = 0; i < thresholdCrossed; i++)
        {
            if (coins[i] != null)
                coins[i].SetActive(false);
        }
    }

    public void OnCoinCollected(){
        coinsCollected++;
        coinCounter.text = "Coins: " + coinsCollected;

        // if user has collected the last coin, call level changer script 
        if (coinsCollected == maxCoins){
            levelChanger.loadNextLevel();
        }
    }

    
}
