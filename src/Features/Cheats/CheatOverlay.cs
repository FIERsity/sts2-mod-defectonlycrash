using System.Collections.Generic;
using System.Reflection;
using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards.Holders;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.sts2.Core.Nodes.TopBar;

namespace ModTemplate;

public partial class CheatHealOverlay : CanvasLayer
{
    private static readonly FieldInfo TopBarHpPlayerField = typeof(NTopBarHp).GetField("_player", BindingFlags.Instance | BindingFlags.NonPublic);
    private static readonly FieldInfo EnergyCounterPlayerField = typeof(NEnergyCounter).GetField("_player", BindingFlags.Instance | BindingFlags.NonPublic);
    private static readonly FieldInfo InspectCardsField = typeof(NInspectCardScreen).GetField("_cards", BindingFlags.Instance | BindingFlags.NonPublic);
    private static readonly FieldInfo InspectIndexField = typeof(NInspectCardScreen).GetField("_index", BindingFlags.Instance | BindingFlags.NonPublic);

    private readonly Button _hpHotspot = CreateTransparentHotspot("Click to fully heal.");
    private readonly Button _energyHotspot = CreateTransparentHotspot("Click to fully restore energy.");
    private readonly Button _deleteButton = CreateDeleteButton();

    private NTopBarHp _topBarHp;
    private NEnergyCounter _energyCounter;
    private NInspectCardScreen _inspectCardScreen;

    public override void _Ready()
    {
        Name = "ModTemplateCheatOverlay";
        Layer = 100;
        ProcessMode = ProcessModeEnum.Always;

        _hpHotspot.Name = "HpHotspot";
        _hpHotspot.Pressed += OnHealPressed;
        AddChild(_hpHotspot);

        _energyHotspot.Name = "EnergyHotspot";
        _energyHotspot.Pressed += OnRefillEnergyPressed;
        AddChild(_energyHotspot);

        _deleteButton.Name = "DeleteButton";
        _deleteButton.Pressed += OnDeletePressed;
        AddChild(_deleteButton);
    }

    public override void _Process(double delta)
    {
        _topBarHp = ResolveNode(_topBarHp);
        _energyCounter = ResolveNode(_energyCounter);
        _inspectCardScreen = ResolveNode(_inspectCardScreen);

        UpdateHotspot(_hpHotspot, _topBarHp, GetTopBarPlayer(_topBarHp) != null);
        UpdateHotspot(_energyHotspot, _energyCounter, GetEnergyPlayer(_energyCounter)?.PlayerCombatState != null);
        UpdateInspectDeleteButton();
    }

    private async void OnHealPressed()
    {
        Player player = GetTopBarPlayer(_topBarHp);
        if (player == null)
        {
            MainFile.Logger.Warn("Heal hotspot pressed before a player was available.");
            return;
        }

        Creature creature = player.Creature;
        if (creature.CurrentHp >= creature.MaxHp)
        {
            return;
        }

        await CreatureCmd.SetCurrentHp(creature, creature.MaxHp);
        MainFile.Logger.Info($"Cheat heal applied: {creature.CurrentHp}/{creature.MaxHp}");
    }

    private void OnRefillEnergyPressed()
    {
        Player player = GetEnergyPlayer(_energyCounter);
        if (player?.PlayerCombatState == null)
        {
            MainFile.Logger.Warn("Energy hotspot pressed before combat state was available.");
            return;
        }

        PlayerCombatState combatState = player.PlayerCombatState;
        if (combatState.Energy >= combatState.MaxEnergy)
        {
            return;
        }

        combatState.Energy = combatState.MaxEnergy;
        MainFile.Logger.Info($"Cheat energy refill applied: {combatState.Energy}/{combatState.MaxEnergy}");
    }

    private async void OnDeletePressed()
    {
        CardModel card = GetInspectedCard(_inspectCardScreen);
        if (!CanDelete(card))
        {
            return;
        }

        _deleteButton.Disabled = true;

        switch (card.Pile.Type)
        {
            case PileType.Deck:
                await CardPileCmd.RemoveFromDeck(card, showPreview: false);
                break;
            case PileType.Hand:
            case PileType.Draw:
            case PileType.Discard:
            case PileType.Exhaust:
            case PileType.Play:
                await CardPileCmd.RemoveFromCombat(card, isBeingPlayed: false, skipVisuals: false);
                break;
        }

        MainFile.Logger.Info($"Deleted card via cheat UI: {card.Id.Entry}");
        _deleteButton.Visible = false;
        _deleteButton.Disabled = false;
        _inspectCardScreen?.Close();
    }

