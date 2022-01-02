using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Settings : ScriptableObject
{
    [Tooltip("Language selected by the user")]
    public LocalisationSystem.Language currentLanguage;
}
