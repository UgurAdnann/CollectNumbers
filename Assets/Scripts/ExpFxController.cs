using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpFxController : MonoBehaviour
{
    public GridSystemSO gridSystemSO;
    private GridManager gridManager;
 
    void Start()
    {
        gridManager = ObjectManager.GridManager;
    }

    public void SetColor(NumbersController target)
    {
        GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[target.colorValue];
        transform.GetChild(0).GetComponent<ParticleSystem>().startColor=gridSystemSO.colors[target.colorValue];
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(WaitDestroy(target));

        //    transform.GetChild(0).GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[1];
        //if (target.GetComponent<GoalController>().colorType.Equals(ColorType.Red))
        //{
        //    GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[1];
        //    transform.GetChild(0).GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[1];
        //}
        //if (target.GetComponent<GoalController>().colorType.Equals(ColorType.Green))
        //{
        //    GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[2];
        //    transform.GetChild(0).GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[2];
        //}
        //if (target.GetComponent<GoalController>().colorType.Equals(ColorType.Blue))
        //{
        //    GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[3];
        //    transform.GetChild(0).GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[3];
        //}
        //if (target.GetComponent<GoalController>().colorType.Equals(ColorType.Orange))
        //{
        //    GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[4];
        //    transform.GetChild(0).GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[4];
        //}
        //if (target.GetComponent<GoalController>().colorType.Equals(ColorType.Pink))
        //{
        //    GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[5];
        //    transform.GetChild(0).GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[5];
        //}


    }

    IEnumerator WaitDestroy(NumbersController target)
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        gridManager.expFxQue.Enqueue(transform.gameObject);
    }
}
