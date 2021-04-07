using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    private float loadTime = 2f;
    private float timeloaded = 0f;
    public Text loadText;

    private void Start()
    {
        timeloaded = 0f;
    }

    private void Update()
    {
        if(timeloaded >= loadTime)
        {
            loadText.enabled = false;

            
                SceneManager.LoadScene(2);
            
        } else
        {
            timeloaded += Time.deltaTime;
        }

    }
}
