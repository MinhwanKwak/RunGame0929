using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;
}

public class AudioManager : MonoBehaviour
{
    //bgmmixer 
    public AudioMixer MasterMixer;
    [SerializeField] Sound[] sfxsounds;
    

    [Header("효과음 플레이어")]
    public AudioSource[] sfxPlayer;
    public Slider BgmSlider;
    public Slider SfxSlider;
    

    public static AudioManager Instance;

    private float BGMBeforeAudioSliderValue =  0f;
    private float SFXBeforeAudioSliderValue =  0f;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(Instance);
        }
    }

    private void Start()
    {
        MasterMixer.SetFloat("BGM", PlayerPrefs.GetFloat("BGMGroundSound"));
        BgmSlider.value = PlayerPrefs.GetFloat("BGMGroundSound");

        MasterMixer.SetFloat("SFX", PlayerPrefs.GetFloat("SFXGroundSound"));
        SfxSlider.value = PlayerPrefs.GetFloat("SFXGroundSound");


    }

    public void PlaySoundSfx(string _soundName)
    {
        for(int i = 0;  i < sfxsounds.Length; i++)
        {
            if(_soundName == sfxsounds[i].soundName)
            {
                for(int j = 0; j < sfxPlayer.Length; ++j)
                {
                    if(!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfxsounds[i].clip;
                        sfxPlayer[j].Play();
                        return;
                    }
                }
            }
        }
        Debug.Log("모든 효과음이 재생중 입니다.");
        return;
    }

    private void Update()
    {
        if (BGMBeforeAudioSliderValue != BgmSlider.value)
        {
            BGMBeforeAudioSliderValue = BgmSlider.value;
            BGMAudioController();
        }

        if(SFXBeforeAudioSliderValue != SfxSlider.value)
        {
            SFXBeforeAudioSliderValue = SfxSlider.value;
            SFXAudioController();
        }
    }

    public void BGMAudioController()
    {

        PlayerPrefs.SetFloat("BGMGroundSound", BgmSlider.value);

        float sound = PlayerPrefs.GetFloat("BGMGroundSound");

        if (sound == -40f)
        {
            MasterMixer.SetFloat("BGM", -80);

        }
        else
        {
            MasterMixer.SetFloat("BGM", sound);
        }

    }

    public void SFXAudioController()
    {

        PlayerPrefs.SetFloat("SFXGroundSound", SfxSlider.value);

        float sound = PlayerPrefs.GetFloat("SFXGroundSound");

        if (sound == -40f)
        {
            MasterMixer.SetFloat("SFX", -80);

        }
        else
        {
            MasterMixer.SetFloat("SFX", sound);
        }

    }
}
