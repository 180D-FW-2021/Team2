import React, { useState, useEffect } from "react";
import { secToDate } from "../utils/Utils";
// import { getLeaderboard } from "../utils/FakeBackend";
import { getLeaderboard } from "../utils/RealBackend";
import ToolkitProvider, {
  Search,
} from "react-bootstrap-table2-toolkit/dist/react-bootstrap-table2-toolkit.min";
import BootstrapTable from "react-bootstrap-table-next";
import paginationFactory from "react-bootstrap-table2-paginator";
import "bootstrap/dist/css/bootstrap.css";
import "react-bootstrap-table-next/dist/react-bootstrap-table2.min.css";
import "react-bootstrap-table2-toolkit/dist/react-bootstrap-table2-toolkit.min.css";
import "./Leaderboard.css";

const { SearchBar } = Search;
const elemsPerPage = 10;

function formatLevelCol(cell, row) {
  // return "-" for unplayed levels
  if (!cell) {
    return "-";
  }
  return secToDate(parseInt(cell));
}

function sortScores(a, b, order) {
  // always sort unplayed levels last
  if (a === 0) {
    return 1;
  }
  if (b === 0) {
    return -1;
  }
  // neg -> a before b
  // pos -> b before a
  if (order === "asc") {
    return b - a;
  } else {
    return a - b;
  }
}

const columns = [
  {
    dataField: "username",
    text: "Username",
    formatter: (cellContent) => {
      let hyperlink = `/history/${cellContent}`;
      return <a href={hyperlink}>{cellContent}</a>;
    },
  },
  {
    dataField: "level0",
    text: "Level 0",
    formatter: formatLevelCol,
    sort: true,
    sortFunc: sortScores,
    searchable: false,
  },
  {
    dataField: "level1",
    text: "Level 1",
    formatter: formatLevelCol,
    sort: true,
    sortFunc: sortScores,
    searchable: false,
  },
  {
    dataField: "level2",
    text: "Level 2",
    formatter: formatLevelCol,
    sort: true,
    sortFunc: sortScores,
    searchable: false,
  },
  {
    dataField: "level3",
    text: "Level 3",
    formatter: formatLevelCol,
    sort: true,
    sortFunc: sortScores,
    searchable: false,
  },
  {
    dataField: "level4",
    text: "Level 4",
    formatter: formatLevelCol,
    sort: true,
    sortFunc: sortScores,
    searchable: false,
  },
];

function Leaderboard() {
  const [data, setData] = useState([]);
  useEffect(() => {
    getLeaderboard().then((data) => setData(data));
  }, []);
  return (
    <div>
      <h1>Leaderboard</h1>
      <ToolkitProvider keyField="username" data={data} columns={columns} search>
        {(props) => {
          return (
            <p>
              <SearchBar
                srText=""
                placeholder="Search username"
                {...props.searchProps}
              />
              <hr />
              <BootstrapTable
                {...props.baseProps}
                pagination={paginationFactory({
                  sizePerPage: elemsPerPage,
                  alwaysShowAllBtns: false,
                  hideSizePerPage: true,
                })}
                hover
                condensed
              />
            </p>
          );
        }}
      </ToolkitProvider>
    </div>
  );
}

export default Leaderboard;
