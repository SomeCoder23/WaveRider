using UnityEngine;
using UnityEngine.UI;


public class SoundManager : MonoBehaviour
{
    #region Singleton
    public static SoundManager instance;
    private void Awake()
    {

        if (instance != null)
        {
            Debug.LogWarning("More than one Sound Manager!");
            return;
        }

        instance = this;
    }

    #endregion

    public AudioSource musicAudio, soundAudio;
    public AudioClip buttonClick;
    public AudioClip gameMusic, mainMenuMusic;
    [SerializeField]
    private Sprite soundOn, soundOff;
    [SerializeField]
    private Sprite musicOn, musicOff;

    bool sound = true, music = true, paused = false;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Sound"))
            sound = PlayerPrefs.GetInt("Sound") == 0 ? false : true;

        if (PlayerPrefs.HasKey("Music"))
            music = PlayerPrefs.GetInt("Music") == 0 ? false : true;

    }

    //void InitializeAudioSettings()
    //{
    //    soundToggle.image.sprite = sound ? soundOn : soundOff;
    //    musicToggle.image.sprite = music ? musicOn : musicOff;

    //    if (musicAudio != null)
    //        musicAudio.mute = !music;

    //    if (soundAudio != null)
    //    {
    //        soundAudio.mute = !sound;
    //    }
    //}


    public void ToggleSound(Image icon)
    {
        sound = !sound;
        SetSoundPrefs();

        icon.sprite = sound ? soundOn : soundOff;
        soundAudio.mute = !sound;
    }

    public void ToggleMusic(Image icon)
    {
        music = !music;
        SetMusicPrefs();

        icon.sprite = music ? musicOn : musicOff;
        musicAudio.mute = !music;
    }

    public void ClickBtn()
    {
        if (sound)
            soundAudio.PlayOneShot(buttonClick);
    }

    public void PlaySound(AudioClip clip)
    {
        if (sound)
            soundAudio.PlayOneShot(clip);
    }
    public void StopSound()
    {
        soundAudio.Stop();
    }
    public void StopMusic()
    {
        musicAudio.Stop();
    }

    public void PlayMusic()
    {
        if (musicOn)
            musicAudio.Play();
    }

    public void ChangeMusic(AudioClip clip)
    {
        musicAudio.clip = clip;
    }

    void SetSoundPrefs()
    {
        int on = sound ? 1 : 0;
        PlayerPrefs.SetInt("Sound", on);
    }

    void SetMusicPrefs()
    {
        int on = music ? 1 : 0;
        PlayerPrefs.SetInt("Music", on);
    }

    public bool isSoundOn()
    {
        if (!paused)
            return sound;
        else return false;
    }

    public bool isMusicOn()
    {
        if (!paused)
            return music;
        else return false;
    }

    public void Pause()
    {
        paused = true;
        StopMusic();
        StopSound();
    }

    public void UnPause()
    {
        paused = false;
        PlayMusic();
    }
}