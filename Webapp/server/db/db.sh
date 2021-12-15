use AMaze;
db.Leaderboard.drop();
db.createCollection("Leaderboard");
db.Leaderboard.insertMany([
    {
        "username": "user1",
        "history": [
            {
                "level": "level0",
                "date": 213435436,
                "score": 23413525,
            },
            {
                "level": "level1",
                "date": 21,
                "score": 2341323145,
            }
        ]
    },
    {
        "username": "user2",
        "history": [
            {
                "level": "level0",
                "date": 213435436,
                "score": 23413525,
            },
            {
                "level": "level1",
                "date": 213435436,
                "score": 23413525,
            }
        ]
    },
]);