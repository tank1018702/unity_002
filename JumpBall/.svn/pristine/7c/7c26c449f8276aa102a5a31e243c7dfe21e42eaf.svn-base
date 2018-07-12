using UnityEngine;

public class SparksExecutioner : MonoBehaviour 
{
    public float Lifetime = 1.0f;

	// Use this for initialization
	private void Start () 
    {
        Invoke("Kill", Lifetime);
	}	

    private void Kill()
    {
        Destroy(this.gameObject);
    }
}
