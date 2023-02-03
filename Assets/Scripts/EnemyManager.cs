using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 지정된 위치들 중에서 랜덤시간 간격에 한번씩 적을 만들고 싶다.
// 필요속성 : 위치들, 랜덤시간 간격, 경과시간
public class EnemyManager : MonoBehaviour
{
    // 필요속성 : 위치들, 랜덤시간 간격, 경과시간, 적 공장
    // 위치들
    public Transform[] spawnPositions;
    // 랜덤시간 간격
    public float minTime = 2;
    public float maxTime = 5;
    // 생성시간
    float createTime;
    // 경과시간
    float currentTime;
    // 적 공장
    public GameObject enemyFactory;


    void Start()
    {
        //  -> 생성시간이 랜덤하게 구해져야 한다.
        createTime = Random.Range(minTime, maxTime);
    }

    // Update is called once per frame
    void Update()
    {
        // 지정된 위치들 중에서 랜덤시간 간격에 한번씩 적을 만들고 싶다.
        // 1. 시간이 흘러야한다.
        currentTime += Time.deltaTime;
        // 2. 생성시간이 됐으니까
        if (currentTime > createTime)
        {
            currentTime = 0;
            // 3. 적을 만들고 싶다.
            GameObject enemy = Instantiate(enemyFactory);
            // -> 지정된 위치들 중에서
            int index = Random.Range(0, spawnPositions.Length);
            enemy.transform.position = spawnPositions[index].position;
            //  -> 생성시간이 랜덤하게 구해져야 한다.
            createTime = Random.Range(minTime, maxTime);
        }
    }
}
