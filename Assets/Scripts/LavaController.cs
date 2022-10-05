using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaController : MonoBehaviour
{

    void OnTriggerEnter2D (Collider2D col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerController>().Kill();
            AudioController.instance.Play("Death_Lava");
            Destroy(col.gameObject, 1);
        }
    }
}
