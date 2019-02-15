using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCntr : MonoBehaviour
{

    public string action;
    public GameObject mus_on, mus_off;
    
    private void Start()
    {
        if (gameObject.name == "SoundButton")
            if (PlayerPrefs.GetString("Music") != "no")
            {
                mus_on.SetActive(true);
                mus_off.SetActive(false);
                //GameObject.Find("MainAudio").GetComponent<AudioSource>().Play();
            }
            else
            {
                mus_on.SetActive(false);
                mus_off.SetActive(true);
            }
    }
    
    private void OnMouseUpAsButton()
    {
        //if (PlayerPrefs.GetString("Music") != "no")
          //  GameObject.Find("ClickAudio").GetComponent<AudioSource>().Play();
        switch (gameObject.name)
        {
            case "PlayButton":
                Application.LoadLevel("play");
                break;
            case "SoundButton":
                if (PlayerPrefs.GetString("Music") != "no")
                {
                    PlayerPrefs.SetString("Music", "no");
                    mus_on.SetActive(false);
                    mus_off.SetActive(true);
                   // GameObject.Find("MainAudio").GetComponent<AudioSource>().Stop();
                }
                else
                {
                    PlayerPrefs.SetString("Music", "yes");
                    mus_on.SetActive(true);
                    mus_off.SetActive(false);
                    //GameObject.Find("MainAudio").GetComponent<AudioSource>().Play();
                }

                break;
            case "Home":
                Application.LoadLevel("Main");
                break;
            case "Replay":
                Application.LoadLevel("play");
                break;
           
        }
    }
}


 