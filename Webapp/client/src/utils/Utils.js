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
