import IMU from "./HomeComponents/IMU";
import WebCam from "./HomeComponents/WebCam";
import VoiceCommands from "./HomeComponents/VoiceCommands";
import KeyboardControls from "./HomeComponents/KeyboardControls";
import Levels from "./HomeComponents/Levels";
import Download from "./HomeComponents/Download";

function Home() {
  return (
    <div>
      <h1>A-Maze Game</h1>
      <p>
        A-maze is an interactive indoor game designed to test a user's memory
        and dexerity. Players navigate through intricate maze environments,
        while jumping and ducking over obstacles.
      </p>
      <div id="instructions">
        <h2>Instructions</h2>
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
      <div id="levels">
        <Levels />
      </div>
      <div id="download">
        <Download />
      </div>
    </div>
  );
}

export default Home;
