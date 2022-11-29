using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public string horizontalMoveName = "HorizontalMove";    
    public string jumpButtonName = "Jump";

    public static float HORIZONTALMOVE;    
    public static bool JUMPBUTTON;
    public static bool ATTACKBUTTON;

    private Player playerControls;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        playerControls = ReInput.players.GetPlayer(0);
    }

    
    void Update()
    {
        HORIZONTALMOVE = playerControls.GetAxisRaw(horizontalMoveName);
        
        JUMPBUTTON = playerControls.GetButtonDown(jumpButtonName);

        ATTACKBUTTON = playerControls.GetButtonDown("Attack");
    }
}
