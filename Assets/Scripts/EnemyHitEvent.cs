using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 적이 공격할 때 이벤트가 호출되면 처리할 클래스
public class EnemyHitEvent : MonoBehaviour
{
    // Player 한테 Damage 주고 싶다.
    void OnHit()
    {
        //플레이어의 체력을 -1 깍는다.
        PlayerHealth.Instance.HP--;
        //PlayerHealth.Instance.SetHP(PlayerHealth.Instance.GetHP() - 1);
    }
}
