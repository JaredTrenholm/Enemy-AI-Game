using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    private float loadTime = 5f;
    private float timeloaded = 0f;
    public Text loadText;

    private void Start()
    {
        timeloaded = 0f;
    }

    private void Update()
    {
        if (timeloaded >= loadTime)
        {
            loadText.enabled = false;


            SceneManager.LoadScene(2);

        }
        else
        {
            timeloaded += Time.deltaTime;
        }

    }
}
