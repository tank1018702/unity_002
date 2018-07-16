using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleList : MonoBehaviour
{
    public Circle circle;

    List<Circle> list;

    void Start()
    {
        list = new List<Circle>();

    }

    public void Init()
    {
        foreach (var c in list)
        {
            c.transform.SetParent(transform);
            c.StopAllCoroutines();
            c.Init();
        }
    }
    [ContextMenu("Generate")]
    public void GenerateByLevel()
    {
        Init();
        int r = Random.Range(1, 7);
        Debug.Log(r);
        float angle = 0;
        for (int i = 0; i < r; i++)
        {
            Circle c;
            if(i<=list.Count-1)
            {
                c = list[i];
                c.gameObject.SetActive(true);
            }
            else
            {
                c = Instantiate(circle, transform.position, Quaternion.identity, transform);
                c.Init();
                list.Add(c);
            }
            
            float radian = (Mathf.PI * 2) / r;


            float RandomRadian = Random.Range(0, radian);
            c.GenerateCircleByLevel(RandomRadian);

            c.transform.Rotate(0, angle, 0);
            c.transform.position = transform.position;
            angle += c.RealAngle;


        }

    }
    [ContextMenu("drop")]
    public void Drop()
    {
        foreach(var c in list)
        {
            if(c.gameObject.activeInHierarchy)
            {
                c.Drop();
            }
           
        }
    }
    void Update()
    {

    }
}
