using Dalamud.Game.Command;
using Dalamud.Game.Gui;
using Dalamud.IoC;
using Dalamud.Plugin;
using ImGuiNET;
using System;
using System.Numerics;

namespace CinemaScreen
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "Mannerplexx Screen";

        private const string CommandName = "/pscreen";

        [PluginService]
        [RequiredVersion("1.0")]
        private DalamudPluginInterface PluginInterface { get; set; }
        [PluginService]
        [RequiredVersion("1.0")]
        private CommandManager CommandManager { get; set; }
        [PluginService]
        private GameGui GameGui { get; set; }
        private Configuration Configuration { get; init; }

        private bool Visible = false;
        private bool SettingsVisible = false;
        private ScreenWindow Screen;

        public Plugin() {
            Configuration = PluginInterface.GetPluginConfig() as Configuration ?? new Configuration();
            Configuration.Initialize(PluginInterface);

            CommandManager.AddHandler(CommandName, new CommandInfo(OnCommand)
            {
                HelpMessage = "A useful message to display in /xlhelp"
            });

            PluginInterface.UiBuilder.Draw += DrawUI;
            PluginInterface.UiBuilder.OpenConfigUi += ToggleConfig;
        }

        public void Dispose()
        {
            CommandManager.RemoveHandler(CommandName);
        }

        [STAThread]
        private void OnCommand(string command, string args)
        {
            //Visible = !Visible;
            //if (Screen == null) { Screen = new(); }
            try
            {
                Screen = new();
                Screen.Show();
            } catch (Exception ex)
            {
                Dalamud.Logging.PluginLog.Error(ex, "Error on screen load");
            }
        }

        private void ToggleConfig()
        {
            SettingsVisible = !SettingsVisible;
        }

        private void DrawUI()
        {
            if (Screen != null)
            {
                float worldX = -1.5f;
                GameGui.WorldToScreen(new Vector3(worldX, -2.4f, -4f), out Vector2 topleft);
                Screen.Update(topleft);
            }
            DrawScreen();
            DrawConfigUI();
        }

        private void DrawScreen()
        {
            if (!Visible)
            {
                return;
            }

            float worldX = -1.5f;

            //MediaElement mediaElement = new MediaElement();

            GameGui.WorldToScreen(new Vector3(worldX, -2.4f, -4f), out Vector2 topleft);
            GameGui.WorldToScreen(new Vector3(worldX, -2.4f, 4f), out Vector2 topright);
            GameGui.WorldToScreen(new Vector3(worldX, -6.5f, -4f), out Vector2 bottomleft);
            GameGui.WorldToScreen(new Vector3(worldX, -6.5f, 4f), out Vector2 bottomright);
            float width = topright.X - topleft.X;
            float height = bottomleft.Y - topleft.Y;
            Vector2 size = new Vector2(width, height);

            //Dalamud.Interface.ImGuiHelpers.ForceNextWindowMainViewport();
            ImGui.Begin("Screen");
            ImGui.SetWindowPos(topleft);
            ImGui.SetWindowSize(size);
            //ImGui.Text($"topleft is {topleft}");
            //ImGui.Text($"topright is {topright}");
            //ImGui.Text($"bottomleft is {bottomleft}");
            //ImGui.Text($"bottomright is {bottomright}");           
            ImGui.End();
        }

        private void DrawConfigUI()
        {
            if (!SettingsVisible)
            {
                return;
            }

            ImGui.SetNextWindowSize(new Vector2(232, 75), ImGuiCond.Always);
            ImGui.Begin("A Wonderful Configuration Window", ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);
            // can't ref a property, so use a local copy
            var configValue = Configuration.SomePropertyToBeSavedAndWithADefault;
            if (ImGui.Checkbox("Random Config Bool", ref configValue))
            {
                Configuration.SomePropertyToBeSavedAndWithADefault = configValue;
                // can save immediately on change, if you don't want to provide a "Save and Close" button
                Configuration.Save();
            }
            ImGui.End();
        }
    }
}
