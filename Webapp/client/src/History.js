import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { getHistory } from "./backend/FakeBackend";
// import { getLeaderboard } from "./backend/RealBackend";
import BootstrapTable from "react-bootstrap-table-next";
import paginationFactory from "react-bootstrap-table2-paginator";
import "bootstrap/dist/css/bootstrap.css";
import "react-bootstrap-table-next/dist/react-bootstrap-table2.min.css";

const columns = [
  { datafield: "id", hidden: true },
  { dataField: "date", text: "Date" },
  { dataField: "level", text: "Level" },
  { dataField: "score", text: "Score" },
];

function History() {
  let { username } = useParams();
  const [data, setData] = useState([]);
  useEffect(() => {
    getHistory(username).then((data) => setData(data));
  }, [username]);
  return (
    <div>
      <h1>
        <b>{username}</b>'s Player History
      </h1>
      <BootstrapTable
        keyField="id"
        data={data}
        columns={columns}
        pagination={paginationFactory()}
        hover
        condensed
      />
    </div>
  );
}

export default History;
