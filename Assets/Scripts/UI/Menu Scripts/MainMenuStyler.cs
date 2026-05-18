using UnityEngine;
using UnityEngine.UI;
using TMPro;

[ExecuteAlways]
public class MainMenuStyler : MonoBehaviour
{
    private void OnEnable() => Apply();

#if UNITY_EDITOR
    private void OnValidate() => Apply();
#endif

    private void Apply()
    {
        SetupCanvasScaler();
        StylePanel();
        StyleTitle();
        SetupButtonContainer();
        Canvas.ForceUpdateCanvases();
    }

    private void SetupCanvasScaler()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null) return;

        CanvasScaler scaler = canvas.GetComponent<CanvasScaler>();
        if (scaler == null) scaler = canvas.gameObject.AddComponent<CanvasScaler>();

        scaler.uiScaleMode        = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.screenMatchMode    = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;
    }

    private void StylePanel()
    {
        GameObject panel = GameObject.Find("Panel");
        if (panel == null) return;

        // Stretch panel to fill entire canvas
        RectTransform rt = panel.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        Image img = panel.GetComponent<Image>();
        if (img != null)
        {
            Color c;
            ColorUtility.TryParseHtmlString("#0F0F1A", out c);
            img.color = c;
        }
    }

    private void StyleTitle()
    {
        GameObject title = GameObject.Find("Title Text");
        if (title == null) return;

        // Stretch horizontally, sit in the top third of the screen
        RectTransform rt = title.GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.anchorMin        = new Vector2(0f, 0.65f);
            rt.anchorMax        = new Vector2(1f, 0.85f);
            rt.offsetMin        = new Vector2(40f, 0f);
            rt.offsetMax        = new Vector2(-40f, 0f);
        }

        TextMeshProUGUI tmp = title.GetComponent<TextMeshProUGUI>();
        if (tmp == null) return;

        tmp.text      = "Car Escape";
        tmp.fontSize  = 96;
        tmp.fontStyle = FontStyles.Bold;
        tmp.color     = Color.white;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.enableAutoSizing  = true;
        tmp.fontSizeMin       = 24;
        tmp.fontSizeMax       = 96;
    }

    private void SetupButtonContainer()
    {
        // Find or create a container to hold the buttons with auto-spacing
        GameObject container = GameObject.Find("ButtonContainer");
        if (container == null)
        {
            GameObject tutBtn   = GameObject.Find("TutorialButton");
            GameObject lvl1Btn  = GameObject.Find("Level1Button");
            if (tutBtn == null || lvl1Btn == null) return;

            container = new GameObject("ButtonContainer");
            container.transform.SetParent(tutBtn.transform.parent, false);

            // Parent the buttons into the container
            tutBtn.transform.SetParent(container.transform, false);
            lvl1Btn.transform.SetParent(container.transform, false);
        }

        // Anchor container to the center of the screen
        RectTransform crt = container.GetComponent<RectTransform>();
        if (crt == null) crt = container.AddComponent<RectTransform>();

        crt.anchorMin        = new Vector2(0.3f, 0.25f);
        crt.anchorMax        = new Vector2(0.7f, 0.55f);
        crt.offsetMin        = Vector2.zero;
        crt.offsetMax        = Vector2.zero;

        // VerticalLayoutGroup spaces buttons automatically
        VerticalLayoutGroup vlg = container.GetComponent<VerticalLayoutGroup>();
        if (vlg == null) vlg = container.AddComponent<VerticalLayoutGroup>();

        vlg.spacing              = 20f;
        vlg.childControlWidth    = true;
        vlg.childControlHeight   = true;
        vlg.childForceExpandWidth  = true;
        vlg.childForceExpandHeight = true;
        vlg.padding              = new RectOffset(0, 0, 0, 0);

        StyleButton("TutorialButton");
        StyleButton("Level1Button");
    }

    private void StyleButton(string buttonName)
    {
        GameObject btn = GameObject.Find(buttonName);
        if (btn == null) return;

        Image img = btn.GetComponent<Image>();
        if (img != null)
        {
            Color c;
            ColorUtility.TryParseHtmlString("#1E1E35", out c);
            img.color = c;
        }

        Button button = btn.GetComponent<Button>();
        if (button != null)
        {
            Color normal, hover, press;
            ColorUtility.TryParseHtmlString("#1E1E35", out normal);
            ColorUtility.TryParseHtmlString("#2E2E55", out hover);
            ColorUtility.TryParseHtmlString("#0F0F20", out press);

            ColorBlock cb       = button.colors;
            cb.normalColor      = normal;
            cb.highlightedColor = hover;
            cb.pressedColor     = press;
            cb.colorMultiplier  = 1f;
            button.colors       = cb;
        }

        TextMeshProUGUI tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
        {
            tmp.fontSize        = 36;
            tmp.fontStyle       = FontStyles.Bold;
            tmp.color           = Color.white;
            tmp.alignment       = TextAlignmentOptions.Center;
            tmp.enableAutoSizing = true;
            tmp.fontSizeMin      = 18;
            tmp.fontSizeMax      = 36;
        }
    }
}
