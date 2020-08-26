using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpMovesMenu : MonoBehaviour
{
    public GameObject popUpMenu;
    public GameObject defaultSelect;
    public bool paused = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("p"))
        {
            popUpMenu.SetActive(!popUpMenu.activeSelf);
            defaultSelect.GetComponent<Button>().Select();
            if (paused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
                    
        }


    void Pause()
    {
        Time.timeScale = 0;
        paused = true;
    }
    void Resume()
    {
        Time.timeScale = 1;
        paused = false;
    }
    }
