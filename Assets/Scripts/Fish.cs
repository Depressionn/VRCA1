using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FishRarity
{
    COMMON, UNCOMMON, RARE, EXOTIC, LEGENDARY
}

public class Fish : ScriptableObject
{
    public GameObject fishPrefab;

    [Header("Visuals")]
    public string fishName;
    public string fishWeight;
    public FishRarity rarity;

    [Header("Stats")]
    public float nibbleRate; //Nibbles per second
    public float avgNibble; //Number of nibbles before can reel
    public float aggressiveNess; //0-1 How strong the fish is
    public float chanceToGetAway; //0-1 Chance to get away per nibble
}
