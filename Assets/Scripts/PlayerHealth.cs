using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private int hp = 3;

    public static PlayerHealth Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    public int GetHP()
    {
        return hp;
    }
    // hp 값을 설정하는 기능
    public void SetHP(int value)
    {
        hp = value;
        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }

}
