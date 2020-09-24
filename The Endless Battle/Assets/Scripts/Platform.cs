using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Platform : MonoBehaviour
{
    // reference to the navmeshsurface of the platform
    public NavMeshSurface navMesh;
    // Start is called before the first frame update
    void Start()
    {
        navMesh = GetComponent<NavMeshSurface>();
        // Build the navmesh upon instantiation since the size may differ despite the original size fo the prefab
        navMesh.BuildNavMesh();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
