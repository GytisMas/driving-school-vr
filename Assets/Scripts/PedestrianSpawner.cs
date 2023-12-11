using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestrianSpawner : MonoBehaviour
{
    public List<GameObject> pedestrianPrefabs;
    public int pedestrianCount;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Spawn()
    {
        int count = 0;

        while (count < pedestrianCount)
        {
            GameObject randomPrefab = pedestrianPrefabs[Random.Range(0, pedestrianPrefabs.Count - 1)];
            GameObject obj = Instantiate(randomPrefab);
            Transform child = transform.GetChild(Random.Range(0, transform.childCount - 1));
            obj.GetComponent<WayPointNavigator>().currentWaypoint = child.GetComponent<Waypoint>();
            obj.transform.position = child.position;

            yield return new WaitForEndOfFrame();

            count++;
        }
    }
}
