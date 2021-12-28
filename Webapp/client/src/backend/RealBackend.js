export async function getLeaderboard() {
  let res = await fetch("/api/leaderboard");
  if (res.status == 200) {
    let data = await res.json();
    return data;
  } else {
    return Promise.reject(new Error(String(res.status)));
  }
}

export async function getHistory(username) {
  let url = `/api/history?username=${username}`;
  let res = await fetch(url);
  if (res.status == 200) {
    let data = await res.json();
    return data;
  } else {
    return Promise.reject(new Error(String(res.status)));
  }
}
