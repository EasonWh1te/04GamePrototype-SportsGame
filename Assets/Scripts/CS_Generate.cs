using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class CS_Generate : MonoBehaviour
{
    //计时器
    [SerializeField] float[] myTimer;
    //[SerializeField] float myTimer;
    private float myLiveTime = 0f;
    
    //飞盘
    //[SerializeField] GameObject mySaucer;
    [SerializeField] GameObject[] mySaucer;
    [SerializeField] Transform[] myPosition;
    private int pos;
    private int num;
    [SerializeField] Vector3[] myDir;
    // 玩家
    [SerializeField] GameObject myPlayer;

    //流程控制
    private int i = 0;
    public int round = 1;

    //控制每轮生成飞盘总数相等
    private bool hasTwice = false;

    // UI
    [SerializeField] GameObject myUI;

    [SerializeField] Text myRound;
    [SerializeField] Text myEndTips;
    [SerializeField] Text mySpeedTips;
    [SerializeField] Text myTimerTips;

    [SerializeField] Text finalScore;

    [SerializeField] GameObject myNameInput;

    // Music
    public AudioSource myMusic;
    public AudioClip soundPass;

    // other
    public static bool isInput = false;
    private void Start()
    {
        myEndTips.enabled = false;
        mySpeedTips.enabled = false;
        myTimerTips.enabled = false;
        finalScore.enabled = false;
        myMusic.clip = soundPass;
        isInput = false;
    }
    private void Update()
    {
        if (CS_Player.gameStart == true)
        {
            myLiveTime += Time.deltaTime;
            //流程控制
            
            if (myLiveTime >= myTimer[round - 1])//动态更新重置时间
            {
                // lastround
                myLiveTime = 0f;//刷新计时器

                //轮次控制 debug
                if (i == 3)
                {
                    i = 1;
                    if (round < 3)// 逻辑debug
                    {
                        round++;
                        StartCoroutine(Tips1());
                        hasTwice = false;
                    }
                }
                else
                {
                    i++;
                }

                Debug.Log(i);

                //随机位置与发射个数
                pos = Random.Range(0, 4);
                if (hasTwice == false)
                {
                    if (i != 3)
                        num = Random.Range(0, 2);
                    else
                        num = 1;
                }
                else
                    num = 0;
                if (num == 1) hasTwice = true;
                //生成飞盘
                switch (num)
                {
                    case 0:
                        GenerateSaucer();
                        myPlayer.GetComponent<CS_Player>().shootChance = 1;
                        break;
                    case 1:
                        GenerateSaucer();
                        StartCoroutine(GenerateSaucer2(round));
                        myPlayer.GetComponent<CS_Player>().shootChance = 2;
                        //Invoke("GenerateSaucer", 1);
                        break;
                }
                 if(i == 3 && round == 3)
                {
                    CS_Player.gameStart = false;
                    Debug.Log("GameOver");
                    //GameOver2();
                    StartCoroutine(GameOver());
                    
                    //
                    /*
                    myRound.enabled = false;
                    myEndTips.enabled = true;
                    CS_Player.gameStart = false;
                    */
                }


            }
        }
        
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void MyQuit()
    {
        Application.Quit();
    }
    /*private void Update()
    {
        myLiveTime += Time.deltaTime;
        if (myLiveTime >= myTimer)
        {
            myLiveTime = 0f;
            if (i % 3 == 0)
            {
                GameObject t_saucer = Instantiate(mySaucer, myPosition[i - 1].position, Quaternion.identity);
                Vector3 myDirection = new Vector3(3, 2, 0);
                myDirection = myDirection.normalized;
                t_saucer.GetComponent<CS_Saucer>().Init(myDirection);
                //Debug.Log(2);

                StartCoroutine("Generate2");//在此处函数中重置i 发射间隔不能小于1s

                //可以尝试invoke是否可行——可行
                //Invoke("Generate2new", 1);
                //i = 1;
            }
            else
            {
                GameObject t_saucer = Instantiate(mySaucer, myPosition[i - 1].position, Quaternion.identity);
                Vector3 myDirection;
                if (i == 1)
                    myDirection = new Vector3(1, 1, 0);
                else
                    myDirection = new Vector3(-1, 1, 0);
                t_saucer.GetComponent<CS_Saucer>().Init(myDirection);
                i++;
            }
            
        }
    }*/

    private IEnumerator Tips1()
    {
        if(round == 2)
            mySpeedTips.enabled = true;
        myTimerTips.enabled = true;
        yield return new WaitForSeconds(2.0f);
        if(round == 2)
            mySpeedTips.enabled = false;
        myTimerTips.enabled = false;
    }

    private void GameOver2()
    {
        myRound.enabled = false;
        myEndTips.enabled = true;
        CS_Player.gameStart = false;
    }
    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(4f);
        myRound.enabled = false;
        myEndTips.enabled = true;
        

        Cursor.lockState = CursorLockMode.None;
        myNameInput.SetActive(true);
        finalScore.enabled = true;
        finalScore.text = "Your Score:" + CS_Player.score.ToString();
        myUI.SetActive(false);
        isInput = true;
    }
    private void GenerateSaucer()
    {
        GameObject t_saucer = Instantiate(mySaucer[round - 1], myPosition[pos].position, Quaternion.identity);//生成速度随轮次改变，位置随机的飞盘
        t_saucer.GetComponent<CS_Saucer>().Init(myDir[pos].normalized);//发射方向与发射位置相关 （可以尝试与发射个数相关）
        myMusic.Play();
    }

    private IEnumerator GenerateSaucer2(int round)
    {
        yield return new WaitForSeconds(1.0f);
        GameObject t_saucer = Instantiate(mySaucer[round - 1], myPosition[pos].position, Quaternion.identity);//生成速度随轮次改变，位置随机的飞盘
        t_saucer.GetComponent<CS_Saucer>().Init(myDir[pos].normalized);//发射方向与发射位置相关 （可以尝试与发射个数相关）
        myMusic.Play();
    }

    private void Generate2new()
    {
        //Debug.Log(i);
        GameObject t_saucer = Instantiate(mySaucer[0], myPosition[i - 1].position, Quaternion.identity);
        Vector3 myDirection = new Vector3(3, 2, 0);
        myDirection = myDirection.normalized;
        t_saucer.GetComponent<CS_Saucer>().Init(myDirection);
        i = 1;
    }
    private IEnumerator Generate2()
    {
        
        yield return new WaitForSeconds(1.0f);
        //Debug.Log(i);
        GameObject t_saucer = Instantiate(mySaucer[0], myPosition[i - 1].position, Quaternion.identity);
        Vector3 myDirection = new Vector3(3, 2, 0);
        myDirection = myDirection.normalized;
        t_saucer.GetComponent<CS_Saucer>().Init(myDirection);
        i = 1;
    }
}
