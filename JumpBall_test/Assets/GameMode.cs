using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMode : MonoBehaviour
{
    public Circle PrefabCircle;
    public Transform Root;

    LinkedList<Circle> CircleQueue;
    LinkedListNode<Circle> curNode;

    public float gap = 8.0f;
    float lowestCircleY;

    Transform cam;
    Transform pillar;

    Ball ball;
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
        if (next.Value.transform.position.y > lowestCircleY + 2f * gap)
        {
            next.Value.gameObject.SetActive(false);
        }
        //如果链表的下一个环在场景中是隐藏的就返回这个环并在场景中显示
        if (!next.Value.gameObject.activeInHierarchy)
        {
            curNode = next;
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
        ball = GameObject.Find("ball").GetComponent<Ball>();

        CircleQueue = new LinkedList<Circle>();

        CircleQueue.AddLast(GetNewCircle());
        curNode = CircleQueue.Last;
    }
	
	
	void Update ()
    {
        while (lowestCircleY + gap > ball.transform.position.y)
        {
            var circle = GetNextCircle();
            //每次得到新的圆环时改变高度
            circle.transform.position = new Vector3(0, lowestCircleY - gap);
            circle.GenerateCircleByLevel();
            lowestCircleY = circle.transform.position.y;
            
        }

        pillar.position = new Vector3(pillar.position.x, cam.position.y, pillar.position.z);
    }
}
