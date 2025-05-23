using Content.Client.UserInterface.Controls;
using Content.Shared.Cargo.Components;
using Content.Shared.Cargo.Prototypes;
using Content.Shared.Cargo;
using Content.Shared._Adventure.Sponsors;
using Robust.Client.AutoGenerated;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;
using Robust.Shared.Prototypes;
using System.Linq;
using static Robust.Client.UserInterface.Controls.BaseButton;

namespace Content.Client._Adventure.Administration.SponsorChange;

[GenerateTypedNameReferences]
public sealed partial class SponsorChangeWindow : FancyWindow
{
    public event Action<string>? OnUsernameEntered;
    public event Action<string?>? OnTierSelected;
    // List of all possible sponsor tiers, "not a sponsor" is not a tier, so not nullable.
    public List<ProtoId<SponsorTierPrototype>> _protos;
    public SponsorChangeWindow(List<ProtoId<SponsorTierPrototype>> protos)
    {
        RobustXamlLoader.Load(this);
        _protos = protos;
        PlayerNameLine.OnTextEntered += e => OnUsernameEntered?.Invoke(e.Text);
        PlayerNameLine.OnFocusExit += e => OnUsernameEntered?.Invoke(e.Text);
        for (int i = 0; i < _protos.Count; i++)
        {
            var proto = protos[i];
            SponsorTierOption.AddItem(proto.Id, i);
        }
        SponsorTierOption.AddItem("По умолчанию", -1);
        // SponsorTierOption.OnItemSelected += args => SponsorTierOption.SelectId(args.Id);
        SponsorTierOption.OnItemSelected += TierSelected;
    }

    private void TierSelected(OptionButton.ItemSelectedEventArgs args)
    {
        if (args.Id == -1)
        {
            OnTierSelected?.Invoke(null);
            return;
        }
        OnTierSelected?.Invoke(_protos[args.Id].Id);
    }

    // I don't care if it can silently fail, validate your shit yourself!
    public void SelectSponsorTier(ProtoId<SponsorTierPrototype>? proto)
    {
        if (proto == null)
        {
            SponsorTierOption.SelectId(-1);
            return;
        }
        var ind = _protos.IndexOf(proto.Value);
        if (ind == -1) return;
        SponsorTierOption.SelectId(ind);
    }

    public void SetMenuEnabled(bool enabled)
    {
        SponsorTierOption.Disabled = !enabled;
    }

    public void SetPlayerName(string name)
    {
        PlayerNameLine.Text = name;
    }

    public string GetUsername()
    {
        return PlayerNameLine.Text;
    }
}
