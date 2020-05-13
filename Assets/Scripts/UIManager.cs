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

    // Start is called before the first frame update
    void Start() {

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

    
}
