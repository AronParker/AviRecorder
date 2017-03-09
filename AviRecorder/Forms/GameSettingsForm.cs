using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using AviRecorder.Controller;
using AviRecorder.Extensions;
using AviRecorder.Steam;

namespace AviRecorder.Forms
{
    public partial class GameSettingsForm : Form
    {
        private Configuration _config;
        private int _lastIndex = -1;

        public GameSettingsForm(Configuration config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            _config = config;

            InitializeComponent();

            _gameComboBox.DisplayMember = "Name";
            _gameComboBox.DataSource = config.Games;
            _gameComboBox.SelectedItem = config.Settings.CurrentGame;

            if (_gameComboBox.SelectedIndex == -1)
                _gameComboBox.SelectedIndex = 0;
            
            _userComboBox.DisplayMember = "Name";
            _userComboBox.DataSource = config.Users;
            _userComboBox.SelectedItem = config.Settings.CurrentUser;

            if (_userComboBox.SelectedIndex == -1)
                _userComboBox.SelectedIndex = 0;

            _toolTip.SetToolTip(_gameLabel, "The source game or source mod to use for recording.");
            _toolTip.SetToolTip(_userLabel, "The steam user that uses this tool. This is currently only used for importing the current launch options.");
            _toolTip.SetToolTip(_customLabel, "The customization files (.vpk files or folders in custom) to use for the game.\r\nWhen starting the game, it will move all unused items from custom to custom_store.\r\nLikewise it will attempt to move any currently missing custom items from custom_store to custom.");
            _toolTip.SetToolTip(_startmovieCommandsLabel, "The commands to run when starting a recording.");
            _toolTip.SetToolTip(_endmovieCommandsLabel, "The commands to run when a recording is stopped.");
        }

        public bool RecordingSettingsOnly
        {
            set
            {
                var enabled = !value;

                _gameComboBox.Enabled = enabled;
                _userComboBox.Enabled = enabled;
                _launchOptionsTextBox.Enabled = enabled;
                _customCheckedListBox.Enabled = enabled;
                _importLinkLabel.Visible = enabled;
            }
        }

        public bool GameSettingsChanged { private set; get; } = false;

        private SteamGameInfo CurrentGame
        {
            get => (SteamGameInfo)_gameComboBox.SelectedItem;
            set => _gameComboBox.SelectedItem = value;
        }

        private SteamUserInfo CurrentUser
        {
            get => (SteamUserInfo)_userComboBox.SelectedItem;
            set => _userComboBox.SelectedItem = value;
        }

        private void GameComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_lastIndex != -1)
            {
                var previousGame = (SteamGameInfo)_gameComboBox.Items[_lastIndex];
                var previousGameSettings = _config.Settings.GetGameSettings(previousGame);

                if (!GameSettingsEqual(previousGameSettings) && ConfirmGameSettingsChanges(previousGame))
                {
                    UpdateGameSettings(previousGame, previousGameSettings);
                    GameSettingsChanged = true;
                }
            }

            if (_gameComboBox.SelectedIndex != -1)
                LoadGameSettings(CurrentGame);

