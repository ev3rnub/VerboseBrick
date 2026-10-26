using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionsGUI : MonoBehaviour
{

    public AudioMixer mainAudioMixer;
    public Slider mainVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public StatsManager someStatsManager;

    //define some strings for playerprefs and to save the sliders values. 
    private const string MasterKey = "MasterVolume";
    private const string MusicKey = "MusicVolume";
    private const string SfxKey = "SfxVolume";

    public void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            mainVolumeSlider.value = PlayerPrefs.GetFloat(MasterKey, 1f);    
        }
        else
        {
            PlayerPrefs.SetFloat(MasterKey, 1f);
            PlayerPrefs.Save();
            mainVolumeSlider.value = PlayerPrefs.GetFloat(MasterKey);
        }
        
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicKey);
        }
        else
        {
            PlayerPrefs.SetFloat(MusicKey, 1f);
            PlayerPrefs.Save();
            musicVolumeSlider.value = PlayerPrefs.GetFloat(MusicKey);
        }
        if (PlayerPrefs.HasKey("SfxVolume"))
        {
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(SfxKey, 1f);    
        }
        else
        {
            PlayerPrefs.SetFloat(SfxKey, 1f);
            PlayerPrefs.Save();
            sfxVolumeSlider.value = PlayerPrefs.GetFloat(SfxKey);
        }

        //init our sound on start
        OnMainVolumeChange();
        OnMusicVolumeChange();
        OnSFXVolumeChange();
    }

    public void OnClearHighScores()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        someStatsManager.ClearHighScores();
    }

    public void OnMainVolumeChange()
    {
        PlayerPrefs.SetFloat(MasterKey, mainVolumeSlider.value);
        PlayerPrefs.Save();

        float newVolume = mainVolumeSlider.value;
        if (newVolume <= 0)
        {
            newVolume = -80;
        }
        else
        {
            newVolume = Mathf.Log10(newVolume);
            newVolume = newVolume * 20;  
        }
        mainAudioMixer.SetFloat("MainVolume", newVolume);
    }

    public void OnMusicVolumeChange()
    {
        PlayerPrefs.SetFloat(MusicKey, musicVolumeSlider.value);
        PlayerPrefs.Save();
        float newVolume = musicVolumeSlider.value;
        if (newVolume <= 0)
        {
            newVolume = -80;
        }
        else
        {
            newVolume = Mathf.Log10(newVolume);
            newVolume = newVolume * 20;  
        }
        mainAudioMixer.SetFloat("MusicVolume", newVolume);
    }

    public void OnSFXVolumeChange()
    {
        PlayerPrefs.SetFloat(SfxKey, sfxVolumeSlider.value);
        Debug.Log("SFX VOL CHANGED");
        PlayerPrefs.Save();
        
        float newVolume = sfxVolumeSlider.value;
        if (newVolume <= 0)
        {
            newVolume = -80;
        }
        else
        {
            newVolume = Mathf.Log10(newVolume);
            newVolume = newVolume * 20;  
        }
        mainAudioMixer.SetFloat("SfxVolume", newVolume);
    }
}
