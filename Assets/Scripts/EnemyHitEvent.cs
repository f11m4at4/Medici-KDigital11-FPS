using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������ �� �̺�Ʈ�� ȣ��Ǹ� ó���� Ŭ����
public class EnemyHitEvent : MonoBehaviour
{
    // Player ���� Damage �ְ� �ʹ�.
    void OnHit()
    {
        //�÷��̾��� ü���� -1 ��´�.
        PlayerHealth.Instance.HP--;
        //PlayerHealth.Instance.SetHP(PlayerHealth.Instance.GetHP() - 1);
    }
}
