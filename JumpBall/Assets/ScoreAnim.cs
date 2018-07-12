using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAnim : MonoBehaviour {

    Image numImage;
    public List<Sprite> nums;
    int num;
	
	void Start ()
    {
        numImage = transform.Find("Image").GetComponent<Image>();
        numImage.sprite = nums[num % 10];
        numImage.CrossFadeAlpha(0, 0.5f, false);
        Destroy(gameObject, 1.0f);
    }

    public void SetNumber(int n)
    {
        this.num = n;
    }
	
	void Update ()
    {
        transform.Translate(new Vector3(0, 50*Time.deltaTime, 0));        
	}
}
