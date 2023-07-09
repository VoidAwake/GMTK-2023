using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BaristaObject : ScriptableObject
{
    public Sprite Nuetral;
    public Sprite Confused;
    public Sprite Angry;
    public string Identifier;
    public float textSpeed;
    public List<AudioClip> blips;
    public List<AudioClip> music;
}
