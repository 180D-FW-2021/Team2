export async function getLeaderboard(params) {
  return [
    {
      total_score: 4683180529,
      level0: 2341857384,
      level1: 2341323145,
      username: "user1",
    },
    { total_score: 2341857384, level0: 2341857384, username: "user3" },
    {
      total_score: 46827050,
      level1: 23413525,
      level0: 23413525,
      username: "user2",
    },
  ];
}

export async function getHistory(username) {
  return [
    { level: "level0", date: 213435436, score: 23413525 },
    { level: "level1", date: 213435436, score: 23413525 },
    { level: "level1", date: 1640049643531, score: 1234 },
    { level: "level1", date: 1640049661267, score: 1234 },
    { level: "level1", date: 1640049713164, score: 1234 },
    { level: "level1", date: 1640049739574, score: 1234 },
    { level: "level1", date: 1640049758826, score: 1234 },
  ];
}
