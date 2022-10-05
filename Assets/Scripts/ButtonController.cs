using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public Sprite pressedSprite;
    public Sprite releasedSprite;
    public GameObject[] targets;

    private SpriteRenderer sr;
    private List<PlayerController> collidingPlayers = new List<PlayerController>();

    void Awake ()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.tag == "Player")
        {
            sr.sprite = pressedSprite;
            collidingPlayers.Add(col.GetComponent<PlayerController>());
            foreach (var target in targets)
            {
                if (target.tag == "Door")
                {
                    target.GetComponent<DoorController>().OpenDoor();
                }
                else if (target.tag == "Ground")
                {
                    for (int i = 0; i < target.transform.childCount; i++)
                    {
                        if (target.transform.GetChild(i).tag == "OpenPlatform")
                            target.gameObject.SetActive(false);
                    }
                }
                else if (target.tag == "OpenPlatform")
                {
                    if (target.transform.parent != null)
                        target.transform.parent.gameObject.SetActive(false);
                    else
                        target.gameObject.SetActive(false);
                }
            }
        }
    }

    void OnTriggerExit2D (Collider2D col)
    {
        if (col.tag == "Player")
        {
            collidingPlayers.Remove(col.GetComponent<PlayerController>());
            if (collidingPlayers.Count == 0)
            {
                sr.sprite = releasedSprite;
                foreach (var target in targets)
                {
                    if (target.tag == "Door")
                    {
                        target.GetComponent<DoorController>().CloseDoor();
                    }
                    else if (target.tag == "OpenPlatform")
                    {
                        target.gameObject.SetActive(true);
                    }
                }
            }
        }
    }
}
