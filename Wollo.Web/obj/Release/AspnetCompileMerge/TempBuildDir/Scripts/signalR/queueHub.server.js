//To trigger server method
// Declare a proxy to reference the hub. 
var queue = $.connection.queueHub;
// Start the connection.
$.connection.hub.start().done(function () {
});

var queueHub = {
    send: function (name, stockId, data) {
        return queue.server.send(name, stockId, data);
    }
};