using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public float smoothing = 1;         //平滑度
    public float restTime = 1;          //每一帧加一个休息时间 防止惯性
    public AudioClip chop1Audio;
    public AudioClip chop2Audio;
    public AudioClip step1Audio;
    public AudioClip step2Audio;
    public AudioClip food1Audio;
    public AudioClip food2Audio;
    public AudioClip soda1Audio;
    public AudioClip soda2Audio;

    private  float restTimer = 0;

    [HideInInspector]public Vector2 targetPos = new Vector2(1, 1);      //目标位置
    private Rigidbody2D rigidbody;
    private BoxCollider2D collider;
    private Animator animator;
    
	// Use this for initialization
    void Start()
    {

        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        rigidbody.MovePosition(Vector2.Lerp(transform.position, targetPos, smoothing * Time.deltaTime));    //deltaTime:时间间隔

        if (GameManager.Instance.food <= 0 || GameManager.Instance.isEnd == true) return;

        restTimer += Time.deltaTime;
        if (restTimer < restTime) return;

        float h = Input.GetAxisRaw("Horizontal");          //水平方向按键
        float v = Input.GetAxisRaw("Vertical");            //垂直方向按键
        if (h > 0) 
        {
            v = 0;
        }

        if (h != 0 || v != 0)
        {
            GameManager.Instance.ReduceFood(1);    //每走一步食物减一

            //检测是否碰撞到物体
            collider .enabled=false ;      //禁用自身碰撞器 防止碰到自身
            RaycastHit2D hit = Physics2D.Linecast(targetPos, targetPos + new Vector2(h, v)); //检测前方有没有东西
            collider.enabled=true ;
            if (hit.transform == null) 
            {
                targetPos += new Vector2(h, v);
                AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
            }
            else
                switch (hit.collider.tag) 
                { 
                    case"OutWall":
                        break;
                    case "Wall":
                        animator.SetTrigger("Attack");
                        AudioManager.Instance.RandomPlay(chop1Audio, chop2Audio);
                        hit.collider.SendMessage("TakeDamage");
                        break;
                    case "Food":
                        GameManager.Instance.AddFood(5);
                        targetPos += new Vector2(h, v);
                        AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
                        Destroy(hit.transform.gameObject);
                        AudioManager.Instance.RandomPlay(food1Audio, food2Audio);
                        break;
                    case "Soda":
                        GameManager.Instance.AddFood(10);
                        targetPos += new Vector2(h, v);
                        AudioManager.Instance.RandomPlay(step1Audio, step2Audio);
                        Destroy(hit.transform.gameObject);
                        AudioManager.Instance.RandomPlay(soda1Audio, soda2Audio);
                        break;
                    case "Enemy":
                        break;
                }

            GameManager.Instance.OnPlayerMove();
            restTimer = 0;   //不管是攻击还是移动 都需要休息

            
        }
	}
    public void TakeDamage(int lossFood)
    {
        GameManager.Instance.ReduceFood(lossFood);
        animator.SetTrigger("Damage");
    }
}
