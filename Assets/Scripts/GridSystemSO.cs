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

    [System.Serializable]
    public struct EntryNumber
    {
        [Header("0 and above max number=rnd value")]
        public int value;
        public int row,column;
    };

    [System.Serializable]
    public struct GoalType
    {
        public ColorType color;
        public int value;
    };
    #endregion

    public int row, column, maxNumber, useableMaxNumber,matchNumber;
    public float fillTime;
    public float gridScale;
    public Color[] colors;
    public bool isSmootCreateStart;

    public GameObject gridPrefab;
    public Dictionary<NumberType, GameObject> numberPrefabDict;
    public NumberPrefab[] numberPrefabs;
    public NumbersController[,] numbers;
    public GameObject[,] grids;
    public EntryNumber[] entryNumbers;
    public GoalType[] goals;

}
