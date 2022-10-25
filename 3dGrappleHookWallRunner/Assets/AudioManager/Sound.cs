using UnityEngine.Audio;
using UnityEngine;
[CreateAssetMenu(fileName ="New Sound",menuName ="Sounds/New Sound")]
public class Sound : ScriptableObject
{
    public AudioClip clip;

    public string name;

    [Range(0f,1f)]
    public float volume;
    [Range(.1f,3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}
