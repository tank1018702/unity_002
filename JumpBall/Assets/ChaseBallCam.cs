using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseBallCam : MonoBehaviour
{

    Transform ball;
    float baseY;
    float camOffsetY;

    void Start ()
    {
        ball = GameObject.FindGameObjectWithTag("Player").transform;

        camOffsetY = ball.transform.position.y - transform.position.y;
    }
	
	void Update ()
    {
        //当球的位置低于偏移值时让摄像机和球一起动
		float diffY = ball.transform.position.y - transform.position.y - camOffsetY;
        if (diffY < 0)
        {
            // 比必要的位置再低一些，可以防止抖动
            transform.position += new Vector3(0, diffY -0.15f, 0);
        }
    }
}
