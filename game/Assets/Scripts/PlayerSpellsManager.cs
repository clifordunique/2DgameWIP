using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerSpellsManager : MonoBehaviour
{
    public enum MagicSpells { FIRESTRIKE, THUNDERSTRIKE, BOOSTSELF, YSTRIKE };

    public UnityEvent newSpellSelected;
    public int[] currentSpell = new int[2];
    void Awake()
    {
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
        newSpellSelected?.Invoke();
        currentSpell[1] = currentSpell[0];
        currentSpell[0] = newSpell;
        Debug.Log(currentSpell[0]);
        Debug.Log(currentSpell[1]);
    }

    // Use this for initialization



}
