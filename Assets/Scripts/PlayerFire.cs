using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//사용자가 발사버튼을 누르면 총구가 향하는 방향으로 총구에서 총알이 발사되도록 하고시프다.
// 필요속성 : 총구, 총알공장
// 0. 탄창을 만들어서 사용하고 싶다.
// -> 필요속성 : 탄창, 탄창 크기
public class PlayerFire : MonoBehaviour
{
    // 필요속성 : 총구, 총알공장
    public Transform firePosition;
    public GameObject bulletFactory;
    // -> 필요속성 : 탄창, 탄창 크기
    public int bulletPoolSize = 10;
    //GameObject[] bulletPool;
    public List<GameObject> bulletPool;
    // Start is called before the first frame update
    void Start()
    {
        // 탄창초기화
        bulletPool = new List<GameObject>();
        // 탄창에 총알을 하나씩 만들어서 넣고 싶다.
        for (int i = 0; i < bulletPoolSize; i++)
        {
            // 총알을 만든다.
            GameObject bullet = Instantiate(bulletFactory);
            // 탄창에 넣는다.
            bulletPool.Add(bullet);
            // 총알을 비활성화 시킨다.
            bullet.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //사용자가 발사버튼을 누르면 총구가 향하는 방향으로 총구에서 총알이 발사되도록 하고시프다.
        // 1. 사용자가 발사버튼을 눌렀으니까
        if (Input.GetButtonDown("Fire1"))
        {
            // 탄창에 총알이 있을 때만 발사하고 싶다.
            if (bulletPool.Count > 0)
            {
                GameObject bullet = bulletPool[0];
                // 3. 총알발사(위치)
                bullet.transform.position = firePosition.position;
                // 방향설정
                bullet.transform.forward = firePosition.forward;
                bullet.SetActive(true);
                // 리스트에서 제거시켜주자
                bulletPool.RemoveAt(0);
            }
        }
    }
}
