using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {

    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get { return _instance; }
    }
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;

    public AudioSource efxSource; //音效
    public AudioSource bgSource; 

    void Awake() 
    {
        _instance = this;
    }

    public void RandomPlay(params AudioClip[] clips)   //随机播放两个音效中的一个 随机播放音效速度(Pitch)
    {
        float pitch = Random.Range(minPitch, maxPitch);
        int index = Random.Range(0, clips.Length);   //随机取得一个索引
        AudioClip clip = clips[index];
        efxSource.clip = clip;
        efxSource.pitch = pitch;
        efxSource.Play(); //音效播放
    }

    public void StopBgMusic() 
    {
        bgSource.Stop();
    }
}
