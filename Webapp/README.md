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
|            /api/signup            |  POST  |            create new user             |
|            /api/login             |  POST  |       Validate user credentials        |

1. `/api/leaderboard`

Return data format:

```
[
    { username: "user1", level0: 23413525, level1: 2341323145 },
    { username: "user2", level0: 23413525, level1: 23413525 },
]
```

2. `/api/history?username=${username}`

Return data format:

```
[
    {
      // accumulated best score by level
      level1: 2341323145,
      level2: 2341857384,
      username: "user1",
    },
    {
      level1: 23413525,
      level2: 23413525,
      username: "user2",
    },
    {
      username: "user3",
      level1: 2341857384
    },
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

4. `/api/signup`

Input JSON data format:

```
{
    username: "user1",
    password: "some_password"
}
```

Responses:

|     Response Code     |          Description          |
| :-------------------: | :---------------------------: |
|       200 (OK)        |            Success            |
| 400 (Invalid Request) | invalid param format/username |

For a 400 response, the error message will indicate whether the general parameter format was incorrect or if a username is already taken.

5. `api/login`

Input JSON data format:

```
{
    username: "user1",
    password: "some_password"
}
```

Responses:

|   Response Code    |        Description        |
| :----------------: | :-----------------------: |
|      200 (OK)      |          Success          |
| 401 (Unauthorized) | invalid user/passwd combo |

### Database

The database schemas for the `Users` and `Leaderboard` collections are shown below.

#### Users

```
[
    {
        "username": "user1",
        "password": "hashed_password1",
    },
    {
        "username": "user2",
        "password": "hashed_password2",
    }
]
```

#### Leaderboard

```
[
    {
        "username": "user1",
        "history": [
            {
                "level": "level0",
                "date": 213435436, // ms since epoch
                "score": 23413525, // maze completion time (s)
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
