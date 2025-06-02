using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TerrainAutoNeighbor : MonoBehaviour
{
    public void AutoSetAllNeighbors()
    {
        Terrain[] allTerrains = Terrain.activeTerrains;
        Dictionary<Vector2Int, Terrain> terrainMap = new Dictionary<Vector2Int, Terrain>();

        // 기준 좌표 계산 (Terrain 위치 / size → 격자 위치로 변환)
        foreach (Terrain t in allTerrains)
        {
            Vector3 pos = t.transform.position;
            Vector3 size = t.terrainData.size;
            Vector2Int key = new Vector2Int(Mathf.RoundToInt(pos.x / size.x), Mathf.RoundToInt(pos.z / size.z));
            terrainMap[key] = t;
        }

        // 각 Terrain에 대해 인접 Terrain 지정
        foreach (var kvp in terrainMap)
        {
            Vector2Int pos = kvp.Key;
            Terrain center = kvp.Value;

            terrainMap.TryGetValue(pos + Vector2Int.left, out Terrain left);
            terrainMap.TryGetValue(pos + Vector2Int.right, out Terrain right);
            terrainMap.TryGetValue(pos + new Vector2Int(0, 1), out Terrain top);
            terrainMap.TryGetValue(pos + new Vector2Int(0, -1), out Terrain bottom);

            center.SetNeighbors(left, top, right, bottom);
        }

        Debug.Log("All terrain neighbors automatically set.");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TerrainAutoNeighbor))]
public class TerrainAutoNeighborEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TerrainAutoNeighbor tool = (TerrainAutoNeighbor)target;

        if (GUILayout.Button("자동으로 SetNeighbors 적용"))
        {
            tool.AutoSetAllNeighbors();
        }
    }
}
#endif
