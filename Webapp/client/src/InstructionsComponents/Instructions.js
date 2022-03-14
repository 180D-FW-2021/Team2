import KeyboardControls from "./KeyboardControls";
import IMU from "./IMU";
import WebCam from "./WebCam";
import VoiceCommands from "./VoiceCommands";
import "./Instructions.css";

function Instructions() {
  return (
    <div className="instructions">
      <h1>Instructions</h1>
      <p>
        The game uses an IMU controls for player movement, voice commands for
        menu navigation, and the web cam for jump/duck detection. For
        convenience, all controls can also be performed using the keyboard.
      </p>
      <KeyboardControls />
      <IMU />
      <WebCam />
      <VoiceCommands />
    </div>
  );
}

export default Instructions;
