using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    #region Create Grid
    private CanvasManager canvasManager; //------------
    public GridSystemSO gridSystemSO;//-------------
    #endregion


    private void Awake()
    {
        ObjectManager.GridManager = this; //-------------
    }

    void Start()
    {
        CreateGrid();
    }

    #region Create Grid
    private void CreateGrid()
    {
        //Add numberPrefabs to dictionary
        gridSystemSO.numberPrefabDict = new Dictionary<NumberType, GameObject>();
        for (int i = 0; i < gridSystemSO.numberPrefabs.Length; i++)
        {
            if (!gridSystemSO.numberPrefabDict.ContainsKey(gridSystemSO.numberPrefabs[i].type))
            {
                gridSystemSO.numberPrefabDict.Add(gridSystemSO.numberPrefabs[i].type, gridSystemSO.numberPrefabs[i].prefab);
            }
        }
        //Create Grids
        for (int i = 0; i < gridSystemSO.column; i++)
        {
            for (int j = 0; j < gridSystemSO.row; j++)
            {
                GameObject grid = Instantiate(gridSystemSO.gridPrefab, GetWorldPosition(i, j), Quaternion.identity);
                grid.transform.SetParent(transform);
                grid.transform.localScale = Vector3.one * gridSystemSO.gridScale;
            }
        }


        //Create Numbers
        gridSystemSO.numbers = new NumbersController[gridSystemSO.column, gridSystemSO.row];
        for (int i = 0; i < gridSystemSO.column; i++)
        {
            for (int j = 0; j < gridSystemSO.row; j++)
            {
               GameObject newNumber = Instantiate(gridSystemSO.numberPrefabDict[NumberType.Normal], Vector3.zero, Quaternion.identity);
                newNumber.name = "Number(" + i + "," + j + ")";
                newNumber.transform.SetParent(transform);
                newNumber.transform.localScale = Vector3.one * gridSystemSO.gridScale;
                gridSystemSO.numbers[i, j] = newNumber.GetComponent<NumbersController>();
                gridSystemSO.numbers[i, j].Init(i, j, this, NumberType.Normal);

                gridSystemSO.numbers[i, j].GetComponent<MovementManager>().Movement(i,j);
            }
        }
    }
    #endregion
    //Set Grids Positions
  public  Vector2 GetWorldPosition(int x, int y)
    {
        return new Vector2((transform.position.x - gridSystemSO.column / 2 + x) * gridSystemSO.gridScale, (transform.position.y + gridSystemSO.row / 2 - y) * gridSystemSO.gridScale);
    }

}
