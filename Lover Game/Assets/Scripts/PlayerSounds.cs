using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public void PlayFootstep()
    {
        AudioManager.Instance.PlayFootstep();
    }

    public void PlayQuietFootstep()
    {
        AudioManager.Instance.PlayQuietFootstep();
    }
}
