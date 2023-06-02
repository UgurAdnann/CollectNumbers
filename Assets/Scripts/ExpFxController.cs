using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpFxController : MonoBehaviour
{
    #region Variables for General
    public GridSystemSO gridSystemSO;
    private GridManager gridManager;
    #endregion

    void Start()
    {
        gridManager = ObjectManager.GridManager;
    }

    [System.Obsolete]
    public void SetColor(NumbersController target)
    {
        GetComponent<ParticleSystem>().startColor = gridSystemSO.colors[target.colorValue];
        transform.GetChild(0).GetComponent<ParticleSystem>().startColor=gridSystemSO.colors[target.colorValue];
        GetComponent<ParticleSystem>().Play();
        StartCoroutine(WaitDestroy(target));
    }

    IEnumerator WaitDestroy(NumbersController target)
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
        gridManager.expFxQue.Enqueue(transform.gameObject);
    }
}
