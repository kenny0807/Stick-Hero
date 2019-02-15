using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    #region Unity lifecycle
    void Start()
    {
        if (GameObject.FindGameObjectsWithTag("BackgroundAudio").Length == 1)
            DontDestroyOnLoad(gameObject);
        else
            Destroy(gameObject);
    }
    #endregion
}
