import React, { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import { secToDate, getLevelText } from "./utils/Utils";
// import { getHistory } from "./utils/FakeBackend";
import { getHistory } from "./utils/RealBackend";
import BootstrapTable from "react-bootstrap-table-next";
import paginationFactory from "react-bootstrap-table2-paginator";
import "bootstrap/dist/css/bootstrap.css";
import "react-bootstrap-table-next/dist/react-bootstrap-table2.min.css";

const elemsPerPage = 20;

const columns = [
  { datafield: "id", hidden: true },
  {
    dataField: "date",
    text: "Date",
    formatter: (cellContent) => {
      let dateObj = new Date(cellContent);
      return dateObj.toLocaleDateString();
    },
  },
  {
    dataField: "level",
    text: "Level",
    formatter: (cellContent) => {
      return getLevelText(cellContent);
    },
  },
  {
    dataField: "score",
    text: "Score",
    formatter: (cellContent) => {
      if (cellContent == null) {
        return "-";
      }
      return secToDate(parseInt(cellContent));
    },
  },
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
        pagination={paginationFactory({
          sizePerPage: elemsPerPage,
          alwaysShowAllBtns: false,
          hideSizePerPage: true,
        })}
        hover
        condensed
      />
    </div>
  );
}

export default History;