    private void UpdateInspectDeleteButton()
    {
        CardModel card = GetInspectedCard(_inspectCardScreen);
        if (_inspectCardScreen == null
            || !GodotObject.IsInstanceValid(_inspectCardScreen)
            || _inspectCardScreen.IsQueuedForDeletion()
            || !_inspectCardScreen.Visible
            || !CanDelete(card))
        {
            _deleteButton.Visible = false;
            _deleteButton.Disabled = false;
            return;
        }

        _deleteButton.Visible = true;
        _deleteButton.Disabled = false;
        Control upgradeTickbox = _inspectCardScreen.GetNodeOrNull<Control>("%Upgrade");
        Control upgradeLabel = _inspectCardScreen.GetNodeOrNull<Control>("%ShowUpgradeLabel");
        Control anchor = upgradeLabel ?? upgradeTickbox;

        if (anchor != null)
        {
            _deleteButton.GlobalPosition = anchor.GlobalPosition + new Vector2(138f, 44f);
        }
        else
        {
            _deleteButton.GlobalPosition = _inspectCardScreen.GlobalPosition + new Vector2(1040f, 290f);
        }
    }

    private static bool CanDelete(CardModel card)
    {
        if (card?.Owner == null || card.Pile == null)
        {
            return false;
        }

        return card.Pile.Type == PileType.Deck
            || card.Pile.Type == PileType.Hand
            || card.Pile.Type == PileType.Draw
            || card.Pile.Type == PileType.Discard
            || card.Pile.Type == PileType.Exhaust
            || card.Pile.Type == PileType.Play;
    }

    private static void UpdateHotspot(Control hotspot, Control target, bool shouldShow)
    {
        if (target == null || !GodotObject.IsInstanceValid(target) || target.IsQueuedForDeletion() || !shouldShow)
        {
            hotspot.Visible = false;
            return;
        }

        hotspot.Visible = true;
        hotspot.GlobalPosition = target.GlobalPosition;
        hotspot.Size = target.Size;
    }

    private static CardModel GetInspectedCard(NInspectCardScreen inspectCardScreen)
    {
        if (inspectCardScreen == null)
        {
            return null;
        }

        List<CardModel> cards = (List<CardModel>)InspectCardsField?.GetValue(inspectCardScreen);
        if (cards == null || cards.Count == 0)
        {
            return null;
        }

        int index = (int?)InspectIndexField?.GetValue(inspectCardScreen) ?? 0;
        if (index < 0 || index >= cards.Count)
        {
            return null;
        }

        return cards[index];
    }

    private static Player GetTopBarPlayer(NTopBarHp topBarHp)
    {
        return (Player)TopBarHpPlayerField?.GetValue(topBarHp);
    }

    private static Player GetEnergyPlayer(NEnergyCounter energyCounter)
    {
        return (Player)EnergyCounterPlayerField?.GetValue(energyCounter);
    }

    private T ResolveNode<T>(T existing) where T : Node
    {
        if (existing != null && GodotObject.IsInstanceValid(existing) && !existing.IsQueuedForDeletion())
        {
            return existing;
        }

        return FindDescendant<T>(GetTree()?.Root);
    }

    private static T FindDescendant<T>(Node node) where T : class
    {
        if (node == null)
        {
            return null;
        }

        if (node is T match)
        {
            return match;
        }

        foreach (Node child in node.GetChildren())
        {
            T nestedMatch = FindDescendant<T>(child);
            if (nestedMatch != null)
            {
                return nestedMatch;
            }
        }

        return null;
    }

    private static Button CreateTransparentHotspot(string tooltip)
    {
        return new Button
        {
            Visible = false,
            TooltipText = tooltip,
            FocusMode = Control.FocusModeEnum.None,
            Flat = true,
            MouseFilter = Control.MouseFilterEnum.Stop,
            Modulate = new Color(1f, 1f, 1f, 0.01f),
        };
    }

    private static Button CreateDeleteButton()
    {
        Button button = new()
        {
            Visible = false,
            Text = "删牌",
            TooltipText = "Delete this card.",
            FocusMode = Control.FocusModeEnum.None,
            MouseFilter = Control.MouseFilterEnum.Stop,
            Flat = true,
            Alignment = HorizontalAlignment.Left,
            CustomMinimumSize = new Vector2(96f, 32f),
            Size = new Vector2(96f, 32f),
            Modulate = Colors.White,
        };

        button.AddThemeFontSizeOverride("font_size", 18);
        button.AddThemeColorOverride("font_color", new Color(0.93f, 0.85f, 0.72f));
        button.AddThemeColorOverride("font_hover_color", new Color(1f, 0.93f, 0.82f));
        button.AddThemeColorOverride("font_pressed_color", new Color(0.78f, 0.66f, 0.54f));
        return button;
    }
}
