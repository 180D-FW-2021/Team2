import BootstrapTable from "react-bootstrap-table-next";

const voice_columns = [
  {
    dataField: "command",
    text: "Command",
  },
  {
    dataField: "description",
    text: "Description",
  },
];

const voice_main_menu = [
  {
    command: "Back or Logout",
    description: "Logout and return to start screen",
  },
  {
    command: "Levels",
    description: "Open level selection screen",
  },
  {
    command: "Help",
    description: "Open help menu",
  },
  {
    command: "Quit",
    description: "Quit game",
  },
];

const voice_level_selector = [
  {
    command: "Back",
    description: "Return to main menu",
  },
  {
    command: "Zero",
    description: "Start tutorial (level 0)",
  },
  {
    command: "One",
    description: "Start level 1",
  },
  {
    command: "Two",
    description: "Start level 2",
  },
  {
    command: "Three",
    description: "Start level 3",
  },
  {
    command: "Four",
    description: "Start level 4",
  },
];

const voice_ready = [
  {
    command: "Yes",
    description: "Start game",
  },
  {
    command: "No",
    description: "Go back to level selection screen",
  },
];

const voice_pause_menu = [
  {
    command: "Stop or Pause",
    description: "Open pause menu",
  },
  {
    command: "Start or Resume",
    description: "Resume game",
  },
  {
    command: "Quit",
    description: "Exit game",
  },
];

const voice_end_screen = [
  {
    command: "Menu",
    description: "Go back to main menu",
  },
];

function VoiceCommands() {
  return (
    <div>
      <h3>Voice Commands</h3>
      <p>
        You can use voice commands to <b>navigate game menus</b>.
      </p>
      <h5>Main Menu</h5>
      <p>
        <BootstrapTable
          keyField="command"
          columns={voice_columns}
          data={voice_main_menu}
        />
      </p>
      <h5>Level Selection</h5>
      <p>
        <BootstrapTable
          keyField="command"
          columns={voice_columns}
          data={voice_level_selector}
        />
      </p>
      <h5>"Are You Ready" Screen</h5>
      <p>
        <BootstrapTable
          keyField="command"
          columns={voice_columns}
          data={voice_ready}
        />
      </p>
      <h5>Pause Menu</h5>
      <p>
        <BootstrapTable
          keyField="command"
          columns={voice_columns}
          data={voice_pause_menu}
        />
      </p>
      <h5>End Screen</h5>
      <p>
        <BootstrapTable
          keyField="command"
          columns={voice_columns}
          data={voice_end_screen}
        />
      </p>
    </div>
  );
}

export default VoiceCommands;
