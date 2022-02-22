import { BrowserRouter, Routes, Route } from "react-router-dom";
import NavBar from "./NavBar";
import Leaderboard from "./Leaderboard";
import History from "./History";
import Home from "./Home";
import "./App.css";

function App() {
  return (
    <div>
      <NavBar />
      <BrowserRouter>
        <Routes>
          <Route path="/history/:username" element={<History />} />
          <Route path="/leaderboard" element={<Leaderboard />} />
          <Route path="/" element={<Home />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
