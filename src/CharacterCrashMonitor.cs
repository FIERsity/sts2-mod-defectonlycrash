using System;
using System.Reflection;
using Godot;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;
using MegaCrit.Sts2.Core.Timeline.Epochs;

namespace DefectOnlyCrash;

public partial class CharacterCrashMonitor : Node
{
    private static readonly FieldInfo RunStateField = typeof(NRun).GetField("_state", BindingFlags.Instance | BindingFlags.NonPublic);
    private static readonly string DefectEpochId = EpochModel.GetId<Defect1Epoch>();
    private bool _unlockAttempted;
    private bool _hasTriggered;

    public override void _Ready()
    {
        Name = "DefectOnlyCrashMonitor";
        ProcessMode = ProcessModeEnum.Always;
    }

    public override void _Process(double delta)
    {
        try
        {
            EnsureDefectUnlocked();
        }
        catch (Exception ex)
        {
            _unlockAttempted = true;
            MainFile.Logger.Error($"Failed to auto-unlock Defect: {ex}");
        }

        if (_hasTriggered)
        {
            return;
        }

        if (!RunManager.Instance.IsInProgress)
        {
            return;
        }

        NRun runNode = NRun.Instance;
        if (runNode == null || RunStateField == null)
        {
            return;
        }

        RunState runState = (RunState)RunStateField.GetValue(runNode);
        if (runState == null)
        {
            return;
        }

        Player player = LocalContext.GetMe(runState);
        if (player == null)
        {
            return;
        }

        if (player.Character is Defect)
        {
            return;
        }

        _hasTriggered = true;
        string characterName = player.Character.Id.Entry;
        MainFile.Logger.Error($"Non-Defect character detected: {characterName}. Terminating process.");
        System.Environment.FailFast($"DefectOnlyCrash: character {characterName} is not allowed.");
    }

    private void EnsureDefectUnlocked()
    {
        if (_unlockAttempted)
        {
            return;
        }

        SaveManager saveManager = SaveManager.Instance;
        if (!IsSaveManagerReady(saveManager) || saveManager.Progress == null)
        {
            return;
        }

        bool defectRevealed = saveManager.Progress.Epochs.Any(epoch => epoch.Id == DefectEpochId && epoch.State >= EpochState.Revealed);
        if (defectRevealed)
        {
            _unlockAttempted = true;
            return;
        }

        saveManager.ObtainEpochOverride(DefectEpochId, EpochState.Revealed);
        saveManager.SaveProgressFile();
        _unlockAttempted = true;
        MainFile.Logger.Info("Auto-unlocked Defect by revealing Defect1Epoch.");
    }

    private static bool IsSaveManagerReady(SaveManager saveManager)
    {
        try
        {
            _ = saveManager.CurrentProfileId;
            return true;
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }
}
