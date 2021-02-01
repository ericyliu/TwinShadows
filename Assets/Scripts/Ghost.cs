using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ghost : MonoBehaviour
{
  public Vector2 xBounds = new Vector2(-1f, 1f);
  public Vector2 yBounds = new Vector2(-1f, 1f);
  public Vector2 zBounds = new Vector2(-.5f, .5f);
  public float speed = 5f;
  public Vector3 destination = Vector3.zero;
  public Transform mousePlane;
  public ParticleSystem mouseMissParticle;
  public ParticleSystem mouseHitParticle;
  public Animator animator;
  public bool isHit = false;
  public bool started = false;
  public int level = 1;
  public AudioSource ghostHitSound;
  public UnityEvent onFinish = new UnityEvent();

  // Start is called before the first frame update
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    GhostMovement();
    GhostTouch();
  }

  void GhostMovement()
  {
    if (isHit || !started) return;
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
    transform.Translate((destination - transform.localPosition).normalized * Time.deltaTime * speed * level, transform.parent);
  }

  void GhostTouch()
  {
    if (!Input.GetMouseButtonDown(0) || !started) return;
    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    RaycastHit[] hits = Physics.RaycastAll(ray);
    if (hits.Length > 0)
    {
      bool ghostHit = false;
      foreach (RaycastHit hit in hits)
      {
        if (hit.transform.Equals(mousePlane))
        {
          mouseHitParticle.transform.position = hit.point;
          mouseMissParticle.transform.position = hit.point;
        }
        if (hit.transform.Equals(transform) && !isHit)
        {
          ghostHit = true;
          isHit = true;
          animator.Play("Hit");
          ghostHitSound.Play();
          level += 1;
          if (level > 3) EndHunt();
        }
      }
      (ghostHit ? mouseHitParticle : mouseMissParticle).Play();
    }

  }

  void EndHunt()
  {
    transform.localPosition = new Vector3(0, 0, 4);
    started = false;
    transform.localRotation = Quaternion.Euler(0, 180, 0);
    onFinish.Invoke();
  }
}
