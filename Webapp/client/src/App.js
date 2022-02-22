import { BrowserRouter, Routes, Route } from "react-router-dom";
import { Container } from "react-bootstrap";
import NavBar from "./NavBar";
import Leaderboard from "./Leaderboard";
import History from "./History";
import Home from "./Home";
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
            <Route path="/" element={<Home />} />
          </Routes>
        </BrowserRouter>
      </Container>
    </div>
  );
}

export default App;
