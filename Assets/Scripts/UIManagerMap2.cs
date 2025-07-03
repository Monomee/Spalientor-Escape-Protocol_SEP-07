using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMap2 : MonoBehaviour
{
    public static UIManagerMap2 Instance;
    private void OnEnable()
    {
        Instance = this;
    }
    private void OnDisable()
    {
        Instance = null;
    }
    public Slider healthBar;
    public TextMeshProUGUI bulletNumber;
    [SerializeField] private TextMeshProUGUI deadText;
    [SerializeField] private Button playAgain;
    [SerializeField] private Button quitButton;
    [SerializeField] private Image deadPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OpenDeadText()
    {
        deadText.gameObject.SetActive(true);
        playAgain.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        deadPanel.gameObject.SetActive(true);
    }
}
