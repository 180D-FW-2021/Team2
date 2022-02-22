function Download() {
  return (
    <div>
      <h2>Download</h2>
      <p>
        We have downloadable executables available for the following platforms:
      </p>
      <p>
        <ul>
          <li>
            <a
              href="https://drive.google.com/drive/folders/1uT8gXs9GdFS7LiCJ-cUtam71ef4i_pRa?usp=sharing"
              rel="noopener noreferrer"
              target="_blank"
            >
              MacOS 64-bit
            </a>{" "}
            (Intel/Apple Silicon)
          </li>
          <li>
            <a
              href="https://drive.google.com/drive/folders/1N7XR1c9_QFn2WRFulpuJr3XWpB3DDAPM?usp=sharing"
              rel="noopener noreferrer"
              target="_blank"
            >
              Windows 10 64-bit
            </a>
          </li>
        </ul>
      </p>
      <p>
        After downloading, open the game executable under{" "}
        <code>Unity/MazeProject.exe</code>. This will automatically boot the web
        cam jump/duck detection once you login.
      </p>
      <p>
        If your platform is different, you can still play our game by compiling
        our{" "}
        <a href="https://github.com/180D-FW-2021/Team2#Compilation">
          source code
        </a>
        . Instructions can be found in our documentation under{" "}
        <a href="https://github.com/180D-FW-2021/Team2#Compilation">
          Compilation
        </a>
        .
      </p>
    </div>
  );
}

export default Download;
