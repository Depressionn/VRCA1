using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {
    public Text lmao;
    public Text RPM;
    public Text CastingDistance;
    public Text HookDistance;
    public Canvas statsUI;
    public Canvas CaughtUI;
    public Text fishname;
    public Text fishrarity;
    public Text fishdata;

    // Start is called before the first frame update
    void Start() {
        HideCatchScreen();
    }

    // Update is called once per frame
    void Update() {
        if((int)FishingRod.Instance.CurrentRodState > 2 && !statsUI.enabled)
        {
            statsUI.enabled = true;
        }
        else if((int)FishingRod.Instance.CurrentRodState < 2 && statsUI.enabled)
        {
            statsUI.enabled = false;
        }
    }

    public void SetCastingDistance(float d)
    {
        CastingDistance.text = $"Casting Distance - {(int)d}meters";
    }

    public void SetHookDistance(float d)
    {
        HookDistance.text = $"Hook Distance - {(int)d}meters";
    }

    public void ShowCatchScreen(Fish fish)
    {
        CaughtUI.enabled = true;
        fishname.text = fish.fishName;
        fishrarity.text = fish.rarity.ToString();
        Color color = new Color();
        switch (fish.rarity)
        {
            case FishRarity.COMMON:
                color = new Color(0.6f, 0.6f, 0.6f);
                    break;
            case FishRarity.UNCOMMON:
                color = new Color(0.3f, 0.6f, 0.3f);
                break;
            case FishRarity.RARE:
                color = new Color(0.3f, 0.3f, 0.6f);
                break;
            case FishRarity.LEGENDARY:
                color = new Color(0.6f, 0.6f, 0.3f);
                break;
            case FishRarity.EXOTIC:
                color = new Color(0.8f, 0.2f, 0.2f);
                break;
        }
        fishrarity.color = color;

        string dataString = "";
        dataString += $"Weight:{fish.fishWeight}<br>";
        dataString += $"Size:{fish.fishSize}<br>";
        dataString += $"Colour:{fish.fishColour}<br>";
        dataString += "<br>" + fish.additionalData;
    }

    public void HideCatchScreen()
    {
        CaughtUI.enabled = false;
    }
}
