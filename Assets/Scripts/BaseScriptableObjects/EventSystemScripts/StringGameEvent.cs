using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class StringGameEvent : GameEvent
{
    private string stringOutput;

    public string GetString()
    {
        return stringOutput;
    }

    public void SetString(string newString)
    {
        stringOutput = newString;
    }
}
