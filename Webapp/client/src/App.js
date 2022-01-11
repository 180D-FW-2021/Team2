import { BrowserRouter, Routes, Route } from "react-router-dom";
import Leaderboard from "./Leaderboard";
import History from "./History";
import "./App.css";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/history/:username" element={<History />} />
        <Route path="/" element={<Leaderboard />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
