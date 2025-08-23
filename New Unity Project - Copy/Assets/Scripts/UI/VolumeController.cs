using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour, IDataPersistence
{

    public AudioMixer musicMixer;
    public AudioMixer effectsMixer;

    public Slider musicSlider;
    public Slider effectsSlider;

    public string musicVolume;
    public string effectsVolume;

    public void OnEnable()
    {
        DataPersistenceManager.instance.LoadGame();
    }

    public void SetMusicVolume(float sliderValue)
    {
        musicMixer.SetFloat(musicVolume, Mathf.Log10(sliderValue) * 20);
        DataPersistenceManager.instance.SaveGame();

    }

    public void SetEffectsVolume(float sliderValue)
    {
        effectsMixer.SetFloat(effectsVolume, Mathf.Log10(sliderValue) * 20);
        DataPersistenceManager.instance.SaveGame();
    }

    public void SetMusicVolumeInternal(float value)
    {
        effectsMixer.SetFloat(musicVolume, value);
        musicSlider.value = Mathf.Pow(10, value/20);
    }

    public void SetEffectsVolumeInternal(float value)
    {
        effectsMixer.SetFloat(effectsVolume, value);
        effectsSlider.value = Mathf.Pow(10, value/20);
    }



    public void LoadData(GameData data)
    {
        SetMusicVolumeInternal(data.musicVolume);
        SetEffectsVolumeInternal(data.effectsVolume);
    }

    public void SaveData(GameData data)
    {
        if (this)
        {
            musicMixer.GetFloat(musicVolume, out float saver);
            effectsMixer.GetFloat(effectsVolume, out float saverTwo);
            if (!float.IsNaN(saver))
            {
                data.musicVolume = saver;
                data.effectsVolume = saverTwo;
            }                
        }
    }
}
