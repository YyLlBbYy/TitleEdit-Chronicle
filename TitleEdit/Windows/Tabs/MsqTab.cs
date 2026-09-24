using Dalamud.Interface.Components;
using Dalamud.Interface.Utility.Raii;
using Dalamud.Bindings.ImGui;
using TitleEdit.Data.Msq;
using TitleEdit.Data.Persistence;
using TitleEdit.Utility;

namespace TitleEdit.Windows.Tabs
{
    internal class MsqTab : AbstractTab
    {
        public override string Title => "MSQ";

        public override void Draw()
        {
            base.Draw();
            using var wrap = ImRaii.TextWrapPos(ImGui.GetFontSize() * 28);

            ImGui.TextWrapped("Chronicle follows the Main Scenario of the character you are currently playing and sets that expansion's vanilla title screen + idle movie for the next time you see the lobby. It does not use the furthest character on the account, and it is not locked on Free Trial.");

            ImGui.Separator();

            var tracker = Services.MsqTrackerService;
            var cfg = Services.ConfigurationService;
            var next = tracker.GetExpansionForNextLogin();
            var name = string.IsNullOrEmpty(tracker.CurrentCharacterName)
                ? (string.IsNullOrEmpty(cfg.LastLoggedCharacterName) ? "No character observed yet" : cfg.LastLoggedCharacterName)
                : tracker.CurrentCharacterName;

            ImGui.TextUnformatted("Logged-in character");
            ImGui.TextWrapped(name);
            ImGui.TextUnformatted($"Detected expansion: {tracker.CurrentExpansion}");
            ImGui.TextUnformatted($"Gate quest: {tracker.CurrentGateQuest}");
            ImGui.TextUnformatted($"Next title / movie: {next}");
            ImGui.TextUnformatted($"Preset: {MsqVanillaPresets.PresetPath(next)}");

            ImGui.Separator();

            var follow = cfg.TitleDisplayTypeOption.Type == TitleDisplayType.MsqProgress;
            if (ImGui.Checkbox("Follow this character's MSQ (recommended)", ref follow))
            {
                cfg.TitleDisplayTypeOption = new()
                {
                    Type = follow ? TitleDisplayType.MsqProgress : TitleDisplayType.Preset,
                    PresetPath = follow ? null : "?/ARealmReborn.json"
                };
                cfg.Save();
                Services.LobbyService.ReloadTitleScreen();
            }

            ImGuiComponents.HelpMarker("When enabled, Display → Title screen setting is driven by this tab. Disable it from Display if you want a fixed or random preset instead.");

            if (ImGui.Checkbox("Apply as soon as you return to the title screen", ref cfg.ApplyMsqTitleImmediately))
            {
                cfg.Save();
            }

            ImGuiComponents.HelpMarker("Vanilla 7.1 waits for a subsequent launch. Chronicle reloads the lobby immediately on logout, without restarting the client.");

            if (ImGui.Checkbox("Ignore Free Trial title lock", ref cfg.BypassFreeTrialTitleLock))
            {
                cfg.Save();
            }

            ImGuiComponents.HelpMarker("Free Trial accounts are forced onto a single title in vanilla. Chronicle still uses the expansions your client actually has files for (ARR through Shadowbringers on trial).");

            if (ImGui.Button("Rescan MSQ now"))
            {
                tracker.RefreshFromClient("manual");
                tracker.ApplyToLobbyIfOnTitle();
            }
        }
    }
}
