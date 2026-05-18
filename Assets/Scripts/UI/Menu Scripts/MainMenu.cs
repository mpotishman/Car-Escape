using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadTutorial()
    {
        SceneManager.LoadScene("TutorialLevel");
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("FirstLevel");
    }
}
