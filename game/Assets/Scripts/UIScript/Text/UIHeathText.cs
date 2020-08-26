using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHeathText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private PlayerController player;
    // Start is called before the first frame update
    string text;
    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        text = player.health.GetHealth().ToString() + "/" + player.health.GetMaxHealth().ToString();
        textMesh.text = text;
    }
}
