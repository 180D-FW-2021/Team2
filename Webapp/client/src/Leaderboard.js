import React, { useState, useEffect } from "react";
import { secToDate } from "./utils/Utils";
// import { getLeaderboard } from "./utils/FakeBackend";
import { getLeaderboard } from "./utils/RealBackend";
import ToolkitProvider, {
  Search,
} from "react-bootstrap-table2-toolkit/dist/react-bootstrap-table2-toolkit.min";
import BootstrapTable from "react-bootstrap-table-next";
import paginationFactory from "react-bootstrap-table2-paginator";
// import "bootstrap/dist/css/bootstrap.css";
import "react-bootstrap-table-next/dist/react-bootstrap-table2.min.css";
import "react-bootstrap-table2-toolkit/dist/react-bootstrap-table2-toolkit.min.css";

const { SearchBar } = Search;
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
    dataField: "level1",
    text: "Level 1",
    formatter: (cellContent) => {
      if (cellContent == null) {
        return "-";
      }
      return secToDate(parseInt(cellContent));
    },
    sort: true,
    searchable: false,
  },
  {
    dataField: "level2",
    text: "Level 2",
    formatter: (cellContent) => {
      if (cellContent == null) {
        return "-";
      }
      return secToDate(parseInt(cellContent));
    },
    sort: true,
    searchable: false,
  },
  {
    dataField: "level3",
    text: "Level 3",
    formatter: (cellContent) => {
      if (cellContent == null) {
        return "-";
      }
      return secToDate(parseInt(cellContent));
    },
    sort: true,
    searchable: false,
  },
  {
    dataField: "level4",
    text: "Level 4",
    formatter: (cellContent) => {
      if (cellContent == null) {
        return "-";
      }
      return secToDate(parseInt(cellContent));
    },
    sort: true,
    searchable: false,
  },
];

/*
const defaultSorted = [
  {
    dataField: "level1",
    order: "asc",
  },
];
*/

function Leaderboard() {
  const [data, setData] = useState([]);
  useEffect(() => {
    getLeaderboard().then((data) => setData(data));
  }, []);
  return (
    // bug: pagination/styling is not showing
    <div>
      <h1>A-Maze Leaderboard</h1>
      <ToolkitProvider
        keyField="username"
        data={data}
        columns={columns}
        pagination={paginationFactory()}
        defaultSortDirection="asc"
        hover
        condensed
        search
      >
        {(props) => (
          <div>
            <SearchBar {...props.searchProps} />
            <hr />
            <BootstrapTable {...props.baseProps} />
          </div>
        )}
      </ToolkitProvider>
    </div>
  );
}

export default Leaderboard;
