using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCntr : MonoBehaviour
{
    private const float SPEED_PLATFORM = 20f;
    private const float SPEED_BRIDGE = 4f;
    private const float SPEED_PLAYER = 3f;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject platform;
    [SerializeField]
    private GameObject bridge;
    [SerializeField]
    private GameObject allObjects;
    [SerializeField]
    private GameObject buttonsLose;
    [SerializeField]
    private GameObject cubeToDouble;
    [SerializeField]
    private Text score;
    [SerializeField]
    private Text record;

    private AudioSource audioFallBridge,
    audioGameOver;

    private GameObject platformCurrent,
    platformNext,
    bridgeCurrent,
    pointStartBridge,
    doubleCube;

    private float rangeMoveScreene,
    playerStartPositionX,
    lenthBridge,
    platformNextPositionX;

    private int scoreValue;

    private bool isMousePress,
    isLengthBridgeIncrease,
    isBridgeFell,
    isLenthBridgeCorrect,
    isLevelomplete,
    isGameOver,
    isDoubleScore;


    #region Unity lifecycle
    void Start()
    {
       // audioFallBridge = GameObject.Find("FallBridge").GetComponent<AudioSource>();
        //audioGameOver = GameObject.Find("GameOver").GetComponent<AudioSource>();
        platformNext = CreatePlatform();
        platformCurrent = GameObject.FindGameObjectWithTag("Platform");
       // score.gameObject.SetActive(true);
       // record.text = "RECORD : " + PlayerPrefs.GetInt("Record");
        //scoreValue = 0;
        playerStartPositionX = -2.2f;
    }

    void Update()
    {
        if (!isGameOver)
        {
            if (platformNext.transform.position.x != platformNextPositionX)
            {
                platformNext.transform.position = Vector3.MoveTowards(platformNext.transform.position,
                    new Vector3(platformNextPositionX,
                        platformCurrent.transform.position.y,
                        platformNext.transform.position.z),
                    Time.deltaTime * SPEED_PLATFORM);
            }
            if (!isLengthBridgeIncrease)
            {
                if (isMousePress)
                {
                    BridgeUp();
                }
            }
            else
            {
                LowerBridge();
            }
            if (isBridgeFell)
            {

                if (isLenthBridgeCorrect)
                {

                    if ((player.transform.position.x >= platformNext.transform.position.x + platformNext.transform.localScale.x / 2 - 0.19f - 0.000002f) &&
                        (player.transform.position.x <= platformNext.transform.position.x + platformNext.transform.localScale.x / 2 - 0.19f + 0.000002f))
                    {
                        isLenthBridgeCorrect = false;
                        isLevelomplete = true;
                        isBridgeFell = false;
                        isLengthBridgeIncrease = false;
                        Destroy(doubleCube.gameObject);
                        scoreValue += isDoubleScore ? 2 : 1;
                        score.text = scoreValue.ToString();
                        rangeMoveScreene = allObjects.transform.position.x
                            - platformNext.transform.position.x + pointStartBridge.transform.position.x
                            - platformNext.transform.localScale.x / 2;
                        platformCurrent = platformNext;
                        platformNext = CreatePlatform();

                    }
                    else
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position,
                            new Vector3(
                                platformNext.transform.position.x + platformNext.transform.localScale.x / 2 - 0.19f,
                                player.transform.position.y,
                                player.transform.position.z),
                            Time.deltaTime * SPEED_PLAYER);
                    }
                }
                else
                {
                    if (player.transform.position.x >= pointStartBridge.transform.position.x + lenthBridge - 0.000002f &&
                        player.transform.position.x <= pointStartBridge.transform.position.x + lenthBridge + 0.000002f)
                    {
                        audioGameOver.Play();
                        isGameOver = true;
                        buttonsLose.gameObject.SetActive(true);
                        record.gameObject.SetActive(true);
                        if (PlayerPrefs.GetInt("Record") < scoreValue)
                        {
                            PlayerPrefs.SetInt("Record", scoreValue);
                            record.text = "RECORD : " + PlayerPrefs.GetInt("Record");
                        }

                    }
                    else
                    {
                        player.transform.position = Vector3.MoveTowards(player.transform.position,
                            new Vector3(pointStartBridge.transform.position.x + lenthBridge,
                                player.transform.position.y,
                                player.transform.position.z),
                            Time.deltaTime * SPEED_PLAYER);
                    }
                }
            }

            if (isLevelomplete)
            {
                GoToNextLevel();
                StartCoroutine(WaitNexLevel());
            }
        }

        if (isGameOver)
        {
            GameOver();
        }


    }

    void OnMouseDown()
    {
        if (!isLengthBridgeIncrease)
        {
            if (Mathf.Approximately(playerStartPositionX, player.transform.position.x))
            {
                bridgeCurrent = CreateBridge();
                pointStartBridge = CreateStartPointBridge();
                isMousePress = true;
            }
        }

    }

    void OnMouseUp()
    {
        if (isMousePress)
        {
            isMousePress = false;
            isLengthBridgeIncrease = true;
            lenthBridge = bridgeCurrent.transform.localScale.y;
        }

    }
    #endregion

    #region Public methods
    public void Restart()
    {
        foreach (Transform child in allObjects.transform)
        {
            if (child.tag != "Player" && child != null)
            {
                GameObject.Destroy(child.gameObject);
            }
        }
        scoreValue = 0;
        score.text = scoreValue.ToString();
        buttonsLose.gameObject.SetActive(false);
        allObjects.transform.position = new Vector3(0f, 0f, 0f);
        platformCurrent = Instantiate(platform, new Vector3(-2.6f, -3.1f, -2.9f), Quaternion.identity);
        platformCurrent.transform.localScale = new Vector3(1.2f, 4, 1);
        platformCurrent.transform.parent = allObjects.transform;
        platformNext = CreatePlatform();
        player.transform.position = new Vector3(-2.2f, -0.95f, -2.9f);
        isGameOver = false;
        isLevelomplete = false;
        isMousePress = false;
        isLengthBridgeIncrease = false;
        isBridgeFell = false;
        isLenthBridgeCorrect = false;
        isDoubleScore = false;
        playerStartPositionX = player.transform.position.x;
        record.gameObject.SetActive(false);
    }
    #endregion

    #region Private methods
    void GoToNextLevel()
    {
        if (allObjects.transform.position.x != rangeMoveScreene)
        {
            allObjects.transform.position = Vector3.MoveTowards(allObjects.transform.position,
                new Vector3(rangeMoveScreene, allObjects.transform.position.y, allObjects.transform.position.z),
                Time.deltaTime * SPEED_PLATFORM);
        }
    }

    void GameOver()
    {
        if (pointStartBridge != null && pointStartBridge.transform.rotation.eulerAngles.z != 180f)
        {
            pointStartBridge.transform.Rotate(0, 0, -9);
        }
        player.transform.position = Vector3.MoveTowards(player.transform.position,
            new Vector3(player.transform.position.x, -6f, player.transform.position.z),
            Time.deltaTime * SPEED_PLAYER * 6);
    }

    IEnumerator WaitNexLevel()
    {
        yield return new WaitForSeconds(0.3f);
        playerStartPositionX = player.transform.position.x;
        isLevelomplete = false;
    }

    GameObject CreatePlatform()
    {
        GameObject platformInst;
        platformInst = Instantiate(platform, new Vector3(8.90f, -3.1f, -2.9f), Quaternion.identity);
        platformInst.transform.localScale = new Vector3(Random.Range(0.3f, 1.2f), 4, 1);
        platformNextPositionX = Random.Range(-1.8f + platformInst.transform.localScale.x / 2,
            2.8f - platformInst.transform.localScale.x / 2);
        platformInst.transform.parent = allObjects.transform;

        doubleCube = Instantiate(cubeToDouble,
            new Vector3(platformInst.transform.position.x,
                platformInst.transform.position.y + platformInst.transform.localScale.y / 2,
                platformInst.transform.position.z - 1f),
            Quaternion.identity);
        doubleCube.transform.localScale = new Vector3(0.2f, 0.04f, 1f);
        doubleCube.transform.position = new Vector3(doubleCube.transform.position.x,
            doubleCube.transform.position.y - doubleCube.transform.localScale.y / 2,
            doubleCube.transform.position.z);
        doubleCube.transform.parent = platformInst.transform;

        return platformInst;
    }

    GameObject CreateStartPointBridge()
    {
        GameObject pointInst;
        pointInst = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pointInst.transform.position = new Vector3(
            platformCurrent.transform.position.x + platformCurrent.transform.localScale.x / 2,
            platformCurrent.transform.position.y + platformCurrent.transform.localScale.y / 2,
            platformCurrent.transform.position.z);
        pointInst.transform.localScale = new Vector3(0.000001f, 0.000001f, 0.000001f);
        pointInst.transform.parent = allObjects.transform;

        return pointInst;
    }

    GameObject CreateBridge()
    {
        GameObject bridgeInst;
        bridgeInst = Instantiate(bridge,
            new Vector3(platformCurrent.transform.position.x + platformCurrent.transform.localScale.x / 2,
                platformCurrent.transform.position.y + platformCurrent.transform.localScale.y / 2,
                platformCurrent.transform.position.z),
            Quaternion.identity);
        bridgeInst.transform.parent = allObjects.transform;
        return bridgeInst;
    }

    void BridgeUp()
    {
        if (bridgeCurrent.transform.localScale.y < 5.5f)
        {
            bridgeCurrent.transform.localScale = new Vector3(bridgeCurrent.transform.localScale.x,
                bridgeCurrent.transform.localScale.y + Time.deltaTime * SPEED_BRIDGE,
                bridgeCurrent.transform.localScale.z);
            bridgeCurrent.transform.position = new Vector3(bridgeCurrent.transform.position.x,
                bridgeCurrent.transform.position.y + Time.deltaTime * SPEED_BRIDGE / 2,
                bridgeCurrent.transform.position.z);
        }

    }

    void LowerBridge()
    {
        bridgeCurrent.transform.parent = pointStartBridge.transform;
        if (pointStartBridge.transform.rotation.eulerAngles.z != 270f)
        {
            pointStartBridge.transform.Rotate(0, 0, -3);
        }
        if (Mathf.Approximately(pointStartBridge.transform.rotation.eulerAngles.z, 270f))
        {
            isLengthBridgeIncrease = false;
            audioFallBridge.Play();
            isBridgeFell = true;
            if ((lenthBridge >= platformNext.transform.position.x - platformNext.transform.localScale.x / 2 - pointStartBridge.transform.position.x) &&
                (lenthBridge <= platformNext.transform.position.x + platformNext.transform.localScale.x / 2 - pointStartBridge.transform.position.x))
            {
                isLenthBridgeCorrect = true;
                if ((lenthBridge >= platformNext.transform.position.x - doubleCube.transform.lossyScale.x / 2 - pointStartBridge.transform.position.x) &&
                    (lenthBridge <= platformNext.transform.position.x + doubleCube.transform.lossyScale.x / 2 - pointStartBridge.transform.position.x))
                {
                    isDoubleScore = true;
                }
                else
                {
                    isDoubleScore = false;
                }

            }
            else
            {
                isLenthBridgeCorrect = false;
            }
        }
    }
    #endregion
}
