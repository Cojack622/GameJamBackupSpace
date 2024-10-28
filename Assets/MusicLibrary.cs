using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public struct MusicTrack
{
    public string trackName;
    public AudioClip musicTrack;
};

public class MusicLibrary : MonoBehaviour
{
    public MusicTrack[] Playlist;

    public AudioClip GetMusicFromName(string musicName) {
        foreach (MusicTrack curTrack in Playlist) {
            if (curTrack.trackName.Equals(musicName)) {
                Debug.Log($"Current Track: {curTrack.trackName}");
                return (curTrack.musicTrack);
            }
        }
        return null;
    }
}
