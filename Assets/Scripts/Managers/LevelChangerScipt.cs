// gets the current level from scene eg Level1, and then when last coin is collected make it go to level {current level + 1}
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChangerScipt : MonoBehaviour
{
    



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void loadNextLevel(){
        string sceneName = SceneManager.GetActiveScene().name;
        string currentLevel = sceneName.Substring(5);
        int levelNumber = int.Parse(currentLevel) + 1;
        string nextLevel = "Level" + levelNumber;

        // now check if next level exists, and if it does then move player to that level
        if (SceneUtility.GetBuildIndexByScenePath(nextLevel) != -1)
        {
            SceneManager.LoadScene(nextLevel);
        }
    }
}
