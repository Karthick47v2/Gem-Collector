using UnityEngine;

public class GameManager : MonoBehaviour{

    public static GameManager Instance { get; private set; }

    void Singletone(){
        if (Instance == null)
            Instance = this;
        else Destroy(gameObject);
    }

    void Awake(){
        Singletone();
        SetTargetFPS();
    }

    void SetTargetFPS(){
        const int TARGET_FRAME_RATE = 120;
        Application.targetFrameRate = TARGET_FRAME_RATE;
    }
}