using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public static class WebGLBuild
{
    public static void Build()
    {
        const string sceneDirectory = "Assets/Scenes";
        const string scenePath = sceneDirectory + "/WebGLBootstrap.unity";
        Directory.CreateDirectory(sceneDirectory);

        Scene scene = EditorSceneManager.NewScene(
            NewSceneSetup.EmptyScene,
            NewSceneMode.Single);

        if (!EditorSceneManager.SaveScene(scene, scenePath))
            throw new System.Exception("Could not create the WebGL bootstrap scene.");

        EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.WebGL,
            BuildTarget.WebGL);

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { scenePath },
            locationPathName = "build/WebGL",
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result != BuildResult.Succeeded)
            throw new System.Exception(
                "WebGL build failed: " + report.summary.result);
    }
}
