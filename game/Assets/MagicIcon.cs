using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicIcon : MonoBehaviour
{

    public int spellNo;
    Button button;
    private ColorBlock cb;
    void Start()
    {
        button = GetComponent<Button>();
        cb = button.colors;
    }

    // Update is called once per frame
    void Update()
    {
        if (Spells.Instance.currentSpell[0] == spellNo - 1 || Spells.Instance.currentSpell[1] == spellNo - 1)
        {
            cb.normalColor = Color.white;
            button.colors = cb;
            Debug.Log("move:" + spellNo + "ACTIVE");
        }
        else
        {
            cb.normalColor = new Vector4(1f, 1f, 1f, 0.3f);
            button.colors = cb;
        }
    }
}