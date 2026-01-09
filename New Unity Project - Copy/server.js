

const db = require("./database");
const eventStore = require("./eventStore");
const express = require("express");
const app = express();

app.use(express.json());

app.get("/ping", (req, res) => {
    res.send("pong");
});

let events = eventStore.loadEvents();

app.post("/events", (req, res) => {
    const payload = req.body.events;

    // Safety check
    if (!Array.isArray(payload)) {
        return res.status(400).send("Expected an array of events");
    }

    // Optional: basic validation
    for (const evt of payload) {
        if (!evt.eventType|| !evt.eventData || !evt.timestamp || !evt.playerId || !evt.sessionId) {
            return res.status(400).send("Invalid event format");
        }
    }

    db.insertEvents(payload);
    console.log(payload);

    res.sendStatus(200);
});

app.get("/events", (req, res) => {
    db.getAllEvents((err, rows) => {
        if (err) {
            return res.status(500).send("DB error");
        }
        res.json(rows);
    });
});

app.listen(3000, () => {
    console.log("Server running on port 3000");
});