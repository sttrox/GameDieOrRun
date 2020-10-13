using System.Collections;
using UnityEngine;

public class ControllerColor : MonoBehaviour
{
    public Color baseColor;
    public Color endColor;

    /// <summary>
    /// Время затухания цвета
    /// </summary>
    public float timeDamping;

    private float _tempTimeDamping;
    private Renderer _renderer;

    private readonly Gradient _gradient = new Gradient();

    // Start is called before the first frame update


    void Start()
    {
        _renderer = GetComponent<Renderer>();
        SetColor(_renderer, baseColor);

        _gradient.SetKeys(new GradientColorKey[]
        {
            new GradientColorKey() {color = baseColor, time = 0},
            new GradientColorKey() {color = endColor, time = 1f},
        }, new GradientAlphaKey[]
        {
            new GradientAlphaKey() {alpha = 1, time = 0},
            new GradientAlphaKey() {alpha = 1, time = 1},
        });
    }

    public void ActivateDamping()
    {
        StartCoroutine(ProcessingGradientColor().GetEnumerator());
    }

    private IEnumerable ProcessingGradientColor()
    {
        do
        {
            _tempTimeDamping += Time.deltaTime;
            var coefficient = timeDamping / _tempTimeDamping;
            var color = DumpingColor(_gradient, coefficient);
            SetColor(_renderer, color);
            yield return null;
        } while (_tempTimeDamping <= timeDamping);

        yield break;
    }

    private Color DumpingColor(Gradient gradient, float coefficient)
    {
        return gradient.Evaluate(coefficient);
    }

    private void SetColor(Renderer renderer, Color color)
    {
        var material = renderer.material;
        material.SetColor("_Color", color);
    }
}