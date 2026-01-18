using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AudioSystem : MonoBehaviour
{

    [SerializeField] AudioSource player;

    [Header("Music")]
    [SerializeField] AudioClip[] musics;

    public static AudioSystem instance;
    
    void Start()
    {
        instance = this;
    }

    public void PlayMusic(int i) 
    {
        player.Stop();
        player.generator = musics[i];
        player.Play();
    }

    public void StopMusic() 
    {
        player.Stop();
    }

}
