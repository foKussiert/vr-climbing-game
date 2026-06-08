using UnityEngine;

public class HandVisibility : MonoBehaviour
{
    public Renderer handRenderer;
    public float transparentAlpha = 0.3f;
    public float fadeSpeed = 5f;

    private float targetAlpha = 1f;
    private Material mat;

    void Start()
    {
        mat = handRenderer.material;
    }

    void Update()
    {
        Color c = mat.color;
        c.a = Mathf.Lerp(c.a, targetAlpha, Time.deltaTime * fadeSpeed);
        mat.color = c;
    }

    public void SetTransparent(bool transparent)
    {
        targetAlpha = transparent ? transparentAlpha : 1f;
    }
}
