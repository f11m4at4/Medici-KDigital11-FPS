using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������� ���콺 �Է¿� ���� ��ü�� ȸ����Ű�� �ʹ�.
// �ʿ�Ӽ� : ȸ���ӵ�
// ����ȸ���� -60 ~ 60�� ������ ȸ���Ƿ� �ϰ� �ʹ�.
public class CamRotate : MonoBehaviour
{
    // �ʿ�Ӽ� : ȸ���ӵ�
    public float rotSpeed = 200;
    float mx;
    float my;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ������� ���콺 �Է¿� ���� ��ü�� ȸ����Ű�� �ʹ�.
        // 1. ���콺�Է�����
        float h = Input.GetAxis("Mouse X");
        float v = Input.GetAxis("Mouse Y");
        // 2. ������ �ʿ�
        mx += h * rotSpeed * Time.deltaTime;
        my += v * rotSpeed * Time.deltaTime;
        // ����ȸ���� -60 ~ 60�� ������ ȸ���Ƿ� �ϰ� �ʹ�.
        my = Mathf.Clamp(my, -60, 60);
        transform.eulerAngles = new Vector3(-my, mx, 0);
    }
}
