using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lake : Singleton<Lake>
{
    public List<Fish> fishes;
    FishingRod fishingRod;
    [Range(0.0f, 1.0f)]
    public float luck;

    private int fishIndex = 0;
    public float timer;
    private int numberOfNibblesToBite = 0;
    public Fish CurrentFish { get { return fishes[fishIndex]; } }

    void Start()
    {
        fishingRod = FindObjectOfType<FishingRod>();
        fishingRod.WaitForBite += WaitForBite;
        fishingRod.reel.eOnSpinReel += DisturbFish;
    }

    public void WaitForBite()
    {
        Debug.Log("WaitForBite");
        //Sort Fish at Random
        fishes.Shuffle();
        fishIndex = 0;
        timer = Random.Range(3f - luck, 10f - luck * 2);
        numberOfNibblesToBite = Random.Range(CurrentFish.avgNibble - 2, CurrentFish.avgNibble + 2);
    }

    private void Nibble()
    {
        numberOfNibblesToBite--;
        fishingRod.Nibble(CurrentFish.nibbleStrength * 0.4f + 0.2f);
        timer = 1 / CurrentFish.nibbleRate;
        Debug.Log("Nibble");
    }

    private void Bite()
    {
        fishingRod.Bite(CurrentFish);
        Debug.Log("Bite");
    }

    public void NextFish()
    {
        fishIndex++;
        if(fishIndex >= fishes.Count/2)
        {
            fishIndex = 0;
            fishes.Shuffle();
        }
        timer = Random.Range(3f - luck, 10f - luck * 2);
        numberOfNibblesToBite = Random.Range(CurrentFish.avgNibble - 2, CurrentFish.avgNibble + 2);
    }

    public void DisturbFish(float ammount)
    {
        if(fishingRod.CurrentRodState == RodState.WaitingForBite)
        {
            NextFish();
        } else if (fishingRod.CurrentRodState == RodState.Biting)
        {
            fishingRod.CanReelIn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            if (fishingRod.CurrentRodState == RodState.WaitingForBite)
            {
                if (numberOfNibblesToBite > 0)
                {
                    Nibble();
                } else
                {
                    Bite();
                }
            }
        }
    }
}
