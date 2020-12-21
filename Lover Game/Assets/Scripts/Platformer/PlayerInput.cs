using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class PlayerInput : MonoBehaviour
{
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetButtonDown("Jump")) player.OnJumpInputDown();
        if (Input.GetButtonUp("Jump")) player.OnJumpInputUp();
        if (Input.GetButton("Fire1")) player.OnFireDown();
        if (Input.GetKeyDown(KeyCode.E)) DialogueManager.Instance?.Next();
    }
}
