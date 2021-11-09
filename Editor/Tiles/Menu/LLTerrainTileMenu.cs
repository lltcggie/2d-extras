using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps
{
    static internal partial class AssetCreation
    {
            
        [MenuItem("Assets/Create/2D/Tiles/LL Terrain Tile", priority = (int)ETilesMenuItemOrder.LLTerrainTile)]
        static void CreateLLTerrainTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<LLTerrainTile>(), "New LL Terrain Tile.asset");
        }
    }
}