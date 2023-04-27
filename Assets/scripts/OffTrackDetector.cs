using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CarController; 


public class OffTrackDetector : MonoBehaviour
{
    private int terrainLayer;
    public event Action OnCarOffTrack;
    public event Action OnCarOnTrack;

private void Start()
{
    terrainLayer = LayerMask.NameToLayer("Terrain");
}
private void OnTriggerEnter(Collider other)
{
    Debug.Log("Trigger entered: " + other.gameObject.name);
    if (other.gameObject.name == "Terrain")
    {
        OnCarOffTrack?.Invoke(); 
    }
    else{
        OnCarOnTrack?.Invoke(); 
        
    }
}
private void OnTriggerExit(Collider other)
{
    Debug.Log("Trigger exited: " + other.gameObject.name);
    OnCarOffTrack?.Invoke(); 
}



}
