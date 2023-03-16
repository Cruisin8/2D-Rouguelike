using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance
    {
        get 
        {
            return _instance;
        }
    }
    public int level = 1;//当前关卡
    public int food = 100;
    public AudioClip diedClip;

    [HideInInspector]public List<Enemy> enemyList = new List<Enemy>();

    [HideInInspector]public bool isEnd = false;     //是否到达终点

    private bool sleepStep = true;
    private Text foodText;
    private Text failText;
    private Image dayImage;
    private Text dayText;
    private Player player;
    private MapManager mapManager;
    
    void Awake() 
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);     //重新加载关卡时不会销毁这个gameObject
        InitGame();
    }

    void InitGame() 
    {
        //初始化地图
        mapManager = GetComponent<MapManager>();    //防止重新加载关卡时重新加载游戏 在gameManager里初始化地图
        mapManager.InitMap();

        //初始化UI
        foodText = GameObject.Find("FoodText").GetComponent<Text>();
        UpdateFoodText(0);
        failText = GameObject.Find("FailText").GetComponent<Text>();
        failText.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        dayImage = GameObject.Find("DayImage").GetComponent<Image>();
        dayText = GameObject.Find("DayText").GetComponent<Text>();
        dayText.text = "Day " + level;
        Invoke("HideBlack", 1);

        //初始化参数
        isEnd = false;
        enemyList.Clear();
    }

    void UpdateFoodText(int foodChange) 
    {
        if (foodChange ==0)
        {
            foodText.text = "Food:" + food;
        }
        else
        {
            string str = "";
            if (foodChange <0)
            {
                str = foodChange.ToString();
            }
            else
            {
                str = "+" + foodChange;
            }   
            foodText.text = "Food:" + food + "  " + str;
        }
        
    }

    public void ReduceFood(int count){
        food -= count;
        UpdateFoodText(-count);
        if (food <=0)
        {
            failText.enabled = true;
            AudioManager.Instance.StopBgMusic();
            AudioManager.Instance.RandomPlay(diedClip);

            
        }
    }
    public void AddFood(int count){
        food += count;
        UpdateFoodText(count);
    }
    
    //主角走两步 敌人走一步
    public void OnPlayerMove() {
        if (sleepStep==true  )
        {
            sleepStep = false;
        }
        else
        {
            foreach (var enemy in enemyList) 
            {
                enemy.Move();            
            }
            sleepStep = true;
        } 
        //检测有没有到达终点
        if (player .targetPos.x==mapManager .cols -2&&player .targetPos .y==mapManager .rows-2)
        {
            isEnd = true;
            //加载下一个关卡
            Application.LoadLevel(Application.loadedLevel);   //重新加载本关卡

        }
    }
    void OnLevelWasLoaded(int sceneLevel)       //unity自带函数  当关卡重新加载时调用
    {
        level++;
        InitGame();//初始化游戏
    }

    void HideBlack() 
    {
        dayImage.gameObject.SetActive(false);
    }
   
}
