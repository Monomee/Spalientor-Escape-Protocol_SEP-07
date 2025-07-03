using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MangoController : MonoBehaviour
{
    public static MangoController Instance;
    public List<GameObject> mangoes;
    public int remainingMangoes;
    private void OnEnable()
    {
        Instance = this;
    }
    private void OnDisable()
    {
        Instance = null;
    }
    // Start is called before the first frame update
    void Start()
    {
        remainingMangoes = mangoes.Count;
        UIManagerMap1.Instance.UpdateMangoText(remainingMangoes);
    }
}
