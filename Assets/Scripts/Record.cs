using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Record : MonoBehaviour
{
    #region Unity lifecycle
    void Start()
    {
         GetComponent<Text>().text = PlayerPrefs.GetInt("Record").ToString();
    }
    #endregion
}
