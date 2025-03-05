"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/SignalrServer")
    .build();

connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on("ReceiveNewsUpdate", function () {
    location.reload(); // Reload the page to fetch new articles
});
