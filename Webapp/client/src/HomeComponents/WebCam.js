function WebCam() {
  return (
    <div>
      <h3>Web Cam</h3>
      <p>
        The web cam detects <b>jumping/ducking</b>. Detected movements will be
        reflected in the game, allowing you to navigate obstacles.
      </p>
      <p>
        Ensure your arms are in frame, even when jumping or ducking. Best
        performance is when you are 1-3 ft from your webcam. Also, make sure you
        are not swapping too quickly between movements. After a failed attempt,
        wait around a 1-2 seconds before re-attempting the movement.
      </p>
      <p>
        <img
          src="/images/movenet_position.png"
          alt="pose-detection"
          width="300"
        />
      </p>
      <p>
        <table className="instructions-table">
          <thead>
            <tr>
              <th>Jump</th>
              <th>Duck</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>
                Light jumps are acceptable when 1-3 ft from the camera. Ensure
                to not duck too much before jumping, as this will be recognized
                as a duck instead of jump.
              </td>
              <td>
                Small ducks are also acceptable, given you are 1-3 ft from the
                camera. Quick ducks are more recognizable than slow ducks.
              </td>
            </tr>
          </tbody>
        </table>
      </p>
    </div>
  );
}

export default WebCam;
