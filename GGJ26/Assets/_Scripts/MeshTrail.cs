using System;
using UnityEngine;
using System.Collections;

public class MeshTrail : MonoBehaviour
{
    [SerializeField] private float activeTime = 2f;
    [Header("Trail Settings")]
    [SerializeField] private float meshFreshRate = 0.1f;
    [SerializeField] Transform positionToSpawn;
    [SerializeField] private float meshDestroyDelay = 3f;
    [Header("Trail Material")]
    [SerializeField] private Material mat;
    
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    private bool isTrailActive;
    

    private void Start()
    {
        if(positionToSpawn == null)
            positionToSpawn = transform;

        StaticEventHandler.OnDash += StaticEventHandler_OnDash;
    }
    private void StaticEventHandler_OnDash(bool isPlayerDashing)
    {
        if(isPlayerDashing && !isTrailActive)
            StartTrail();
    }

    private void StartTrail()
    {
        isTrailActive = true;
        StartCoroutine(ActiveTrail(activeTime));
    }

    IEnumerator ActiveTrail(float time)
    {
        while (time > 0)
        {
            Debug.Log(time);
            time -= meshFreshRate;
            if(skinnedMeshRenderers == null) 
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();

                Vector3 position = new Vector3();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);
                MeshRenderer meshRenderer = gObj.AddComponent<MeshRenderer>();
                MeshFilter meshFilter = gObj.AddComponent<MeshFilter>();
                
                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);
                
                meshFilter.mesh = mesh;
                meshRenderer.material = mat;
                
                Destroy(gObj, meshDestroyDelay);
            }
            yield return new WaitForSeconds(meshFreshRate);
        }
        isTrailActive = false;
    }
}
