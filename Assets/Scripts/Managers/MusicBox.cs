using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    private int curSceneNum;
    private int lastSongPlayed;
    private int nextSong;
    private AudioClip clip;
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip[] songList; //each scene needs its own list. 

    public float targetValue = 0;
    public float durationValue = 5;
    void Start()
    {
        lastSongPlayed = Random.Range(0, songList.Length);
        StartCoroutine(ActivateMusicbox());
    }

    IEnumerator SongFadeIn(float _endValue, float _duration) //Lerps AudioSource Volume (NOT MIXER) from targetValue to _duration
    {
        float time = 0;
        float startValue = source.volume;

        while (time < _duration)
        {
            source.volume = Mathf.Lerp(startValue, _endValue, time / _duration);
            time += Time.deltaTime;
            yield return null;
        }
        source.volume = _endValue;
    }

    IEnumerator ActivateMusicbox()
    {
        NewSongTime();
        lastSongPlayed = nextSong;
        source.PlayOneShot(songList[nextSong]);
        StartCoroutine(SongFadeIn(targetValue, durationValue));
        yield return new WaitForSeconds(songList[nextSong].length);
        StartCoroutine(ActivateMusicbox());
    }

    private void NewSongTime()
    {
        int songIdx = Random.Range(0, songList.Length);
        if (songIdx == lastSongPlayed)
        {
            NewSongTime();
        }
        else
        {
            nextSong = songIdx;
        }
    }
}
