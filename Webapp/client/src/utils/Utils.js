// ref: https://stackoverflow.com/questions/1322732/convert-seconds-to-hh-mm-ss-with-javascript
// convert time completed (s) to hh-mm-ss
export function secToDate(seconds) {
  // remove decimals
  // let seconds_rounded = seconds.toFixed();
  if (seconds < 3600) {
    return new Date(seconds * 1000).toISOString().substr(14, 5);
  } else {
    return new Date(seconds * 1000).toISOString().substr(11, 8);
  }
}

export function getLevelText(text) {
  let level = text;
  switch (text) {
    case "level1":
    case "MazeLevel_0":
      level = "Level 1";
      break;
    case "level2":
    case "MazeLevel_1":
      level = "Level 2";
      break;
    case "level3":
    case "MazeLevel_2":
      level = "Level 3";
      break;
    case "level4":
    case "MazeLevel_3":
      level = "Level 4";
      break;
    default:
      break;
  }
  return level;
}
