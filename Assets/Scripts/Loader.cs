using UnityEngine;
using System.Collections;

public class Loader : MonoBehaviour {

    public GameObject gameManager;

    void Awake() 
    {
        if (GameManager .Instance ==null) //让gameManager只被实例化一次
        {
            GameObject.Instantiate(gameManager);  //防止每次重新加载关卡时新建gameManager 将gameManager做成Prefab然后实例化
        }
        
    }
	
	
}
