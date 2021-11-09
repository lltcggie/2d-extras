using UnityEngine;
using System.IO;
using UnityEditor.AssetImporters;
using UnityEngine.Tilemaps;
using UnityEditor;
using System.Collections.Generic;
using Newtonsoft.Json;

/// <summary>
/// .llttのファイルをScriptableObjectDataとして使えるように変換するクラス
/// </summary>
[ScriptedImporter(1, "lltt")]
public class llttImporter : ScriptedImporter
{
    private class MapchipInfo
    {
        // 床マップチップ名(パターン分リストに入ってる)
        public List<string> Floor { get; set; }
        // 壁マップチップの壁を示すビット値とパターン分ファイル名リストの辞書
        public Dictionary<int, List<string>> Wall { get; set; }

    }

    /// <summary>
    /// .llttファイルのインポート時、変更時に実行される(ctxにファイルの情報が格納)
    /// </summary>
    public override void OnImportAsset(AssetImportContext ctx)
    {
        // .llttのファイルにかかれているテキストを取得
        string jsonStr = File.ReadAllText(ctx.assetPath);

        // jsonなのでデシリアライズ
        MapchipInfo mapchipInfo = JsonConvert.DeserializeObject<MapchipInfo>(jsonStr);
        if (mapchipInfo == null || mapchipInfo.Floor.Count == 0 || mapchipInfo.Wall.Count != 256)
        {
            LLTerrainTile data = ScriptableObject.CreateInstance<LLTerrainTile>();
            // 床
            data.m_FloorSprites = new Sprite[1];
            // 壁
            data.m_WallSprites = new Sprite[256];
            // ScriptableObjectDataをメインアセットとして登録
            ctx.AddObjectToAsset("LL Terrain Tile Imported", data); // 第1引数はアセットIDで、後から変更すると参照が切れたりするので注意
            ctx.SetMainObject(data);
        }
        else
        {
            var dir = Path.GetDirectoryName(ctx.assetPath);

            // ScriptableObjectDataを作成、.llttファイルに書かれていたテキストを設定
            LLTerrainTile data = ScriptableObject.CreateInstance<LLTerrainTile>();

            // 床
            data.m_FloorSprites = new Sprite[mapchipInfo.Floor.Count];
            for (int i = 0; i < data.m_FloorSprites.Length; i++)
            {
                var fpath = Path.Combine(dir, mapchipInfo.Floor[i]);
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(fpath);

                if (sprite == null)
                {
                    // TODO: エラーログ出す
                    int j = 0;
                }

                data.m_FloorSprites[i] = sprite;
            }

            // 壁
            data.m_WallSprites = new Sprite[mapchipInfo.Wall.Count];
            for (int i = 0; i < data.m_WallSprites.Length; i++)
            {
                var list = mapchipInfo.Wall[i];
                if (list == null)
                {
                    // TODO: エラーログ出す
                    int j = 0;
                }

                // TODO: パターン対応
                var fpath = Path.Combine(dir, list[0]);
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(fpath);

                if (sprite == null)
                {
                    // TODO: エラーログ出す
                    int j = 0;
                }

                data.m_WallSprites[i] = sprite;
            }

            // ScriptableObjectDataをメインアセットとして登録
            ctx.AddObjectToAsset("LL Terrain Tile Imported", data); // 第1引数はアセットIDで、後から変更すると参照が切れたりするので注意
            ctx.SetMainObject(data);
        }
        
    }

}