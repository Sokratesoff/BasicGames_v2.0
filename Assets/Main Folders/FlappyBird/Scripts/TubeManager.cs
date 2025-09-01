using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TubeManager : MonoBehaviour
{
    public Vector3 startPosition, endPosition;
    public float tubeSpeed, tubeGap, tubeAmount, tubeDistance, minTubeHeight, maxTubeHeight;
    public GameObject tubePrefab;
    public List<GameObject> createdTubes;

    public static TubeManager script;

	void Awake(){
        script = this;
	}

    public IEnumerator HandleCreationOfTubes(){
        for(int i = 0; i < tubeAmount; i++){
            GameObject newTube = Instantiate(tubePrefab, startPosition, Quaternion.identity, gameObject.transform);
            createdTubes.Add(newTube);
            newTube.transform.position = new(newTube.transform.position.x, Random.Range(minTubeHeight, maxTubeHeight), newTube.transform.position.z);
            
            yield return new WaitUntil(() => newTube != null && IsTubeGapReached(newTube));
        }

        StartCoroutine(IncreaseTubeSpeedOverTime());
    }

    bool IsTubeGapReached(GameObject newTube){
        tubeDistance = startPosition.x - newTube.transform.position.x;
        return tubeDistance >= tubeGap;
    }

    IEnumerator IncreaseTubeSpeedOverTime(){
        tubeSpeed += Time.deltaTime;
        yield return null;
    }
}
