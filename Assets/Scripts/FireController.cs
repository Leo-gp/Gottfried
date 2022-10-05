using UnityEngine;

public class FireController : MonoBehaviour
{
    public float fireWaitTime;

    public AnimationClip fireAnimation;

    private Animator animator;

	void Start ()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating("Fire", fireWaitTime, fireAnimation.length + fireWaitTime);
	}

    void Fire ()
    {
        animator.SetTrigger("Fire");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerController>().Kill();
            AudioController.instance.Play("Death_Lava");
            Destroy(col.gameObject);
        }
    }
}