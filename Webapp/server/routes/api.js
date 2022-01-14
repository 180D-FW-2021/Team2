let express = require("express");
let router = express.Router();
let client = require("../db/db");

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
  let history_elem = {
    level: req.body.level,
    date: Date.now(),
    score: req.body.score,
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

module.exports = router;
