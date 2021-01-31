using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
  public Vector2 xBounds = new Vector2(-1f, 1f);
  public Vector2 yBounds = new Vector2(-1f, 1f);
  public Vector2 zBounds = new Vector2(-.5f, .5f);
  public float speed = 5f;
  public Vector3 destination = Vector3.zero;

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    if (Vector3.Equals(destination, Vector3.zero))
    {
      destination = new Vector3(Random.Range(xBounds.x, xBounds.y), Random.Range(yBounds.x, yBounds.y), Random.Range(zBounds.x, zBounds.y));
      transform.LookAt(destination, Vector3.up);
      return;
    }
    if (Vector3.Distance(transform.localPosition, destination) < .1f)
    {
      destination = Vector3.zero;
      return;
    }
    transform.Translate((destination - transform.localPosition).normalized * Time.deltaTime * speed, transform.parent);
  }
}
