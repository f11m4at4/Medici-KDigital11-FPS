using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임의 상태를 관리하는 객체
public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Intro,
        Playing,
        GameOver
    }

    public GameState m_state;

    public static GameManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool IsIntro()
    {
        return m_state == GameState.Intro;
    }
}
