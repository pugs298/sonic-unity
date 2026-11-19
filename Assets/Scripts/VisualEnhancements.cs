using System.Collections;
using UnityEngine;

// Runtime-only visual pass for the procedurally generated Momentum Frontier prototype.
public class VisualEnhancements : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        if (FindObjectOfType<VisualEnhancements>() != null) return;
        new GameObject("Visual Enhancements").AddComponent<VisualEnhancements>();
    }

    private void Start()
    {
        BuildAtmosphere();
        BuildCoastalDetails();
        BuildTrackDetails();
        StartCoroutine(BuildPlayerDetails());
    }

    private static Material MaterialFor(Color color, float metallic = 0f, float smoothness = 0.15f)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null) shader = Shader.Find("Diffuse");
        Material material = new Material(shader);
        material.color = color;
        if (material.HasProperty("_Metallic") ) material.SetFloat("_Metallic", metallic);
        if (material.HasProperty("_Smoothness") ) material.SetFloat("_Smoothness", smoothness);
        if (material.HasProperty("_Glossiness") ) material.SetFloat("_Glossiness", smoothness);
        return material;
    }

    private static GameObject CreateShape(string name, PrimitiveType shape, Vector3 position, Vector3 scale, Color color)
    {
        GameObject item = GameObject.CreatePrimitive(shape);
        item.name = name;
        item.transform.position = position;
        item.transform.localScale = scale;
        item.GetComponent<Renderer>().sharedMaterial = MaterialFor(color);
        Collider collider = item.GetComponent<Collider>();
        if (collider != null) Destroy(collider);
        return item;
    }

    private void BuildAtmosphere()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        RenderSettings.fogDensity = 0.0018f;
        RenderSettings.fogColor = new Color(0.63f, 0.82f, 0.91f);

        Camera camera = Camera.main;
        if (camera != null)
        {
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.38f, 0.72f, 0.95f);
            camera.fieldOfView = 76f;
        }

        foreach (Light light in FindObjectsOfType<Light>())
        {
            if (light.type == LightType.Directional)
            {
                light.color = new Color(1f, 0.94f, 0.78f);
                light.intensity = 1.35f;
                light.shadows = LightShadows.Soft;
            }
        }

        // Wispy cloud banks made from soft, low-poly spheres.
        for (int i = 0; i < 18; i++)
        {
            float angle = i * 137.5f * Mathf.Deg2Rad;
            float radius = 125f + (i % 5) * 18f;
            Vector3 position = new Vector3(Mathf.Cos(angle) * radius, 72f + (i % 4) * 7f, Mathf.Sin(angle) * radius);
            for (int p = 0; p < 3; p++)
            {
                CreateShape("Cloud", PrimitiveType.Sphere,
                    position + new Vector3(p * 7f - 7f, (p == 1 ? 2f : 0f), 0f),
                    new Vector3(12f, 4f, 7f), new Color(0.94f, 0.98f, 1f));
            }
        }
    }

    private void BuildCoastalDetails()
    {
        // Mountains frame the horizon instead of leaving a flat, empty backdrop.
        for (int i = 0; i < 28; i++)
        {
            float angle = i * Mathf.PI * 2f / 28f;
            float radius = 175f + (i % 4) * 8f;
            Vector3 position = new Vector3(Mathf.Sin(angle) * radius, 7f, Mathf.Cos(angle) * radius);
            float height = 18f + (i % 7) * 2.5f;
            CreateShape("Distant Mountain", PrimitiveType.Cylinder, position,
                new Vector3(height * 0.7f, height, height * 0.7f),
                i % 3 == 0 ? new Color(0.25f, 0.38f, 0.32f) : new Color(0.34f, 0.43f, 0.36f));
        }

        // Stylized trees around the outer meadow.
        for (int i = 0; i < 55; i++)
        {
            float angle = i * 2.39996323f;
            float radius = 92f + (i % 9) * 5f;
            Vector3 basePosition = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius);
            CreateShape("Palm Trunk", PrimitiveType.Cylinder, basePosition + Vector3.up * 3f,
                new Vector3(0.65f, 3f, 0.65f), new Color(0.42f, 0.25f, 0.12f));
            CreateShape("Palm Crown", PrimitiveType.Sphere, basePosition + Vector3.up * 6.5f,
                new Vector3(4f, 3.2f, 4f), i % 2 == 0 ? new Color(0.12f, 0.48f, 0.18f) : new Color(0.16f, 0.58f, 0.20f));
        }

        // Small turquoise highlight strips make the meadow feel less monochrome.
        for (int i = 0; i < 16; i++)
        {
            float z = -120f + i * 18f;
            CreateShape("Meadow Stream " + i, PrimitiveType.Cube, new Vector3(-82f + (i % 3) * 2f, 0.08f, z),
                new Vector3(2.2f, 0.08f, 8f), new Color(0.12f, 0.64f, 0.72f));
        }
    }

    private void BuildTrackDetails()
    {
        // Wide coloured strips define the main route and make speed easier to read.
        for (int i = 0; i < 28; i++)
        {
            float z = -112f + i * 9f;
            float sway = Mathf.Sin(i * 0.32f) * 3f;
            CreateShape("Route Marker Left", PrimitiveType.Cube, new Vector3(-7f + sway, 0.12f, z),
                new Vector3(0.5f, 0.08f, 4f), new Color(0.95f, 0.82f, 0.15f));
            CreateShape("Route Marker Right", PrimitiveType.Cube, new Vector3(7f + sway, 0.12f, z),
                new Vector3(0.5f, 0.08f, 4f), new Color(0.95f, 0.82f, 0.15f));
        }

        // Bright goal pillars and a larger finish arch improve visual hierarchy.
        Vector3 goalCenter = new Vector3(0f, 0f, 145f);
        foreach (float x in new[] { -6f, 6f })
        {
            CreateShape("Goal Arch Pillar", PrimitiveType.Cube, goalCenter + new Vector3(x, 4f, 0f),
                new Vector3(1.2f, 8f, 1.2f), new Color(0.16f, 0.75f, 0.92f));
        }
        CreateShape("Goal Arch", PrimitiveType.Cube, goalCenter + Vector3.up * 8f,
            new Vector3(13f, 1.2f, 1.2f), new Color(1f, 0.82f, 0.12f));
    }

    private IEnumerator BuildPlayerDetails()
    {
        yield return null;
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) yield break;

        Transform root = player.transform;
        CreateShape("Player Face", PrimitiveType.Sphere, root.position + Vector3.up * 1.55f,
            new Vector3(0.62f, 0.62f, 0.62f), new Color(0.14f, 0.23f, 0.72f)).transform.SetParent(root, true);

        for (int i = 0; i < 2; i++)
        {
            float side = i == 0 ? -1f : 1f;
            CreateShape("Player Shoe", PrimitiveType.Sphere,
                root.position + new Vector3(side * 0.30f, 0.16f, 0.10f),
                new Vector3(0.38f, 0.22f, 0.62f), new Color(0.92f, 0.18f, 0.12f)).transform.SetParent(root, true);
        }

        // A subtle speed streak follows the character while it moves.
        TrailRenderer trail = root.GetComponent<TrailRenderer>();
        if (trail == null) trail = root.gameObject.AddComponent<TrailRenderer>();
        trail.time = 0.28f;
        trail.startWidth = 0.22f;
        trail.endWidth = 0.02f;
        trail.minVertexDistance = 0.12f;
        trail.material = MaterialFor(new Color(0.32f, 0.75f, 1f, 0.75f), 0f, 0.5f);
    }
}
