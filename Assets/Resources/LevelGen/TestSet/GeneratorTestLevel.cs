using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeneratorTestLevel : MonoBehaviour
{
    [SerializeField] int GridSize;//How many tiles will be generated to the left of the spawn. (Half the map's width).
    [SerializeField] string GeneratorTileset;
    [SerializeField] Object[] tiles;
    [SerializeField] GameObject SpawnTile;

    private Object[] edgeTiles;
    private Object[] normalTiles;

    private System.Random rnd;

    private void Start()
    {
        rnd = new System.Random();
        LoadTiles();
        Generate();
    }

    private void LoadTiles()
    {
        tiles = Resources.LoadAll("LevelGen/" + GeneratorTileset + "/Tiles");

        List<Object> edgeTilesList = new List<Object>();
        List<Object> normalTilesList = new List<Object>();

        for (int i = 0; i < tiles.Length; i++)
        {
            switch (tiles[i].name.Split(',')[0])
            {
                case "e": edgeTilesList.Add(tiles[i]); break;
                default: normalTilesList.Add(tiles[i]); break;
            }
        }

        edgeTiles = edgeTilesList.ToArray();
        normalTiles = normalTilesList.ToArray();
    }

    private void Generate()
    {
        #region Generate Middle Pieces
        GridSize -= 1;
        float CellSize = SpawnTile.transform.localScale.x * 10;
        float currentX = 0 - ((GridSize + 1) * CellSize) + CellSize;
        float currentY = 0 + ((GridSize + 1) * CellSize) - CellSize;//Starts generation in top left corner of map

        int rows = (GridSize * 2);
        int cols = rows;
        while (rows >= 0)
        {
            while (cols >= 0)
            {
                if (currentX != 0 || currentY != 0) { Instantiate(normalTiles[rnd.Next(0,normalTiles.Length)], new Vector3(currentX, 0, currentY), Quaternion.identity); }
                currentX += CellSize;
                cols--;
            }
            cols = (GridSize * 2);
            currentX = 0 - ((GridSize + 1) * CellSize) + CellSize;
            currentY -= CellSize;
            rows--;
        }
        #endregion

        #region Generate Edge Pieces
        currentX = 0 - ((GridSize + 1) * CellSize);
        currentY = 0 + ((GridSize + 1) * CellSize);//Starts generation in top left corner of map

        for (int i = (GridSize * 2) + 1; i >=0; i--)
        {
            Instantiate(edgeTiles[rnd.Next(0, edgeTiles.Length)], new Vector3(currentX, 0, currentY), Quaternion.identity);
            currentX += CellSize;
        }
        for (int i = (GridSize * 2) + 1; i >= 0; i--)
        {
            Instantiate(tiles[0], new Vector3(currentX, 0, currentY), Quaternion.identity);
            currentY -= CellSize;
        }
        for (int i = (GridSize * 2) + 1; i >= 0; i--)
        {
            Instantiate(tiles[0], new Vector3(currentX, 0, currentY), Quaternion.identity);
            currentX -= CellSize;
        }
        for (int i = (GridSize * 2) + 1; i >= 0; i--)
        {
            Instantiate(tiles[0], new Vector3(currentX, 0, currentY), Quaternion.identity);
            currentY += CellSize;
        }
        #endregion
    }
}
