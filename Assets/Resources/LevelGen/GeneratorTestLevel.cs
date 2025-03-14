using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GeneratorTestLevel : MonoBehaviour
{
    [Header("Set")]
    [SerializeField] string GeneratorTileset;
    [SerializeField] GameObject SpawnTile;

    private Object[] tiles;
    private Object[] edgeTiles;
    private Object[] normalTiles;
    private Object[] ruleTiles;
    private TextAsset rules;

    private System.Random rnd;

    [Header("Rates")]
    [Range(5, 100)]
    [SerializeField] int GridSize;//How many tiles will be generated to the left of the spawn. (Half the map's width).
    [Range(5, 0)]
    public double ruleSpawnRate = 1;

    private void Start()
    {
        rules = Resources.Load("LevelGen/" + GeneratorTileset + "/Rules") as TextAsset;
        Debug.Log("Rules: " + rules.text);
        rnd = new System.Random();
        LoadTiles();
        Generate();
    }

    private void LoadTiles()
    {
        tiles = Resources.LoadAll("LevelGen/" + GeneratorTileset + "/Tiles");

        List<Object> edgeTilesList = new List<Object>();
        List<Object> normalTilesList = new List<Object>();
        List<Object> ruleTilesList = new List<Object>();

        string x;
        for (int i = 0; i < tiles.Length; i++)
        {
            x = tiles[i].name.Split(',')[0];
            if(rules.text.Contains(x))
            {
                Debug.Log("Rule Tile Found");
                ruleTilesList.Add(tiles[i]);
            }else{
                switch (x)
                {
                    case "e": edgeTilesList.Add(tiles[i]); break;
                    default: normalTilesList.Add(tiles[i]); break;
                }
            }
        }
        ruleTiles = ruleTilesList.ToArray();
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
            Instantiate(edgeTiles[0], new Vector3(currentX, 0, currentY), Quaternion.identity);
            currentY -= CellSize;
        }
        for (int i = (GridSize * 2) + 1; i >= 0; i--)
        {
            Instantiate(edgeTiles[0], new Vector3(currentX, 0, currentY), Quaternion.identity);
            currentX -= CellSize;
        }
        for (int i = (GridSize * 2) + 1; i >= 0; i--)
        {
            Instantiate(edgeTiles[0], new Vector3(currentX, 0, currentY), Quaternion.identity);
            currentY += CellSize;
        }
        #endregion

        #region Generate Rule
        for (double i = (GridSize / ruleSpawnRate) + 1; i >= 0; i--)
        {
            Object chosenTile = ruleTiles[rnd.Next(0, ruleTiles.Length)];//Pick Tile
            string prefix = chosenTile.name.Split(',')[0];
            string[] rulesA = rules.text.Split(',');
            int ruleIndex = 1;
            for (int j = rulesA.Length - 1; j >= 0; j--)
            {
                if (rulesA[j] == prefix)
                {
                    ruleIndex = j + 1;
                    j = 0;
                }
            }//Match tile name to its rule

            currentX = rnd.Next(0 - (GridSize), (GridSize)) * CellSize;
            currentY = rnd.Next(0 - (GridSize), (GridSize)) * CellSize;
            Vector3 chosenPosition = new Vector3(currentX, 10, currentY);
            //Pick Position

            RaycastHit hitTop;
            Physics.Raycast(chosenPosition + new Vector3(0, 0, CellSize), -Vector3.up, out hitTop);
            RaycastHit hitRight;
            Physics.Raycast(chosenPosition + new Vector3(CellSize, 0, 0), -Vector3.up, out hitRight);
            RaycastHit hitBottom;
            Physics.Raycast(chosenPosition + new Vector3(0, 0, -CellSize), -Vector3.up, out hitBottom);
            RaycastHit hitLeft;
            Physics.Raycast(chosenPosition + new Vector3(-CellSize, 0, 0), -Vector3.up, out hitLeft);
            //Find the tile in the chosen position

            int adjacentMatches = 0;
            if (ruleIndex.ToString() == hitTop.transform.name.Split(',')[0])
            {
                adjacentMatches += 1;
            }
            if (ruleIndex.ToString() == hitRight.transform.name.Split(',')[0])
            {
                adjacentMatches += 1;
            }
            if (ruleIndex.ToString() == hitBottom.transform.name.Split(',')[0])
            {
                adjacentMatches += 1;
            }
            if (ruleIndex.ToString() == hitLeft.transform.name.Split(',')[0])
            {
                adjacentMatches += 1;
            }

            if (adjacentMatches >= 2)
            {
                Debug.Log("Matches, creating rule tile.");
                RaycastHit hit;
                Physics.Raycast(chosenPosition, -Vector3.up, out hit);
                Destroy(hit.collider.gameObject);
                Instantiate(chosenTile, chosenPosition - (Vector3.up * 10), Quaternion.identity);
                Debug.Log("Rule tile created.");
                //Swap out current tile if it fits the rule
            }
        }
        #endregion
    }
}
