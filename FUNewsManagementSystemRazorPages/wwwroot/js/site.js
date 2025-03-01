"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/signalRServer").build();

connection.on("LoadAllItems", function () {
    location.href = '/Products/Index';
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});
