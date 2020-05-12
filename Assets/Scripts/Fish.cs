using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FishRarity
{
    COMMON, UNCOMMON, RARE, EXOTIC, LEGENDARY
}

[CreateAssetMenu(fileName = "New Fish", menuName = "Fish")]
public class Fish : ScriptableObject
{
    public GameObject fishPrefab;
    [Header("Visuals")]
    public string fishName;
    public string fishWeight;
    public FishRarity rarity;

    [Header("Stats")]
    [Range(0.0f, 1.0f)]
    public float nibbleStrength; //How strong the nibble is
    [Range(0.2f, 1.0f)]
    public float nibbleRate; //Nibbles per second
    public int avgNibble; //Number of nibbles before can reel
    [Range(0.0f, 1.0f)]
    public float aggressiveNess; //0-1 How strong the fish is
    [Range(0.0f, 0.8f)]
    public float chanceToGetAway; //0-1 Chance to get away per nibble
}
