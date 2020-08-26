using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public int level;
    public SpawnManager(int currentLevel)
    {
        level = currentLevel;
    }
    public List<GameObject> Monsters = new List<GameObject>();

    [SerializeField]
    private GameObject monster1;
    [SerializeField]
    private GameObject monster2;
    [SerializeField]
    private GameObject monster3;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
