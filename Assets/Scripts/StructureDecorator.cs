using UnityEngine;
using Google.Maps.Event;

public class StructureDecorator : MonoBehaviour
{
  public GameObject player;

  public void OnDidCreateExtrudedStructure(DidCreateExtrudedStructureArgs args)
  {
    args.GameObject.AddComponent<MeshCollider>();
  }

  public void OnDidCreateModeledStructure(DidCreateModeledStructureArgs args)
  {
    args.GameObject.AddComponent<MeshCollider>();
  }

  public void OnDidCreateRegion(DidCreateRegionArgs args)
  {
    args.GameObject.AddComponent<MeshCollider>();
  }

  public void OnDidCreateSegment(DidCreateSegmentArgs args)
  {
    args.GameObject.AddComponent<MeshCollider>();
  }

  public void OnStart(MapLoadStartArgs args)
  {
    player.transform.position = new Vector3(player.transform.position.x, 10f, player.transform.position.z);
  }
}
