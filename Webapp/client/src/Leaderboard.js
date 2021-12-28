import React, { useState, useEffect } from "react";
// import { getLeaderboard } from "./backend/FakeBackend";
import { getLeaderboard } from "./backend/RealBackend";
import BootstrapTable from "react-bootstrap-table-next";
import paginationFactory from "react-bootstrap-table2-paginator";
import "bootstrap/dist/css/bootstrap.css";
import "react-bootstrap-table-next/dist/react-bootstrap-table2.min.css";

const columns = [
  {
    dataField: "username",
    text: "Username",
    formatter: (cellContent) => {
      let hyperlink = `/history/${cellContent}`;
      return <a href={hyperlink}>{cellContent}</a>;
    },
  },
  { dataField: "total_score", text: "Total Score" },
  { dataField: "level0", text: "Level 0" },
  { dataField: "level1", text: "Level 1" },
];

function Leaderboard() {
  const [data, setData] = useState([]);
  useEffect(() => {
    getLeaderboard().then((data) => setData(data));
  }, []);
  return (
    <div>
      <h1>A-Maze Leaderboard</h1>
      <BootstrapTable
        keyField="username"
        data={data}
        columns={columns}
        pagination={paginationFactory()}
        hover
        condensed
      />
    </div>
  );
}

export default Leaderboard;
