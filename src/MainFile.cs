using Godot;
using MegaCrit.Sts2.Core.Modding;

namespace DefectOnlyCrash;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    private const string ModId = "DefectOnlyCrash";

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        if (Engine.GetMainLoop() is not SceneTree tree || tree.Root == null)
        {
            Logger.Error("Could not install crash monitor because the scene tree was unavailable.");
            return;
        }

        CharacterCrashMonitor monitor = new();
        tree.Root.CallDeferred(Node.MethodName.AddChild, monitor);
        Logger.Info("DefectOnlyCrash monitor installed.");
    }
}
