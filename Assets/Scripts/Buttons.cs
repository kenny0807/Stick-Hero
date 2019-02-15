using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour 
{
    #region Fields
    [SerializeField]
	private Sprite mus_on, mus_off;

    private GameObject screen;
    private GameObject backGround_Sound;
    #endregion

    #region Unity lifecycle
    void Start()
	{
       
        backGround_Sound = GameObject.FindGameObjectWithTag("BackgroundAudio");
		if (gameObject.name == "Sound") 
		{
			if(PlayerPrefs.GetString ("Music") == "Off")
			{
				GetComponent<Image> ().sprite = mus_off;
                backGround_Sound.GetComponent<AudioSource>().enabled = false;
            }
		}
	}

	void OnMouseDown ()
	{
		transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
	}

	void OnMouseUp ()
	{
		transform.localScale -= new Vector3 (0.1f, 0.1f, 0.1f);
	}

	void OnMouseUpAsButton()
	{
        
		switch (gameObject.name) 
		{
        case "Play":
            SceneManager.LoadScene ("Play");
            break;
		case "Home":
			SceneManager.LoadScene ("Main");
			break;
		case "Restart":
            transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
			screen = GameObject.FindGameObjectWithTag ("CatchClick");
			screen.GetComponent<Play> ().Restart ();
			break;
		case "Sound":
			if (PlayerPrefs.GetString ("Music") == "Off")
			{
				GetComponent<Image> ().sprite = mus_on;
				PlayerPrefs.SetString ("Music", "On");
                backGround_Sound.GetComponent<AudioSource> ().enabled = true;
			} 
			else
			{
                    backGround_Sound.GetComponent<AudioSource>().enabled = false;
                    GetComponent<Image> ().sprite = mus_off;
				PlayerPrefs.SetString ("Music", "Off");
			}
			break;
		}
	}
    #endregion
}
