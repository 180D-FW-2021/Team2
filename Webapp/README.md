# AMaze Web Application

## Tech Stack

|                          Tech                           |   Description    |
| :-----------------------------------------------------: | :--------------: |
|             [ReactJS](https://reactjs.org/)             |     Frontend     |
|           [ExpressJS](https://expressjs.com/)           |     Backend      |
|           [MongoDB](https://www.mongodb.com/)           |     Database     |
| [MongoDB Atlas](https://www.mongodb.com/atlas/database) | Cloud DB Service |
|            [Heroku](https://www.heroku.com)             |  Cloud Platform  |

## Frontend

### Pages

|        URI         |       Description        |
| :----------------: | :----------------------: |
|         /          |       Leaderboard        |
| /history/:username | $username player history |

## Backend

### APIs

|                URI                | Method |              Description               |
| :-------------------------------: | :----: | :------------------------------------: |
|         /api/leaderboard          |  GET   |       Retrieve leaderboard data        |
| /api/history?username=${username} |  GET   | Retrieve $username player history data |
|            /api/insert            |  POST  |   Insert game records into database    |

1. `/api/leaderboard`

Return data format:

```
[
  { level: "level0", date: 1640671029064, score: 2341857384, id: 0 },
  { level: "level0", date: 1640671029065, score: 23413525, id: 1 },
  { level: "level1", date: 1640671029066, score: 2341323145, id: 2 },
]
```

Output is sorted from highest to lowest score.

2. `/api/history?username=${username}`

Return data format:

```
[
    {
      total_score: 4683180529,
      level1: 2341323145,
      level0: 2341857384,
      username: "user1",
    },
    {
      total_score: 46827050,
      level1: 23413525,
      level0: 23413525,
      username: "user2",
    },
    { total_score: 2341857384, level0: 2341857384, username: "user3" },
]
```

3. `/api/insert`

Input JSON data format:

```
{
    username: "user1",
    level: "level0",
    score: 100,
}
```

If the user does not exist, a new document for the new user will be created. Otherwise, the record will be added to the existing user's game history.

### Database

MongoDB database schema:

```
[
    {
        "username": "user1",
        "history": [
            {
                "level": "level0",
                "date": 213435436, // ms since epoch
                "score": 23413525,
            }
            {
                "level": "level1",
                "date": 213435436,
                "score": 23413525,
            }
        ]
    },
]
```

## Useful Commands

### MongoDB

Start/stop mongodb community locally (MacOS):

```
$ brew services start mongodb-community@5.0
$ brew services stop mongodb-community@5.0
```

## References

1. [Install Mongodb (MacOS)](https://docs.mongodb.com/manual/tutorial/install-mongodb-on-os-x/)
2. [Heroku free dynos](https://railsautoscale.com/heroku-free-dynos/)
3. [Deployment (Heroku/MongoDB Atlas)](https://www.youtube.com/watch?v=2AIL1c-cJM0)
