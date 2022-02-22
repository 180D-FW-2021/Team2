import BootstrapTable from "react-bootstrap-table-next";

const columns = [
  {
    dataField: "control",
    text: "Control",
  },
  {
    dataField: "key",
    text: "Key(s)",
  },
];

const data = [
  {
    control: "Player movement",
    key: "Arrow keys or WASD.",
  },
  {
    control: "Rotate camera",
    key: "'z' (left), 'x' (right)",
  },
  {
    control: "Jump",
    key: "Space bar",
  },
  {
    control: "Duck",
    key: "Left shift",
  },
  {
    control: "Open pause menu",
    key: "ESC",
  },
];

function KeyboardControls() {
  return (
    <div>
      <h3>Keyboard Controls</h3>
      <p>
        <BootstrapTable keyField="control" columns={columns} data={data} />
      </p>
    </div>
  );
}

export default KeyboardControls;
