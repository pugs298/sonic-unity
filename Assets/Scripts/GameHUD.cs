using UnityEngine;

public class GameHUD : MonoBehaviour
{
    private GUIStyle titleStyle;
    private GUIStyle bodyStyle;
    private GUIStyle messageStyle;

    private void OnGUI()
    {
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player == null) return;
        EnsureStyles();
        GUI.Box(new Rect(18f, 18f, 275f, 120f), GUIContent.none);
        GUI.Label(new Rect(32f, 25f, 245f, 30f), "MOMENTUM FRONTIER", titleStyle);
        GUI.Label(new Rect(32f, 57f, 240f, 25f), "RINGS   " + player.Rings + "      SPEED   " + Mathf.RoundToInt(player.Speed), bodyStyle);
        GUI.Label(new Rect(32f, 84f, 90f, 20f), "BOOST", bodyStyle);
        GUI.Box(new Rect(95f, 87f, 170f, 14f), GUIContent.none);
        GUI.color = new Color(1f, 0.75f, 0.1f);
        GUI.Box(new Rect(97f, 89f, 166f * player.BoostMeter / 100f, 10f), GUIContent.none);
        GUI.color = Color.white;
        GUI.Label(new Rect(18f, Screen.height - 65f, Screen.width - 36f, 28f), "WASD / Arrows: move    Space: jump    Shift: boost    E: homing dash    Right mouse: look", bodyStyle);
        if (player.StatusMessage != "" && (player.Finished || player.StatusMessage.Contains("reached") || player.StatusMessage.Contains("collected") || player.StatusMessage.Contains("BOOST") || player.StatusMessage.Contains("checkpoint")))
            GUI.Label(new Rect(0f, 155f, Screen.width, 40f), player.StatusMessage, messageStyle);
        if (player.Finished)
        {
            GUI.Box(new Rect(Screen.width / 2f - 190f, Screen.height / 2f - 55f, 380f, 110f), GUIContent.none);
            GUI.Label(new Rect(Screen.width / 2f - 170f, Screen.height / 2f - 40f, 340f, 70f), "ZONE CLEAR!\nRings collected: " + player.Rings, messageStyle);
        }
    }

    private void EnsureStyles()
    {
        if (titleStyle != null) return;
        titleStyle = new GUIStyle(GUI.skin.label) { fontSize = 17, fontStyle = FontStyle.Bold, normal = { textColor = Color.white } };
        bodyStyle = new GUIStyle(GUI.skin.label) { fontSize = 13, normal = { textColor = Color.white } };
        messageStyle = new GUIStyle(GUI.skin.label) { fontSize = 22, fontStyle = FontStyle.Bold, alignment = TextAnchor.MiddleCenter, normal = { textColor = Color.white } };
    }
}
