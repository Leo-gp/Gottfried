using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Sprite openedSprite;
    public Sprite closedSprite;

    private SpriteRenderer sr;

    private bool opened;

    void Awake ()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start ()
    {
        if (LevelController.instance.doorStartOpened)
            opened = true;
        if (opened)
            sr.sprite = openedSprite;
        else
            sr.sprite = closedSprite;
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.tag == "Player" && col.GetComponent<PlayerController>().enabled && opened)
        {
            GameController.instance.LevelCompleted();
        }
    }

    public void OpenDoor ()
    {
        opened = true;
        sr.sprite = openedSprite;
    }

    public void CloseDoor()
    {
        opened = false;
        sr.sprite = closedSprite;
    }
}
