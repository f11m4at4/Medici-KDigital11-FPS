using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ ��ġ�� �߿��� �����ð� ���ݿ� �ѹ��� ���� ����� �ʹ�.
// �ʿ�Ӽ� : ��ġ��, �����ð� ����, ����ð�
public class EnemyManager : MonoBehaviour
{
    // �ʿ�Ӽ� : ��ġ��, �����ð� ����, ����ð�, �� ����
    // ��ġ��
    public Transform[] spawnPositions;
    // �����ð� ����
    public float minTime = 2;
    public float maxTime = 5;
    // �����ð�
    float createTime;
    // ����ð�
    float currentTime;
    // �� ����
    public GameObject enemyFactory;


    void Start()
    {
        //  -> �����ð��� �����ϰ� �������� �Ѵ�.
        createTime = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        // ������ ��ġ�� �߿��� �����ð� ���ݿ� �ѹ��� ���� ����� �ʹ�.
        // 1. �ð��� �귯���Ѵ�.
        currentTime += Time.deltaTime;
        // 2. �����ð��� �����ϱ�
        if (currentTime > createTime)
        {
            currentTime = 0;
            // 3. ���� ����� �ʹ�.
            GameObject enemy = Instantiate(enemyFactory);
            // -> ������ ��ġ�� �߿���
            int index = Random.Range(0, spawnPositions.Length);
            enemy.transform.position = spawnPositions[index].position;
            //  -> �����ð��� �����ϰ� �������� �Ѵ�.
            createTime = Random.Range(minTime, maxTime);
        }
    }
}
