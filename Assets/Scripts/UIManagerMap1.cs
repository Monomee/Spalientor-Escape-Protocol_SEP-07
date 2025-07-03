using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerMap1 : MonoBehaviour
{
    public static UIManagerMap1 Instance;
    private void OnEnable()
    {
        Instance = this;
    }
    private void OnDisable()
    {
        Instance = null;
    }

    public PasswordUIMap1 passwordUI;

    [Header("UI Init Screeen")]
    [SerializeField] Image image;
    [SerializeField] private float fadeDuration = 1.5f;

    [Header("UI Messages")]
    string[] messages = {
        "WAKE UP",
        "You are trapped in a simulation",
        "Use WASD/Joystick to move around.",
        "Press Space/JumpButton to jump.",
        "Collect mangoes of mind to escape.",       
    };
    [SerializeField] TextMeshProUGUI messageText;

    [Header("UI Mango")]
    [SerializeField] TextMeshProUGUI mangoText;

    [Header("UI Mission")]
    [SerializeField] Image mission1Panel;
    [SerializeField] Image mission2Panel;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisplayMessages());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator FadeOut()
    {
        // init alpha
        float startAlpha = image.color.a;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);

            // update the alpha value of the image
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            yield return null;
        }

        // make sure the alpha is set to 0 at the end
        Color finalColor = image.color;
        finalColor.a = 0f;
        image.color = finalColor;
    }
    IEnumerator DisplayMessages()
    {
        StartCoroutine(FadeOut());
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < messages.Length; i++)
        {
            messageText.text = messages[i];
            yield return new WaitForSeconds(3f);
        }
        messageText.text = "";
        yield return new WaitForSeconds(1f);
        mission1Panel.gameObject.SetActive(true);
    }
    public void UpdateMangoText(int remainingMangoes)
    {
        mangoText.text = remainingMangoes + "/" + MangoController.Instance.mangoes.Count;
        if (remainingMangoes <= 0)
        {
            StartCoroutine(DisplayMission());
        }
    }
    IEnumerator DisplayMission()
    {
        mission1Panel.color = Color.green;
        yield return new WaitForSeconds(3f);
        mission1Panel.gameObject.SetActive(false);
        mission2Panel.gameObject.SetActive(true);
        mission2Panel.color = Color.red;
        yield break;
    }

}
