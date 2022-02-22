import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Container } from "react-bootstrap";
import NavBar from "./NavBar";
import Leaderboard from "./LeaderboardComponents/Leaderboard";
import History from "./HistoryComponents/History";
import Home from "./HomeComponents/Home";
import Levels from "./LevelsComponents/Levels";
import Instructions from "./InstructionsComponents/Instructions";
import "./App.css";

function App() {
  return (
    <div>
      <NavBar />
      <Container>
        <BrowserRouter>
          <Routes>
            <Route path="/history/:username" element={<History />} />
            <Route path="/leaderboard" element={<Leaderboard />} />
            <Route path="/levels" element={<Levels />} />
            <Route path="/instructions" element={<Instructions />} />
            <Route path="/" element={<Home />} />
          </Routes>
        </BrowserRouter>
      </Container>
    </div>
  );
}

export default App;
