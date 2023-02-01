using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 위치값을 가진 배열을 선언

public class SceneCamMove : MonoBehaviour
{
    public GameObject mainCamera;
    public Transform[] checkPoint;
    int curIndex;

    public float moveSpeed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // 게임 상태를 Playing 으로 만들어주기
    void GotoPlaying()
    {
        mainCamera.SetActive(true);
        GameManager.Instance.m_state = GameManager.GameState.Playing;
        // SceneCamera 는 비활성화 시켜주자
        gameObject.SetActive(false);
    }

    void CamMoveUsingScript()
    {
        // 1. 타겟 위치로 이동하고 싶다.
        // from -> to, 속도
        Vector3 target = checkPoint[curIndex].position;
        transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);

        transform.forward = Vector3.Lerp(transform.forward, checkPoint[curIndex].forward, moveSpeed * Time.deltaTime);
        // 2. 타겟과의 거리가 일정범위안에 들어오면
        if (Vector3.Distance(target, transform.position) < 1)
        {
            // 값을 그 위치로 할당
            transform.position = target;
            // -> 3. 다음 위치로 설정하고 싶다.
            curIndex++;
        }
    }
}
