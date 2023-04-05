using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class SceneFader {
    public enum FadeInStatus { None, Fading, Faded};

    private static Image fadeInObject;
    private static FadeInStatus status = FadeInStatus.None;

    public static IEnumerator FadeIn(float duration) {
        if (!fadeInObject)
            CreateFadeInObject();

        float alpha = 1f;
        status = FadeInStatus.Fading;
        while (alpha > 0) {
            alpha -= Time.deltaTime / duration;
            fadeInObject.color = new Color(0, 0, 0, alpha);
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
            fadeInObject.color = new Color(0, 0, 0, alpha);
            yield return null;
        }
        status = FadeInStatus.Faded;
    }

    private static void CreateFadeInObject() {
        GameObject instance = new GameObject();
        Image img = instance.AddComponent<Image>();
        img.color = new Color32(0, 0, 0, 0);
		img.raycastTarget = false;
        instance.transform.localScale = new Vector3(100, 100, 100);
        instance.transform.parent = GameObject.Find("FadeOutCanvas").transform;
		Object.DontDestroyOnLoad(img.transform.root);
        fadeInObject = img;
    }

    public static FadeInStatus GetFadeInStatus() {
        return status;
    }
}
