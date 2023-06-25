using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlickeringEmissive : MonoBehaviour
{
    [SerializeField] private bool Flicker;
    [SerializeField] [Min(0)] private float FlickerSpeed = 1f;

    [SerializeField] AnimationCurve BrightnessCurve;

    private Renderer Renderer;
    private List<Material> Materials = new();
    private List<Color> InitialColors = new();

    private const string EMISSIVE_COLOR_NAME = "_EmissionColor";
    private const string EMISSIVE_KEYWORD = "_EMISSION";
    // Start is called before the first frame update
    void Awake()
    {
        Renderer = GetComponent<Renderer>();
        BrightnessCurve.postWrapMode = WrapMode.Loop;

        foreach(Material material in Renderer.materials) {
            if ( Renderer.material.HasColor(EMISSIVE_COLOR_NAME))
            {
                Materials.Add(material);
                InitialColors.Add(material.GetColor(EMISSIVE_COLOR_NAME));
            }
            else
            {
                Debug.LogWarning($"{material.name} material not emissive");
            }
        }

        if (Materials.Count == 0)
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Flicker && Renderer.isVisible) {
            float scaledTime = Time.time * FlickerSpeed;

            for (int i = 0; i < Materials.Count; i++)
            {
                Color color = InitialColors[i];
                float brightness = BrightnessCurve.Evaluate(scaledTime);
                color = new Color(
                    color.r * Mathf.Pow(2, brightness),
                    color.g * Mathf.Pow(2, brightness),
                    color.b * Mathf.Pow(2, brightness),
                    color.a
                );
                Materials[i].SetColor(EMISSIVE_COLOR_NAME, color);
            }
        }

   
    }

    IEnumerator TurnOffCoroutine()
    {
        //yield on a new YieldInstruction that waits for 115 seconds.
        yield return new WaitForSeconds(115);
        Flicker = false;
    }
}
