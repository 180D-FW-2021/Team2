import { useParams } from "react-router-dom";

function History() {
  let { username } = useParams();
  return <div>History {username}</div>;
}

export default History;
