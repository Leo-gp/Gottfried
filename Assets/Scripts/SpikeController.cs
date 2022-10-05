using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour
{
    public Sprite defaultSprite;
    public Sprite bleedingSprite;

    private SpriteRenderer sr;

    void Awake ()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnCollisionEnter2D (Collision2D col)
    {
        if (col.transform.tag == "Player")
        {
            PlayerController player = col.transform.GetComponent<PlayerController>();
            player.Kill();
            AudioController.instance.Play("Death_Spike");
        }
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.tag == "Player")
        {
            sr.sprite = bleedingSprite;

        }
    }
}
