using UnityEngine;

public class SceneMusicPlayer : MonoBehaviour
{

    public AudioClip sceneMusic;

    void Start()
    {
        AudioManager.Instance.PlayMusic(sceneMusic);  
    }


}
