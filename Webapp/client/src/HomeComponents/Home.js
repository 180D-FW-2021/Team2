import Download from "./Download";

function Home() {
  return (
    <div className="home">
      <h1>A-Maze Game</h1>
      <p>
        <img src="/images/maze.png" alt="maze" width="500" />
      </p>
      <p>
        A-maze is an interactive indoor game designed to test a user's memory
        and dexerity. Players navigate through intricate maze environments,
        while jumping and ducking over obstacles.
      </p>
      <div id="download">
        <Download />
      </div>
    </div>
  );
}

export default Home;
