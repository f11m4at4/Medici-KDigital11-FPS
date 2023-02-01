using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��ġ���� ���� �迭�� ����

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

    // ���� ���¸� Playing ���� ������ֱ�
    void GotoPlaying()
    {
        mainCamera.SetActive(true);
        GameManager.Instance.m_state = GameManager.GameState.Playing;
        // SceneCamera �� ��Ȱ��ȭ ��������
        gameObject.SetActive(false);
    }

    void CamMoveUsingScript()
    {
        // 1. Ÿ�� ��ġ�� �̵��ϰ� �ʹ�.
        // from -> to, �ӵ�
        Vector3 target = checkPoint[curIndex].position;
        transform.position = Vector3.Lerp(transform.position, target, moveSpeed * Time.deltaTime);

        transform.forward = Vector3.Lerp(transform.forward, checkPoint[curIndex].forward, moveSpeed * Time.deltaTime);
        // 2. Ÿ�ٰ��� �Ÿ��� ���������ȿ� ������
        if (Vector3.Distance(target, transform.position) < 1)
        {
            // ���� �� ��ġ�� �Ҵ�
            transform.position = target;
            // -> 3. ���� ��ġ�� �����ϰ� �ʹ�.
            curIndex++;
        }
    }
}
