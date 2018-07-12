using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    SphereCollider sphereCollider;

    public float bounceSpeed;
    public float gravity;
    public float maxSpeed = 1;

    float radius = 0;
    float speed;

    void Start ()
    {
        sphereCollider = GetComponent<SphereCollider>();
        radius = sphereCollider.radius;

	}
    //球掉落移动计算
    void Drop()
    {
        //每帧减去模拟重力带来的速度影响
        speed -= gravity * Time.deltaTime;
        //限制球能达到的最大速度
        speed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);

        transform.position += new Vector3(0, speed, 0);
    }
    void Update()
    {
        Bounce();
        Drop();

    }

    void Bounce()
    {
        //当球反弹回升的时候跳过检测
        if (speed >= 0)
        {
            return;
        }

        //确定一个位置合适的立方体，比球略小，位置偏下
        Vector3 p = transform.position + new Vector3(0, -radius, 0);
        Vector3 size = new Vector3(radius * 0.5f, radius * 0.5f, radius * 0.5f);
        if (Physics.OverlapBox(p, size, Quaternion.identity, LayerMask.GetMask("Ground")).Length > 0)
        {        
            speed = bounceSpeed;      
        }

    }
   
   
}
