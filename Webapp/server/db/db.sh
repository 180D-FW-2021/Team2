use AMaze;
db.Leaderboard.drop();
db.createCollection("Leaderboard");
db.Leaderboard.insertMany([
    {
        "username": "user1",
        "history": [
            {
                "level": "MazeLevel_2",
                "date": 213435436,
                "score": 23413525,
            },
            {
                "level": "MazeLevel_1",
                "date": 21,
                "score": 2341323145,
            }
        ]
    },
    {
        "username": "user2",
        "history": [
            {
                "level": "MazeLevel_2",
                "date": 213435436,
                "score": 23413525,
            },
            {
                "level": "MazeLevel_0",
                "date": 213435436,
                "score": 23413525,
            }
        ]
    },
]);
db.Users.drop();
db.createCollection("Users");
db.Users.insertMany([
    {
        "username": "user1",
        "password": "hashed_password",
    },
    {
        "username": "user2",
        "password": "hashed_password",
    }
]);