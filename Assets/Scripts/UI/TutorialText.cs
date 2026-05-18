using System.Collections;
using UnityEngine;
using TMPro;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI tutorialText;

    [Header("Spotlights")]
    [SerializeField] private GameObject tutorialSpotLightRed1;
    [SerializeField] private GameObject tutorialSpotLightRed2;
    [SerializeField] private GameObject tutorialSpotLightYellow;
    [SerializeField] private GameObject tutorialSpotLightBlack;
    [SerializeField] private float spotlightFadeDuration = 0.5f;

    private int tutorialStep = 1;

    private Light red1Light;
    private Light red2Light;
    private Light yellowLight;
    private Light blackLight;

    private float red1DefaultIntensity;
    private float red2DefaultIntensity;
    private float yellowDefaultIntensity;
    private float blackDefaultIntensity;

    public bool RedCarAllowed => tutorialStep == 1;
    public bool YellowCarAllowed => tutorialStep == 2;
    public bool BlackCarAllowed => tutorialStep == 3;

    private void Start()
    {
        red1Light = tutorialSpotLightRed1.GetComponent<Light>();
        red2Light = tutorialSpotLightRed2.GetComponent<Light>();
        yellowLight = tutorialSpotLightYellow.GetComponent<Light>();
        blackLight = tutorialSpotLightBlack.GetComponent<Light>();

    if (tutorialText == null || red1Light == null || red2Light == null || yellowLight == null || blackLight == null)
        {
            Debug.LogError("TutorialText is missing a text or spotlight reference.");
            return;
        }

        // get the brightness at the minute in the inspector
        red1DefaultIntensity = red1Light.intensity;
        red2DefaultIntensity = red2Light.intensity;
        yellowDefaultIntensity = yellowLight.intensity;
        blackDefaultIntensity = blackLight.intensity;

        // setting the initial intensities
        red1Light.intensity = red1DefaultIntensity;
        red2Light.intensity = red2DefaultIntensity;
        yellowLight.intensity = 0f;
        blackLight.intensity = 0f;

        tutorialText.text = "Use the arrow keys to move the red cars!";
    }

    public void AdvanceToStep2()
    {
        if (tutorialStep != 1) return;

        tutorialStep = 2;
        tutorialText.text = "Now move the yellow cars using A and D!";
        StartCoroutine(FadeSpotlights(
            new Light[] { red1Light, red2Light },
            yellowLight,
            yellowDefaultIntensity,
            spotlightFadeDuration));
        }

    public void AdvanceToStep3(){
        if (tutorialStep != 2) return;

        tutorialStep = 3;
        tutorialText.text = "You can escape! Use the space bar and the X key to move the black car and collect all the coins";
        StartCoroutine(FadeSpotlights(
            new Light[] { yellowLight },
            blackLight,
            blackDefaultIntensity,
            spotlightFadeDuration));
        }

    // create a function that can fade between 2 light sources 
private IEnumerator FadeSpotlights(
    Light[] lightsToFadeOut,
    Light lightToFadeIn,
    float fadeInTargetIntensity,
    float duration)
{
    float[] fadeOutStartIntensities = new float[lightsToFadeOut.Length];

    for (int i = 0; i < lightsToFadeOut.Length; i++)
    {
        fadeOutStartIntensities[i] = lightsToFadeOut[i].intensity;
    }

    float fadeInStartIntensity = lightToFadeIn.intensity;
    float time = 0f;

    while (time < duration)
    {
        time += Time.deltaTime;
        float t = Mathf.SmoothStep(0f, 1f, time / duration);

        for (int i = 0; i < lightsToFadeOut.Length; i++)
        {
            lightsToFadeOut[i].intensity = Mathf.Lerp(fadeOutStartIntensities[i], 0f, t);
        }

        lightToFadeIn.intensity = Mathf.Lerp(fadeInStartIntensity, fadeInTargetIntensity, t);

        yield return null;
    }

    for (int i = 0; i < lightsToFadeOut.Length; i++)
    {
        lightsToFadeOut[i].intensity = 0f;
    }

    lightToFadeIn.intensity = fadeInTargetIntensity;
}



    private IEnumerator FadeSpotlightsToYellow()
    {
        float time = 0f;

        float red1Start = red1Light.intensity;
        float red2Start = red2Light.intensity;
        float yellowStart = yellowLight.intensity;

        while (time < spotlightFadeDuration)
        {
            time += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, time / spotlightFadeDuration);

            red1Light.intensity = Mathf.Lerp(red1Start, 0f, t);
            red2Light.intensity = Mathf.Lerp(red2Start, 0f, t);
            yellowLight.intensity = Mathf.Lerp(yellowStart, yellowDefaultIntensity, t);

            yield return null;
        }

        red1Light.intensity = 0f;
        red2Light.intensity = 0f;
        yellowLight.intensity = yellowDefaultIntensity;
    }
}
