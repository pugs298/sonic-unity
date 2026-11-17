using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateWorld()
    {
        if (FindObjectOfType<PlayerController>() != null)
            return;

        var bootstrap = new GameObject("GameBootstrap");
        bootstrap.AddComponent<GameBootstrap>();
    }

    private void Awake()
    {
        BuildLights();
        BuildWorld();
        BuildPlayer();
        BuildCamera();
        BuildGoal();
    }

    private void BuildLights()
    {
        var sunlight = GameObject.Find("Sunlight");
        if (sunlight == null)
        {
            sunlight = new GameObject("Sunlight");
            var light = sunlight.AddComponent<Light>();
            light.type = LightType.Directional;
            light.intensity = 1.2f;
            light.transform.rotation = Quaternion.Euler(35f, -35f, 0f);
        }
    }

    private void BuildWorld()
    {
        var ground = GameObject.Find("Ground");
        if (ground == null)
        {
            ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(120f, 1f, 220f);
            ground.transform.position = new Vector3(0f, -1f, 80f);
            var groundRenderer = ground.GetComponent<Renderer>();
            groundRenderer.material = new Material(Shader.Find("Standard"));
            groundRenderer.material.color = new Color(0.2f, 0.7f, 0.35f);
        }

        for (int i = 0; i < 5; i++)
        {
            var ramp = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ramp.name = "Ramp" + i;
            ramp.transform.localScale = new Vector3(12f, 1f, 18f);
            ramp.transform.position = new Vector3(0f, 0.5f + i * 2f, 30f + i * 32f);
            var mat = ramp.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
            mat.color = new Color(0.33f, 0.45f, 0.28f);
        }

        for (int i = 0; i < 6; i++)
        {
            var boostPad = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            boostPad.name = "BoostPad" + i;
            boostPad.transform.localScale = new Vector3(2.5f, 0.25f, 2.5f);
            boostPad.transform.position = new Vector3(0f, 0.9f, 18f + i * 24f);
            boostPad.GetComponent<Renderer>().material.color = new Color(1f, 0.8f, 0.1f);
            var pad = boostPad.AddComponent<SpeedPad>();
            pad.force = 16f + i * 2f;
        }

        for (int i = 0; i < 14; i++)
        {
            var ring = GameObject.CreatePrimitive(PrimitiveType.Torus);
            ring.name = "Ring" + i;
            ring.transform.localScale = new Vector3(0.8f, 0.8f, 0.2f);
            ring.transform.position = new Vector3((i % 2 == 0 ? 6f : -6f), 2.5f + (i % 3), 10f + i * 12f);
            ring.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            var renderer = ring.GetComponent<Renderer>();
            renderer.material = new Material(Shader.Find("Standard"));
            renderer.material.color = new Color(1f, 0.9f, 0.2f);
            var trigger = ring.AddComponent<SphereCollider>();
            trigger.isTrigger = true;
            trigger.radius = 1.25f;
            ring.AddComponent<RingPickup>();
        }

        var cliff = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cliff.name = "Cliff";
        cliff.transform.localScale = new Vector3(30f, 10f, 10f);
        cliff.transform.position = new Vector3(0f, 2.5f, 180f);
        cliff.GetComponent<Renderer>().material.color = new Color(0.18f, 0.22f, 0.16f);
    }

    private void BuildPlayer()
    {
        var existing = GameObject.Find("Player");
        if (existing != null)
            return;

        var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 2f, 0f);
        player.transform.localScale = new Vector3(1f, 1.1f, 1f);

        var body = player.GetComponent<Renderer>();
        body.material = new Material(Shader.Find("Standard"));
        body.material.color = new Color(0.1f, 0.6f, 1f);

        var controller = player.AddComponent<CharacterController>();
        controller.height = 2f;
        controller.radius = 0.5f;
        controller.center = new Vector3(0f, 0.2f, 0f);

        player.AddComponent<PlayerController>();
    }

    private void BuildCamera()
    {
        var cam = Camera.main;
        if (cam == null)
        {
            var cameraObject = new GameObject("Main Camera");
            cam = cameraObject.AddComponent<Camera>();
            cameraObject.tag = "MainCamera";
        }

        cam.transform.position = new Vector3(0f, 5f, -12f);
        cam.transform.rotation = Quaternion.Euler(15f, 0f, 0f);
        cam.nearClipPlane = 0.1f;
        cam.GetComponent<CameraFollow>().SetTarget(GameObject.Find("Player").transform);
    }

    private void BuildGoal()
    {
        var goalObject = GameObject.Find("Goal");
        if (goalObject == null)
        {
            goalObject = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            goalObject.name = "Goal";
            goalObject.transform.localScale = new Vector3(3f, 0.35f, 3f);
            goalObject.transform.position = new Vector3(0f, 2f, 180f);
            goalObject.GetComponent<Renderer>().material.color = new Color(1f, 0.2f, 0.2f);
            goalObject.AddComponent<BoxCollider>().isTrigger = true;
            goalObject.AddComponent<GoalTrigger>();
        }
    }
}
