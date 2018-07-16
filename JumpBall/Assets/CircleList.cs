using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleList : MonoBehaviour
{

    public Circle circle;

    List<Circle> list= new List<Circle>();

   

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
    public void GenerateByLevel(int level)
    {
        Init();
        int r = Random.Range(1, 7);
        
        float angle = 0;
        for (int i = 0; i < r; i++)
        {
            Circle c;
            if (i <= list.Count - 1)
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
            float RandomRadian = Random.Range(radian*0.2f, radian*0.8f);
            c.GenerateSingleCircle(RandomRadian,0);
            c.transform.Rotate(0, angle, 0);
            c.transform.position = transform.position;
            angle += c.RealAngle;


        }
        transform.Rotate(0, Random.Range(0, 360), 0);
    }
    [ContextMenu("drop")]
    public void Drop()
    {
        foreach (var c in list)
        {
            if (c.gameObject.activeInHierarchy)
            {
                c.Drop();
            }

        }
        
    }
    void Update()
    {

    }
}
