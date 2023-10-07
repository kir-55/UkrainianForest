using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavMeshUpdater : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMeshSurface;
    [SerializeField] private int reqiredChanges = 5;
    public int changes;
    

    private void Update()
    {
        if (changes >= reqiredChanges)
            UpdateMesh();
    }
    public void UpdateMesh()
    {
        changes = 0;
        if (navMeshSurface.navMeshData != null)
            navMeshSurface.UpdateNavMesh(navMeshSurface.navMeshData);
        else
            navMeshSurface.BuildNavMesh();
    }
}
