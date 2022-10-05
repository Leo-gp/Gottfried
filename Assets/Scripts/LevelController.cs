using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Vector3 playerStartPoint;
    public float playerSize;
    public float playerMoveSpeed;
    public float playerJumpForce;
    public int totalLives;
    public bool doorStartOpened;
    public bool lastLevel;

    public static LevelController instance;

    void Awake ()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
}
