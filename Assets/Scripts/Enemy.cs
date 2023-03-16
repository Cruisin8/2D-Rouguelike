using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    private Vector2 targetPosition;
    private Transform player;
    private Rigidbody2D rigidbody;

    public float smoothing = 3;  //平滑度
    public int lossFood = 10;
    public AudioClip attackAudio;

    public int enemyHeart = 10;

    private BoxCollider2D collider;
    private Animator animator;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
        GameManager.Instance.enemyList.Add(this);
    }

    void Update()
    {
        rigidbody .MovePosition ( Vector2.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime));
    }

    public void Move()
    {
        Vector2 offset = player.position - transform.position;
        if (offset .magnitude <1.1&&offset .magnitude >0)
        {
            //敌人攻击 玩家受伤减少食物
            animator.SetTrigger("Attack");
            AudioManager.Instance.RandomPlay(attackAudio);
            player.SendMessage("TakeDamage", lossFood);

        }
        else if (offset .magnitude ==0)
        {
            animator.SetTrigger("Attack");
            AudioManager.Instance.RandomPlay(attackAudio);
            player.SendMessage("TakeDamage", lossFood);

            //敌人受伤 直到被消灭为止 (需要做一个敌人的Animator触发器 EnemyDamage)

        }
        else
        {
            float x = 0, y = 0;
            if (Mathf .Abs (offset.y)>Mathf .Abs (offset .x))
            {
                //如果y轴距离大于x轴 就向y轴移动
                if (offset .y<0)
                {
                    y = -1;
                }
                else
                {
                    y = 1;
                }
            }
            else
            {
                //否则向x轴移动
                if (offset .x>0)
                {
                    x = 1;
                }
                else
                {
                    x = -1;
                } 
            }
            //设置目标位置前 先检测前方有没有东西
            collider.enabled = false ;
            RaycastHit2D hit = Physics2D.Linecast(targetPosition, targetPosition + new Vector2(x, y));
            collider.enabled = true;
            //Debug.Log(hit.transform);        检测敌人碰撞到了什么物体（Debug）
            if (hit .transform ==null )      //如果前面没物体 则移动
            {
                targetPosition += new Vector2(x, y);
            }
            else
            {
                if (hit.collider .tag =="Food"||hit.collider .tag=="Soda")   //前面是食物或者苏打 也可以移动
                {
                    targetPosition += new Vector2(x, y);
                }
            }
            
        }

    }

}
