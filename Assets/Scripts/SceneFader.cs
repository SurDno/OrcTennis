using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class SceneFader {
    public enum FadeInStatus { None, Fading, Faded};

    private static Image fadeInObject;
    private static FadeInStatus status = FadeInStatus.None;

    public static IEnumerator FadeIn(float duration) {
        Debug.Log("called");

        if (!fadeInObject)
            CreateFadeInObject();

        float alpha = 1f;
        status = FadeInStatus.Fading;
        while (alpha > 0) {
            alpha -= Time.deltaTime / duration;
            Color c = fadeInObject.color;
            c.a = alpha;
            fadeInObject.color = c;
            yield return null;
        }
        status = FadeInStatus.None;
    }

    public static IEnumerator FadeOut(float duration) {
        if (!fadeInObject)
            CreateFadeInObject();

        float alpha = 0f;
        status = FadeInStatus.Fading;
        while (alpha < 1) {
            alpha += Time.deltaTime / duration;
            Color c = fadeInObject.color;
            c.a = alpha;
            fadeInObject.color = c;
            yield return null;
        }
        status = FadeInStatus.Faded;
    }

    private static void CreateFadeInObject() {
        GameObject instance = new GameObject();
        Image img = instance.AddComponent<Image>();
        img.color = new Color32(0, 0, 0, 0);
        instance.transform.localScale = new Vector3(100, 100, 100);
        instance.transform.parent = GameObject.Find("CursorCanvas").transform;


        fadeInObject = img;
    }

    public static FadeInStatus GetFadeInStatus() {
        return status;
    }
}
