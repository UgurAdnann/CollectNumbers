using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{

    private NumbersController numberController;


    private void Awake()
    {
        numberController = GetComponent<NumbersController>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Movement(int newColumn,int newRow)
    {
        numberController.Column = newColumn;
        numberController.Row = newRow;
        numberController.transform.localPosition = numberController.GridRef.GetWorldPosition(newColumn, newRow);
    }
}
