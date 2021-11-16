using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CS_RankList : MonoBehaviour
{
    private static CS_RankList instance = null; //”Ôæ‰∫¨“Â
    public static CS_RankList Instance { get { return instance; } }

    public InputField inputName;
    public Button button;
    public Text myName;
    public Text[] nameList;
    public Text myScore;
    public Text[] scoreList;
    
    [SerializeField] GameObject NameInput;
    [SerializeField] GameObject List;

    public static string[] allName = new string[100];
    public static int[] allScore = new int[100];
    public static int n = 0;
    // Start is called before the first frame update

    /*
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }*/
    void Start()
    {
        //DontDestroyOnLoad(this.gameObject);
        button.GetComponent<Button>().onClick.AddListener(InputOK);
        NameInput.SetActive(false);
        List.SetActive(false);
    }

    void InputOK()
    {
        allName[n] = inputName.text;
        allScore[n] = CS_Player.score;
        n++;
        NameInput.SetActive(false);
        List.SetActive(true);
        SortList();
        ShowList();
    }

    void SortList()
    {
        for (int i = 1; i < n + 1; i++)
        {
            int tmp = allScore[i], j;
            string tmpName = allName[i];
            for (j = i; j > 0 && allScore[j - 1] <= tmp; j--)
            {
                allScore[j] = allScore[j - 1];
                allName[j] = allName[j - 1];
            }
            allScore[j] = tmp;
            allName[j] = tmpName;
        }
    }

    void ShowList()
    {
        for (int i = 0; i < 5; i++)
        {
            nameList[i].enabled = true;
            scoreList[i].enabled = true;
            nameList[i].text = allName[i];
            scoreList[i].text = allScore[i].ToString();
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
