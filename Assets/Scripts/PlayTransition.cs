using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayTransition : MonoBehaviour 
{
	#region Fields
	[SerializeField]
	private GameObject playObjects;

	private bool isStartPlay;
	#endregion

	#region Unity lifecycle
	void Start()
	{
        playObjects.GetComponent<Animator>().Play("StartGame");
        StartCoroutine(WaitAnimation());
        this.GetComponent<Play>().enabled = true;
    }

	#endregion

	#region Private methods
	IEnumerator WaitAnimation()
	{
		yield return new WaitForSeconds(1.5f);
		playObjects.GetComponent<Animator> ().enabled = false;
		playObjects.transform.position = new Vector3 (-2.6f,1.25f,0f);
	}
	#endregion
}