            _lastIndex = _gameComboBox.SelectedIndex;
        }

        private void RefreshCustom()
        {
            try
            {
                var custom = CurrentGame.GetCustom();

                _customCheckedListBox.Items.Clear();
                _customCheckedListBox.Items.AddRange((string[])custom.Active);
                _customCheckedListBox.Items.AddRange((string[])custom.Inactive);

                _customLabel.Enabled = _customCheckedListBox.Items.Count > 0;
            }
            catch (SteamException ex)
            {
                MessageBox.Show(ex.Message, "Failed to load customization files", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool GameSettingsEmpty()
        {
            return _launchOptionsTextBox.Text == SteamGameSettings.DefaultLaunchOptions &&
                   _customCheckedListBox.CheckedItems.Count == 0 &&
                   _startmovieCommandsTextBox.Text == SteamGameSettings.DefaultStartmovieCommands &&
                   _endmovieCommandsTextBox.Text == SteamGameSettings.DefaultEndmovieCommands;
        }

        private bool GameSettingsEqual(SteamGameSettings gameSettings)
        {
            var gameSettingsEmpty = gameSettings == null;
            var formEmpty = GameSettingsEmpty();

            if (gameSettingsEmpty)
                return formEmpty;

            if (formEmpty)
                return false;

            if (gameSettings.LaunchOptions != _launchOptionsTextBox.Text)
                return false;

            if (gameSettings.StartmovieCommands != _startmovieCommandsTextBox.Text)
                return false;

            if (gameSettings.EndmovieCommands != _endmovieCommandsTextBox.Text)
                return false;

            return new HashSet<string>(_customCheckedListBox.CheckedItems.Cast<string>()).SetEquals(gameSettings.Custom);
        }

        private void LoadGameSettings(SteamGameInfo game)
        {
            RefreshCustom();

            var gameSettings = _config.Settings.GetGameSettings(game);

            if (gameSettings == null)
            {
                _launchOptionsTextBox.Text = SteamGameSettings.DefaultLaunchOptions;
                _startmovieCommandsTextBox.Text = SteamGameSettings.DefaultStartmovieCommands;
                _endmovieCommandsTextBox.Text = SteamGameSettings.DefaultEndmovieCommands;
                return;
            }

            _launchOptionsTextBox.Text = gameSettings.LaunchOptions;

            foreach (var custom in gameSettings.Custom)
            {
                var index = _customCheckedListBox.Items.IndexOf(custom);

                if (index != -1)
                    _customCheckedListBox.SetItemChecked(index, true);
            }

            _startmovieCommandsTextBox.Text = gameSettings.StartmovieCommands;
            _endmovieCommandsTextBox.Text = gameSettings.EndmovieCommands;
        }

        private void UpdateGameSettings(SteamGameInfo game, SteamGameSettings gameSettings)
        {
            if (GameSettingsEmpty())
            {
                _config.Settings.SetGameSettings(game, null);
                return;
            }
            
            if (gameSettings == null)
            {
                gameSettings = new SteamGameSettings();
                _config.Settings.SetGameSettings(game, gameSettings);
            }

            gameSettings.LaunchOptions = _launchOptionsTextBox.Text;
            gameSettings.Custom.Replace(_customCheckedListBox.CheckedItems.Cast<string>());
            gameSettings.StartmovieCommands = _startmovieCommandsTextBox.Text;
            gameSettings.EndmovieCommands = _endmovieCommandsTextBox.Text;
        }

        private bool ConfirmGameSettingsChanges(SteamGameInfo game)
        {
            return MessageBox.Show($"Would you like to save the changes made to \"{game.Name}\"?",
                                   "Save changes",
                                   MessageBoxButtons.YesNo,
                                   MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void ImportLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            var game = CurrentGame;
            var user = CurrentUser;

            try
            {
                var launchOptions = game.GetLaunchOptions(user);

                if (launchOptions != null)
                    _launchOptionsTextBox.Text = launchOptions;
            }
            catch (SteamException ex)
            {
                MessageBox.Show(ex.Message, "Failed to import launch options", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                var custom = CurrentGame.GetActiveCustom();

                for (var i = 0; i < _customCheckedListBox.Items.Count; i++)
                    _customCheckedListBox.SetItemChecked(i, custom.Contains((string)_customCheckedListBox.Items[i]));
            }
            catch (SteamException ex)
            {
                MessageBox.Show(ex.Message, "Failed to import user customizations", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            var gameSettings = _config.Settings.GetCurrentGameSettings();

            if (!GameSettingsEqual(gameSettings))
            {
                UpdateGameSettings(CurrentGame, gameSettings);
                GameSettingsChanged = true;
            }
            else if (CurrentGame != _config.Settings.CurrentGame || CurrentUser != _config.Settings.CurrentUser)
            {
                GameSettingsChanged = true;
            }

            _config.Settings.CurrentGame = CurrentGame;
            _config.Settings.CurrentUser = CurrentUser;

            DialogResult = DialogResult.OK;
        }

        private void SelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var value = sender == _selectAllToolStripMenuItem;

            for (var i = 0; i < _customCheckedListBox.Items.Count; i++)
                _customCheckedListBox.SetItemChecked(i, value);
        }

        private void InvertSelectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < _customCheckedListBox.Items.Count; i++)
            {
                var value = _customCheckedListBox.GetItemChecked(i);

                _customCheckedListBox.SetItemChecked(i, !value);
            }
        }
    }
}
