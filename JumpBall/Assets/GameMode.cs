using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMode : MonoBehaviour
{

   
    public Circle PrefabCircle;

    public Transform Root;
    public float gap = 8.0f;

    LinkedList<Circle> CircleQueue;
    LinkedListNode<Circle> curNode;
    LinkedListNode<Circle> DropNode;
    float lowestCircleY;

    Transform cam;
    Transform pillar;
    BouncingBall ball;
    Transform canvas;

    public  GameObject GameOverUI;

    int level;

    public GameObject prefabNumber;

    // gameplay
    int totalScore = 0;
    Text totalScoreText;
    Text score;
    int combo;
    float lastAddScoreTime;

    Circle GetNewCircle()
    {
        Circle circle = Instantiate(PrefabCircle, Root);
        circle.Init();
        return circle;
    }

    

    Circle GetNextCircle()
    {
        // 让链表循环使用的算法
        LinkedListNode<Circle> next = curNode.Next;
        if (next == null)
        {
            // 如果达到了结尾就回到开头 
            next = CircleQueue.First;
        }
        //如果圆环太高就隐藏等待复用
        //加入了掉落特效就不需要高度隐藏了
        //if(next.Value.transform.position.y>lowestCircleY+2f*gap)
        //{
        //    next.Value.gameObject.SetActive(false);
        //}
        //如果链表的下一个环在场景中是隐藏的就返回这个环并在场景中显示
        if (!next.Value.gameObject.activeInHierarchy)
        {
            curNode = next;
            curNode.Value.RigReset();
            curNode.Value.gameObject.SetActive(true);
        }
        //如果下一个环在场景中显示,生成新的环并添加到链表next之前
        else
        {
            curNode = CircleQueue.AddBefore(next, GetNewCircle()); 
        }
        
        return curNode.Value;
    }
    
	void Start ()
    {
        cam = Camera.main.transform;
        pillar = GameObject.Find("Pillar").transform;
        ball = GameObject.Find("Ball").GetComponent<BouncingBall>();
        canvas = GameObject.Find("Canvas").transform;
        totalScoreText = GameObject.Find("FinalScore").GetComponent<Text>();
        score = GameObject.Find("Score").GetComponent<Text>();
        CircleQueue = new LinkedList<Circle>();
        GameOverUI.SetActive(false);

        //初始化两个圆环避免开始重复计算分数
        CircleQueue.AddLast(GetNewCircle());
        curNode = CircleQueue.Last;
        DropNode = CircleQueue.Last;
      
        curNode.Value.GenerateCircleByLevel(level);
        curNode.Value.transform.position = Vector3.zero;
        lowestCircleY = curNode.Value.transform.position.y;

        CircleQueue.AddLast(GetNewCircle());
        curNode = CircleQueue.Last;
        curNode.Value.GenerateCircleByLevel(level);
        curNode.Value.transform.position = new Vector3(0, lowestCircleY - gap);
        lowestCircleY = curNode.Value.transform.position.y;


    }
  
        

    void AddScore()
    {
        if (ball.lastBounceTime > lastAddScoreTime)
        {
            
            combo = 1;
        }
        else
        {
            combo++;
            if (combo > 9) { combo = 9; }
        }
        lastAddScoreTime = Time.time;
        
        var numObj = Instantiate(prefabNumber, canvas);
        numObj.GetComponent<ScoreAnim>().SetNumber(combo);
        totalScore += combo;
        level = totalScore / 100;
        totalScoreText.text = string.Format("Score:{0}", totalScore);
    }

    public  void GameOver()
    {
        Time.timeScale = 0;
        GameOverUI.SetActive(true);
        score.text = score.text + totalScore.ToString();
    }

    public void ReStart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
    }


    void Update ()
    {
		while (lowestCircleY+gap >ball.transform.position.y)
        {
             var circle = GetNextCircle();
            //每次得到新的圆环时改变高度
            circle.transform.position = new Vector3(0, lowestCircleY - gap);
            circle.GenerateCircleByLevel(level);
            lowestCircleY = circle.transform.position.y;

            AddScore();
            DropNode.Value.test2();
            DropNode = DropNode.Next ?? CircleQueue.First;
        }
        pillar.position = new Vector3(pillar.position.x, cam.position.y, pillar.position.z);
	}
}
