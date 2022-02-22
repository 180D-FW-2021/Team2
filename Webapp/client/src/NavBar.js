import { Navbar, Nav, Container } from "react-bootstrap";

function NavBar() {
  return (
    <Navbar bg="light" variant="light">
      <Container>
        <Navbar.Brand href="/">
          <span className="amaze-txt">A-Maze</span>
        </Navbar.Brand>
        <Nav className="me-auto">
          <Nav.Link href="/#instructions">Instructions</Nav.Link>
          <Nav.Link href="/#levels">Levels</Nav.Link>
          <Nav.Link href="/#download">Download</Nav.Link>
          <Nav.Link href="/leaderboard">Leaderboard</Nav.Link>
        </Nav>
      </Container>
    </Navbar>
  );
}
export default NavBar;
