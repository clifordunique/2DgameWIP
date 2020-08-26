using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spells : MonoBehaviour
{
    public static Spells Instance { get; private set; }
    public enum MagicSpells { FIRESTRIKE, THUNDERSTRIKE, BOOSTSELF, YSTRIKE };

    public int[] currentSpell = new int[2];
    void Awake()
    {
        Instance = this;
        currentSpell[0] = (int)MagicSpells.FIRESTRIKE;
        currentSpell[1] = (int)MagicSpells.THUNDERSTRIKE;
    }



    public void SelectNewSpell(int newSpell)
    {
        Debug.Log("SELECTING SPELLS");
        if (newSpell == currentSpell[0] || newSpell == currentSpell[1])
        {           
            return;
        }
        currentSpell[1] = currentSpell[0];
        currentSpell[0] = newSpell;
        Debug.Log(currentSpell[0]);
        Debug.Log(currentSpell[1]);
    }

    // Use this for initialization



}
