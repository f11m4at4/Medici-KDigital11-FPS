using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 적을 잡을 때마다 현재 점수를 1씩 올리고 싶다.
// 만약 현재 점수가 최고점수를 넘어서면 최고점수는 현재점수로
// 갱신되고 싶다.
// 필요속성 : 현재점수, 최고점수
// UI 에 점수를 표시하고 싶다.
// 필요속성 : UIs
public class ScoreManager : MonoBehaviour
{
    // 필요속성 : UIs
    public Text curScoreUI;
    public Text topScoreUI;

    // 필요속성 : 현재점수, 최고점수
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
            // 만약 현재 점수가 최고점수를 넘어서면
            if (_curScore > _topScore)
            {
                // 최고점수는 현재점수로 갱신되고 싶다.
                _topScore = _curScore;
                topScoreUI.text = "Top Score : " + _topScore;

                // 최고점수 저장 위치
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
        // 저장데이터 불러오기
        _topScore = PlayerPrefs.GetInt("TopScore", 0);
        // UI 에 최고점수 표시해주기
        topScoreUI.text = "Top Score : " + _topScore;
    }
}
