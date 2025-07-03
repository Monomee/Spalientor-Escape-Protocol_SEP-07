using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateController : MonoBehaviour
{
    [SerializeField] GameObject topCrate;
    bool isAlreadyAdded = false;
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (isAlreadyAdded)
            return;
        if (collision.gameObject.CompareTag("canPickUp"))
        {
            topCrate.AddComponent<Rigidbody>();
            topCrate.tag = "canPickUp";
            isAlreadyAdded = true;
            MangoController.Instance.mangoes[MangoController.Instance.mangoes.Count - 1].SetActive(true);
        }
    }
}
