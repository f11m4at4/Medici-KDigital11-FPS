using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� ���� ������ ���� ������ 1�� �ø��� �ʹ�.
// ���� ���� ������ �ְ������� �Ѿ�� �ְ������� ����������
// ���ŵǰ� �ʹ�.
// �ʿ�Ӽ� : ��������, �ְ�����
// UI �� ������ ǥ���ϰ� �ʹ�.
// �ʿ�Ӽ� : UIs
public class ScoreManager : MonoBehaviour
{
    // �ʿ�Ӽ� : UIs
    public Text curScoreUI;
    public Text topScoreUI;

    // �ʿ�Ӽ� : ��������, �ְ�����
    private int _curScore;
    private int _topScore;

    public int CurScore
    {
        get
        {
            return _curScore;
        }
        set
        {
            _curScore = value;
            curScoreUI.text = "Score : " + _curScore;
            // ���� ���� ������ �ְ������� �Ѿ��
            if (_curScore > _topScore)
            {
                // �ְ������� ���������� ���ŵǰ� �ʹ�.
                _topScore = _curScore;
                topScoreUI.text = "Top Score : " + _topScore;

                // �ְ����� ���� ��ġ
                PlayerPrefs.SetInt("TopScore", _topScore);
            }
        }
    }

    public static ScoreManager Instance;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        // ���嵥���� �ҷ�����
        _topScore = PlayerPrefs.GetInt("TopScore", 0);
        // UI �� �ְ����� ǥ�����ֱ�
        topScoreUI.text = "Top Score : " + _topScore;
    }
}
