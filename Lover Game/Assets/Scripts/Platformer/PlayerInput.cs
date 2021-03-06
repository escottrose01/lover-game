﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;

    public static bool interacting;
    public static bool pausing;
    public static int interactingBufferFrames;
    public static bool Interacting
    {
        get
        {
            bool tmp = interacting;
            interacting = false;
            return tmp;
        }
        private set
        {
            interacting = value;
        }
    }
    public static bool Pausing
    {
        get
        {
            bool tmp = pausing;
            pausing = false;
            return tmp;
        }
        private set
        {
            pausing = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (interactingBufferFrames > 0)
        {
            if (--interactingBufferFrames == 0) interacting = false;
        }

        Vector2 directionalInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump")) player.OnJumpInputDown();
        if (Input.GetButtonUp("Jump")) player.OnJumpInputUp();
        if (Input.GetButton("Fire1")) player.OnFire1Down();
        if (Input.GetButtonDown("Fire2")) player.OnFire2Down();
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (DialogueManager.Instance != null && DialogueManager.Instance.Showing) DialogueManager.Instance.Next();
            else
            {
                Interacting = true;
                interactingBufferFrames = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape) && PauseMenu.Instance != null) PauseMenu.Instance.TogglePause();
    }
}
