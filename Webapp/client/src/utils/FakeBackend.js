export async function getLeaderboard(params) {
  return [
    { username: "user1", level2: 23413525, level1: 2341323145 },
    { username: "user2", level2: 23413525, level1: 23413525 },
  ];
}

export async function getHistory(username) {
  return [
    { level: "level2", date: 318274813745827, score: 2341857384, id: 0 },
    { level: "level2", date: 213435436, score: 23413525, id: 1 },
    { level: "level1", date: 21, score: 2341323145, id: 2 },
  ];
}