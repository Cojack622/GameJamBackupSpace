using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField]
    private MusicLibrary MusicLibrary;
    [SerializeField]
    private AudioSource MusicSource;

    [SerializeField]
    private int MinPauseTime;
    [SerializeField]
    private int PauseDiff;


    private int trackSwitchNo = 0;
    private bool paused = false;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        } else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Update()
    {
        if (!MusicSource.isPlaying && !paused && Application.isFocused)
        {
            StartCoroutine(PauseThenPlay());
        }
    }

    public void PlayMusic(string trackName, float fadeDuration)
    {
        StartCoroutine(CrossfadeMusic(MusicLibrary.GetMusicFromName(trackName), fadeDuration));
    }

    IEnumerator PauseThenPlay()
    {
        Debug.Log("Tracks");
        paused = true;
        yield return new WaitForSeconds((float)MinPauseTime + Random.value * PauseDiff);
        StartCoroutine(CrossfadeMusic(MusicLibrary.GetMusicFromName(TrackString()), 0.2f));
        IncrementTrack();
        yield return new WaitForSeconds(0.5f);
        paused = false;
    }

    public string TrackString()
    {
        return ("Level" + (trackSwitchNo+1).ToString());
    }

    public void IncrementTrack()
    {
        if (trackSwitchNo < MusicLibrary.Playlist.Length - 1)
        {
            trackSwitchNo++;
        }
        else
        {
            trackSwitchNo = 0;
        }
    }

    IEnumerator CrossfadeMusic(AudioClip nextTrack, float fadeDuration = 2f )
    {
        float percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            MusicSource.volume = Mathf.Lerp(0.5f, 0, percent);
            yield return null;
        }

        MusicSource.clip = nextTrack;
        MusicSource.Play();

        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            MusicSource.volume = Mathf.Lerp(0, 0.5f, percent);
            yield return null;
        }
    }
}
