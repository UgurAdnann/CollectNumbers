using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridSystem", menuName = "CreateGrid")]

public class GridSystemSO : ScriptableObject
{
    

    #region Struct
    [System.Serializable]
    public struct NumberPrefab
    {
        public NumberType type;
        public GameObject prefab;
    };
    #endregion

    public int row, column, maxNumber, useableMaxNumber;
    public Vector2 startPoint;
    public float gridScale;
    public Color[] colors;

    public GameObject gridPrefab;
    public Dictionary<NumberType, GameObject> numberPrefabDict;
    public NumberPrefab[] numberPrefabs;
    public NumbersController[,] numbers;

}
