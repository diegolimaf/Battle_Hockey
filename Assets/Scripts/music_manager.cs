using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class music_manager : MonoBehaviour
{
    public AudioSource musics;
    public AudioClip[] audioArray;

    private void Awake()
    {
        musics = GetComponent<AudioSource>();
    }
    private void Start()
    {
        musics.clip = audioArray[Random.Range(0, audioArray.Length)];
        musics.PlayOneShot(musics.clip);
    }
    public void StopMusic()
    {
        musics.Stop();
    }
    public void PauseMusic()
    {
        musics.Pause();
    }
    public void UnpauseMusic()
    {
        musics.UnPause();
    }
}
