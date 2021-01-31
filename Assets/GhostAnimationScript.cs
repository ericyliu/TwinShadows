using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimationScript : MonoBehaviour
{
  public Ghost ghost;

  public void EndHit()
  {
    ghost.isHit = false;
  }
}
