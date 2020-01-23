using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject deathmenu;
    public GameObject winmenu;
    public GameObject layover;
    public Image timer;
    public byte R;
    public byte G;
    public byte B;
    public float waittime = 30.0f;
    private bool timeup = false;
    private bool courrunning = false;


    private void Start()
    {
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (!timeup)
        {
            timer.fillAmount -= 1.0f / waittime * Time.deltaTime;
            if (timer.fillAmount <.35f && !courrunning) {
                StartCoroutine(changeing());
                courrunning = true;
            }
            if (timer.fillAmount == 0)
            {
                timeup = true;
            }
        }
        else {
            death();
        }
    }

    public void backtoscene(int number) {
        SceneManager.LoadScene(number);
    }



    public void death() {
        Time.timeScale = 0f;
        if (FindObjectOfType<TouchManager>() != null)
        {
            FindObjectOfType<TouchManager>().gameObject.SetActive(false);
        }
        else if (FindObjectOfType<ReverseTouchManager>() != null) {
            FindObjectOfType<ReverseTouchManager>().gameObject.SetActive(false);
        }
        layover.SetActive(true);
        deathmenu.SetActive(true);
    }

    public void Win() {
        Time.timeScale = 0f;
        layover.SetActive(true);
        winmenu.SetActive(true);
    }

    IEnumerator changeing(){
        do
        {
            timer.color = new Color32(R,G,B,255);
            yield return new WaitForSeconds(.5f);
            timer.color = Color.white;
            yield return new WaitForSeconds(.5f);
        } while (true);
    }

}
