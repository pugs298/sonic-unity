using UnityEngine;

public class GameBootstrap : MonoBehaviour
{
    private static bool built;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateWorld()
    {
        if (built || FindObjectOfType<PlayerController>() != null) return;
        built = true;
        new GameObject("MomentumFrontierRuntime").AddComponent<GameBootstrap>();
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        BuildLighting();
        BuildWorld();
        PlayerController player = BuildPlayer();
        BuildCamera(player);
        BuildHUD();
    }

    private static Material MakeMaterial(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null) shader = Shader.Find("Diffuse");
        Material material = new Material(shader);
        material.color = color;
        return material;
    }

    private static GameObject Cube(string name, Vector3 position, Vector3 scale, Color color, Quaternion rotation = default(Quaternion))
    {
        GameObject item = GameObject.CreatePrimitive(PrimitiveType.Cube);
        item.name = name;
        item.transform.position = position;
        item.transform.localScale = scale;
        if (rotation != default(Quaternion)) item.transform.rotation = rotation;
        item.GetComponent<Renderer>().sharedMaterial = MakeMaterial(color);
        return item;
    }

    private static void BuildLighting()
    {
        RenderSettings.ambientLight = new Color(0.55f, 0.63f, 0.72f);
        GameObject sun = new GameObject("Sunlight");
        Light light = sun.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.15f;
        sun.transform.rotation = Quaternion.Euler(45f, -32f, 0f);
        Camera camera = Camera.main;
        if (camera != null) camera.backgroundColor = new Color(0.48f, 0.77f, 0.95f);
    }

    private static void BuildWorld()
    {
        Cube("Open Zone - Meadow", new Vector3(0f, -1f, 0f), new Vector3(340f, 2f, 340f), new Color(0.24f, 0.55f, 0.25f));
        // Raised exploration platforms form several routes across the open zone.
        Vector3[] platforms = {
            new Vector3(-48, 3, -58), new Vector3(42, 5, -42), new Vector3(-62, 7, 5),
            new Vector3(55, 4, 28), new Vector3(-36, 8, 64), new Vector3(34, 10, 78),
            new Vector3(72, 6, 112), new Vector3(-75, 4, 105), new Vector3(0, 13, 118),
            new Vector3(95, 10, -8), new Vector3(-100, 8, -38), new Vector3(10, 6, -92)
        };
        for (int i = 0; i < platforms.Length; i++)
        {
            Vector3 p = platforms[i];
            Cube("Floating Isle " + (i + 1), p, new Vector3(25f, 3f, 22f), new Color(0.31f, 0.64f, 0.29f));
            Cube("Isle Stone " + (i + 1), p + Vector3.down * 4f, new Vector3(18f, 6f, 16f), new Color(0.36f, 0.34f, 0.29f));
            if (i % 2 == 0)
            {
                GameObject ramp = Cube("Launch Ramp " + i, p + new Vector3(0f, -0.5f, -13f), new Vector3(10f, 1.2f, 18f), new Color(0.55f, 0.48f, 0.29f), Quaternion.Euler(-16f, 0f, 0f));
            }
        }

        // Readable landmarks around the play space.
        for (int i = 0; i < 16; i++)
        {
            float angle = i * Mathf.PI * 2f / 16f;
            Vector3 pos = new Vector3(Mathf.Sin(angle) * 145f, 10f, Mathf.Cos(angle) * 145f);
            GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pillar.name = "Zone Landmark";
            pillar.transform.position = pos;
            pillar.transform.localScale = new Vector3(3f, 10f, 3f);
            pillar.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.42f, 0.39f, 0.31f));
        }

        // Boost lanes and spring launches create traversal links between islands.
        for (int i = 0; i < 8; i++)
        {
            Vector3 pos = new Vector3((i % 2 == 0 ? -1f : 1f) * (18f + (i % 3) * 12f), 0.12f, -70f + i * 25f);
            GameObject pad = Cube("Boost Pad", pos, new Vector3(4f, 0.22f, 7f), new Color(1f, 0.72f, 0.08f));
            pad.AddComponent<SpeedPad>().boostSpeed = 34f;
        }
        Vector3[] springPositions = { new Vector3(-42, 0, -25), new Vector3(45, 0, 5), new Vector3(-18, 0, 48), new Vector3(20, 0, 95) };
        foreach (Vector3 pos in springPositions)
        {
            GameObject spring = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            spring.name = "Spring Launcher";
            spring.transform.position = pos + Vector3.up * 0.55f;
            spring.transform.localScale = new Vector3(2f, 0.55f, 2f);
            spring.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(1f, 0.25f, 0.15f));
            spring.AddComponent<SpringLauncher>().launchPower = 24f;
        }

        // Collectible trails guide the player through low and high routes.
        for (int i = 0; i < 72; i++)
        {
            float t = i / 71f;
            Vector3 pos = new Vector3(Mathf.Sin(t * Mathf.PI * 4f) * 12f, 1.7f + Mathf.Sin(t * Mathf.PI * 8f) * 1.2f, -112f + t * 225f);
            CreateRing(pos);
        }
        Vector3[] bonusRoutes = { new Vector3(-48, 5, -58), new Vector3(42, 7, -42), new Vector3(-62, 9, 5), new Vector3(55, 6, 28), new Vector3(-36, 10, 64), new Vector3(34, 12, 78), new Vector3(0, 15, 118) };
        foreach (Vector3 center in bonusRoutes)
            for (int i = 0; i < 6; i++) CreateRing(center + new Vector3((i - 2.5f) * 2.2f, 2f + Mathf.Sin(i * 0.7f), 0f));

        // Homing targets are glowing floating orbs placed beside the elevated routes.
        Vector3[] targets = { new Vector3(-42, 7, -58), new Vector3(48, 9, -42), new Vector3(-57, 11, 5), new Vector3(60, 8, 28), new Vector3(-30, 12, 64), new Vector3(39, 14, 78), new Vector3(0, 17, 118) };
        foreach (Vector3 p in targets) CreateHomingTarget(p);

        Vector3[] checkpoints = { new Vector3(0, 0, -48), new Vector3(0, 0, 32), new Vector3(0, 0, 100) };
        for (int i = 0; i < checkpoints.Length; i++) CreateCheckpoint(checkpoints[i], i + 1);
        CreateGoal(new Vector3(0f, 0f, 145f));
    }

    private static void CreateRing(Vector3 position)
    {
        GameObject ring = new GameObject("Energy Ring");
        ring.transform.position = position;
        SphereCollider trigger = ring.AddComponent<SphereCollider>();
        trigger.isTrigger = true;
        trigger.radius = 1.05f;
        ring.AddComponent<RingPickup>();
        ring.AddComponent<RingVisual>();
        for (int i = 0; i < 10; i++)
        {
            float angle = i * Mathf.PI * 2f / 10f;
            GameObject bead = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bead.name = "Ring Bead";
            bead.transform.SetParent(ring.transform, false);
            bead.transform.localPosition = new Vector3(Mathf.Cos(angle) * 0.48f, Mathf.Sin(angle) * 0.48f, 0f);
            bead.transform.localScale = Vector3.one * 0.16f;
            bead.GetComponent<Collider>().enabled = false;
            bead.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(1f, 0.82f, 0.12f));
        }
    }

    private static void CreateHomingTarget(Vector3 position)
    {
        GameObject target = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        target.name = "Homing Target";
        target.transform.position = position;
        target.transform.localScale = Vector3.one * 1.25f;
        target.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.1f, 0.9f, 1f));
        target.GetComponent<Collider>().isTrigger = true;
        target.AddComponent<HomingTarget>();
    }

    private static void CreateCheckpoint(Vector3 position, int index)
    {
        GameObject checkpoint = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        checkpoint.name = "Checkpoint " + index;
        checkpoint.transform.position = position + Vector3.up * 1.4f;
        checkpoint.transform.localScale = new Vector3(1.3f, 1.4f, 1.3f);
        checkpoint.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.2f, 0.85f, 0.35f));
        checkpoint.GetComponent<Collider>().isTrigger = true;
        checkpoint.AddComponent<Checkpoint>().checkpointPosition = position + Vector3.up * 0.1f;
    }

    private static void CreateGoal(Vector3 position)
    {
        GameObject goal = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        goal.name = "Zone Goal";
        goal.transform.position = position + Vector3.up * 0.25f;
        goal.transform.localScale = new Vector3(5f, 0.25f, 5f);
        goal.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(1f, 0.3f, 0.15f));
        goal.GetComponent<Collider>().isTrigger = true;
        goal.AddComponent<GoalTrigger>();
        GameObject beacon = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        beacon.name = "Goal Beacon";
        beacon.transform.position = position + Vector3.up * 5f;
        beacon.transform.localScale = new Vector3(0.45f, 5f, 0.45f);
        beacon.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(1f, 0.85f, 0.2f));
    }

    private static PlayerController BuildPlayer()
    {
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Player";
        player.tag = "Player";
        player.transform.position = new Vector3(0f, 0.2f, -130f);
        player.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.1f, 0.45f, 1f));
        Collider primitiveCollider = player.GetComponent<Collider>();
        Destroy(primitiveCollider);
        CharacterController character = player.AddComponent<CharacterController>();
        character.height = 1.8f;
        character.radius = 0.42f;
        character.center = new Vector3(0f, 0.9f, 0f);
        return player.AddComponent<PlayerController>();
    }

    private static void BuildCamera(PlayerController player)
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            GameObject cameraObject = new GameObject("Main Camera");
            camera = cameraObject.AddComponent<Camera>();
            cameraObject.tag = "MainCamera";
            cameraObject.AddComponent<AudioListener>();
        }
        camera.fieldOfView = 72f;
        camera.nearClipPlane = 0.1f;
        CameraFollow follow = camera.GetComponent<CameraFollow>();
        if (follow == null) follow = camera.gameObject.AddComponent<CameraFollow>();
        follow.SetTarget(player.transform);
    }

    private static void BuildHUD()
    {
        if (FindObjectOfType<GameHUD>() == null) new GameObject("Game HUD").AddComponent<GameHUD>();
    }
}
