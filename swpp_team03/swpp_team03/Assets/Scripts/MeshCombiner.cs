using UnityEngine;

public class MeshCombiner : MonoBehaviour
{
    [ContextMenu("Combine Meshes")]
    void Combine()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        // 새 메쉬 생성
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine);

        // 루트에 MeshFilter, MeshRenderer 추가
        MeshFilter filter = gameObject.AddComponent<MeshFilter>();
        filter.mesh = combinedMesh;

        MeshRenderer renderer = gameObject.AddComponent<MeshRenderer>();
        renderer.material = meshFilters[0].GetComponent<Renderer>().sharedMaterial;

        // 충돌용 Mesh Collider 추가
        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = combinedMesh;
        collider.convex = false;

        // 자식 오브젝트 제거하거나 비활성화
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false); // 또는 DestroyImmediate(child.gameObject);
        }
    }
}
