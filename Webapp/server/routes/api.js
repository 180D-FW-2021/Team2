let express = require("express");
let router = express.Router();
let client = require("../db/db");
let bcrypt = require("bcryptjs");

router.get("/leaderboard", async (req, res) => {
  let coll = client.db("AMaze").collection("Leaderboard");
  const pipeline = [
    {
      $unwind: {
        path: "$history",
        preserveNullAndEmptyArrays: true,
      },
    },
    // get max score by (user, level)
    {
      $group: {
        _id: {
          username: "$username",
          level: "$history.level",
        },
        score: {
          $min: "$history.score",
        },
      },
    },
    {
      $group: {
        _id: "$_id.username",
        array: {
          $push: {
            k: "$_id.level",
            v: "$score",
          },
        },
      },
    },
    {
      $project: {
        _id: 0,
        username: "$_id",
        level_scores: { $arrayToObject: "$array" },
      },
    },
    {
      $project: {
        username: 1,
        level1: "$level_scores.MazeLevel_0",
        level2: "$level_scores.MazeLevel_1",
        level3: "$level_scores.MazeLevel_2",
        level4: "$level_scores.MazeLevel_3",
      },
    },
  ];
  const agg_cursor = coll.aggregate(pipeline);
  // accumulate total score
  let leaderboard_data = [];
  for await (const doc of agg_cursor) {
    leaderboard_data.push(doc);
  }
  res.json(leaderboard_data);
});

router.get("/history", async (req, res) => {
  let username = req.query.username;
  if (username == null) {
    res.status(400);
    res.send("Error: username not specified");
    return res.end();
  }
  let coll = client.db("AMaze").collection("Leaderboard");
  const user_data = await coll.findOne({ username: username });
  if (user_data == null || !user_data.hasOwnProperty("history")) {
    res.status(404);
    res.send("Error: user not found");
    return res.end();
  }
  let user_history = user_data.history;
  // sort by inverse chronological order (latest games first)
  user_history.sort((a, b) => {
    // neg: a < b; pos o.w.
    if (a.date > b.date) {
      return -1;
    } else {
      return 1;
    }
  });
  // add id field
  user_history.map((elem, index) => {
    elem.id = index;
  });
  res.status(200);
  res.json(user_history);
});

router.post("/insert", async (req, res) => {
  let coll = client.db("AMaze").collection("Leaderboard");
  let username = req.body.username;
  let level = req.body.level;
  let score = req.body.score;
  if (!username || !level || !score) {
    res.status(400);
    res.send("username, level, score fields are required");
    return res.end();
  }
  let history_elem = {
    level: level,
    date: Date.now(),
    score: score,
  };
  let rc = await coll.updateOne(
    { username: username },
    {
      $push: {
        history: history_elem,
      },
    },
    {
      upsert: true,
    }
  );
  if (rc.acknowledged) {
    res.status(200);
    return res.send("Successful update!");
  }
  res.status(500);
  console.dir(rc);
  return res.send("Update failed");
});

router.post("/signup", async (req, res) => {
  let username = req.body.username;
  let passwd = req.body.password;
  // check required parameters
  // TODO: add conditions to username (length, characters, etc.)
  if (!username || !passwd) {
    res.status(400);
    res.send("Username and password are required fields");
    return res.end();
  }
  let coll = client.db("AMaze").collection("Users");
  // check if user already exists
  const existing_user = await coll.findOne({ username: username });
  if (existing_user != null) {
    res.status(400);
    res.send(`Username ${username} already exists`);
    return res.end();
  }
  // hash password before inserting
  let hash = await bcrypt.hash(passwd, 10);
  const result = await coll.insertOne({
    username: username,
    password: hash,
  });
  console.log(`username created with _id: ${result.insertedID}`);
  res.status(200);
  res.send(`username ${username} successfully created!`);
  return res.end();
});

router.post("/login", async (req, res) => {
  let coll = client.db("AMaze").collection("Users");
  let username = req.body.username;
  let expected_passwd = req.body.password;
  let userdata = await coll.findOne({ username: username });
  if (userdata == null || userdata.password == null) {
    res.status(401);
    res.send("Invalid credentials");
    return res.end();
  }
  let actual_passwd = userdata.password;
  let valid = await bcrypt.compare(expected_passwd, actual_passwd);
  if (valid) {
    res.status(200);
    res.send("Successful login!");
    return res.end();
  } else {
    res.status(401);
    res.send("Invalid credentials");
    return res.end();
  }
});

module.exports = router;
