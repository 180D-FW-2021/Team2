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
          $max: "$history.score",
        },
      },
    },
    {
      $project: {
        _id: "$_id.username",
        level: "$_id.level",
        score: 1,
      },
    },
  ];
  const agg_cursor = coll.aggregate(pipeline);
  // accumulate total score
  let leaderboard_data = {};
  for await (const doc of agg_cursor) {
    let username = doc._id;
    if (!leaderboard_data.hasOwnProperty(username)) {
      leaderboard_data[username] = {};
    }
    if (!leaderboard_data[username].hasOwnProperty("total_score")) {
      leaderboard_data[username].total_score = 0;
    }
    if (doc.hasOwnProperty("level") && doc.hasOwnProperty("score")) {
      // update total score
      // for now just accumulate total score
      let new_score = leaderboard_data[username].total_score;
      new_score += doc.score;
      leaderboard_data[username].total_score = new_score;
      // update highest level score
      leaderboard_data[username][doc.level] = doc.score;
    }
  }
  // rearrange into return data format
  let return_data = [];
  for (let key in leaderboard_data) {
    let val = leaderboard_data[key];
    val.username = key;
    return_data.push(val);
  }
  return_data.sort((a, b) => {
    return b.total_score > a.total_score;
  });
  res.json(return_data);
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
    return b.date < a.date;
  });
  res.status(200);
  res.json(user_data.history);
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
