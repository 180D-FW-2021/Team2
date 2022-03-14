export async function getLeaderboard(params) {
  return [
    {
      username: "user1",
      level2: 23413525,
      level1: 2341323145,
      level3: 10,
      level4: 12,
    },
    {
      username: "user3",
      level0: 2,
      level2: 1,
      level1: 1,
      level3: 1,
      level4: 1,
    },
    {
      username: "user4",
      level2: 1,
      level1: 1,
      level3: 0,
      level0: 100,
      level4: 0,
    },
    {
      username: "user2",
      level2: 23413525,
      level1: 23413525,
      level3: 0,
      level4: 0,
    },
  ];
}

export async function getHistory(username) {
  return [
    { level: "MazeLevel_0", date: 318274813745827, score: 2341857384, id: 0 },
    { level: "MazeLevel_2", date: 213435436, score: 23413525, id: 1 },
    { level: "MazeLevel_1", date: 21, score: 2341323145, id: 2 },
  ];
}
