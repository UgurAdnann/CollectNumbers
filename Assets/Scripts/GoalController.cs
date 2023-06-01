using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalController : MonoBehaviour
{
    public GridSystemSO gridSystemSO;
    private GridManager gridManager;
    public ColorType colorType;
    public int value;
    private TMPro.TextMeshProUGUI valueText;
    private bool isDone;

    void Start()
    {
        gridManager = ObjectManager.GridManager;
        valueText = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        SetColorValue();
        SetText(0);
    }

    void Update()
    {
        
    }

    private void SetColorValue()
    {
        if(colorType.Equals(ColorType.Red))
            GetComponent<Image>().color = gridSystemSO.colors[1];
        if (colorType.Equals(ColorType.Green))
            GetComponent<Image>().color = gridSystemSO.colors[2];
        if(colorType.Equals(ColorType.Blue))
            GetComponent<Image>().color = gridSystemSO.colors[3];
        if(colorType.Equals(ColorType.Orange))
            GetComponent<Image>().color = gridSystemSO.colors[4];
        if(colorType.Equals(ColorType.Pink))
            GetComponent<Image>().color = gridSystemSO.colors[5];
    }

    public void SetText(int minus)
    {
        value -= minus;
        if(value<=0)
        {
            if (!isDone)
            {
                isDone = true;
                gridManager.CheckWin();
            }
            value = 0;
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(true);
        }
        valueText.text = value.ToString();
           
    }
}
