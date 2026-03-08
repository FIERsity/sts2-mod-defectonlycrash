using Godot;
using MegaCrit.Sts2.Core.Modding;

namespace ModTemplate;

[ModInitializer(nameof(Initialize))]
public partial class MainFile : Node
{
    private const string ModId = "ModTemplate"; //At the moment, this is used only for the Logger and harmony names.

    public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; } = new(ModId, MegaCrit.Sts2.Core.Logging.LogType.Generic);

    public static void Initialize()
    {
        if (Engine.GetMainLoop() is not SceneTree tree || tree.Root == null)
        {
            Logger.Error("Could not install cheat overlay because the scene tree was unavailable.");
            return;
        }

        CheatHealOverlay overlay = new();
        tree.Root.CallDeferred(Node.MethodName.AddChild, overlay);
        Logger.Info("ModTemplate cheat overlay installed.");
    }
}
