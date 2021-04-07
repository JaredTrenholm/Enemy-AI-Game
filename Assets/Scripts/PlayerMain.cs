using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerMain : MonoBehaviour
{
    private Vector3 StartPos;

    public Image Death;

    public float TargetTime = 2f;
    public float TimePassed = 0f;
    public bool TimerActive = false;

    public bool TestKill = false;

    public static int lives = 1;

    public GameObject enemies;

    public bool TestWin = false;

    private void Start()
    {
        StartPos = this.gameObject.transform.position;
        
    }


    private void Update()
    {
        if (TestWin)
        {
            SceneManager.LoadScene(3);
        }

        if(TestKill == true)
        {
            Kill();
            TestKill = false;
        }
        if(TimerActive == true)
        {

            if(TimePassed >= TargetTime)
            {
                Death.enabled = false;
                SceneManager.LoadScene(2);

                if(lives <= 0) { SceneManager.LoadScene(0); }

            }
            else
            {
                TimePassed += Time.deltaTime;
            }
        }
    }
    public void Kill()
    {
        TimerActive = true;
        Death.enabled = true;
        this.gameObject.GetComponent<CharacterController>().enabled = false;
        this.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        lives = lives - 1;
        enemies.SetActive(false);
    }

    public void Victory() {
        SceneManager.LoadScene(3);
    }
}
