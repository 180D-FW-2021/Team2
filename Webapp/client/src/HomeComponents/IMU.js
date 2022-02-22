function IMU() {
  return (
    <div className="imu">
      <h3>IMU</h3>
      <p>
        The IMU controls <b>player movement</b>. Players can move forwards and
        backwards, as well as change their perspective by turning left and
        right.
      </p>
      <p>
        <table>
          <thead>
            <tr>
              <th>Neutral</th>
              <th>Forward</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td>
                <img src="/images/1.jpg" alt="imu_neutral" width="200" />
              </td>
              <td>
                <img src="/images/2.jpg" alt="imu_forward" width="200" />
              </td>
            </tr>
          </tbody>
          <thead>
            <th>Left</th>
            <th>Right</th>
          </thead>
          <tbody>
            <tr>
              <td>
                <img src="/images/3.jpg" alt="imu_left" width="200" />
              </td>
              <td>
                <img src="/images/4.jpg" alt="imu_right" width="200" />
              </td>
            </tr>
          </tbody>
        </table>
      </p>
      <p>
        We used the{" "}
        <a href="https://www.raspberrypi.com/news/zero-wh/">
          Raspberry Pi Zero
        </a>{" "}
        with the{" "}
        <a href="https://ozzmaker.com/product/berryimu-accelerometer-gyroscope-magnetometer-barometricaltitude-sensor/">
          BerryIMU
        </a>{" "}
        for our controller. For setup, refer to our{" "}
        <a href="https://github.com/180D-FW-2021/Team2">documentation</a>.
      </p>
    </div>
  );
}

export default IMU;
