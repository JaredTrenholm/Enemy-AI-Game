using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Debug.Log("Loaded");
        SceneManager.LoadScene(1);
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            PlayGame();
        }
    }
}
