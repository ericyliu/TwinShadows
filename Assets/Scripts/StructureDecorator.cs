using System.Collections.Generic;
using UnityEngine;
using Google.Maps.Event;

public class StructureDecorator : MonoBehaviour
{
  public GameObject player;
  public List<GameObject> houseModels = new List<GameObject>();

  List<GameObject> roads = new List<GameObject>();
  List<GameObject> houses = new List<GameObject>();


  public void OnDidCreateExtrudedStructure(DidCreateExtrudedStructureArgs args)
  {
    CreateRandomHouse(args.GameObject);
  }

  public void OnDidCreateModeledStructure(DidCreateModeledStructureArgs args)
  {
    CreateRandomHouse(args.GameObject);
  }

  public void OnDidCreateSegment(DidCreateSegmentArgs args)
  {
    roads.Add(args.GameObject);
  }

  public void OnStart(MapLoadStartArgs args)
  {
    player.transform.position = new Vector3(player.transform.position.x, 10f, player.transform.position.z);
  }

  public void OnLoaded(MapLoadedArgs args)
  {
    foreach (GameObject house in houses)
    {
      GameObject closestRoad = GetClosestRoad(house);
      house.transform.LookAt(closestRoad.transform);
    }
    player.transform.LookAt(GetClosestRoad(player).transform);
  }

  void CreateRandomHouse(GameObject parent)
  {
    GameObject house = GameObject.Instantiate(houseModels[Random.Range(0, houseModels.Count)], parent.transform);
    house.transform.localScale = new Vector3(30, 30, 30);
    parent.GetComponent<MeshRenderer>().enabled = false;
    houses.Add(house);
  }

  GameObject GetClosestRoad(GameObject subject)
  {
    GameObject bestTarget = null;
    float closestDistanceSqr = Mathf.Infinity;
    Vector3 currentPosition = subject.transform.position;
    foreach (GameObject potentialTarget in roads)
    {
      Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
      float dSqrToTarget = directionToTarget.sqrMagnitude;
      if (dSqrToTarget < closestDistanceSqr)
      {
        closestDistanceSqr = dSqrToTarget;
        bestTarget = potentialTarget;
      }
    }

    return bestTarget;
  }
}
