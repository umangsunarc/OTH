// Declare a proxy to reference the hub. 
debugger;
var queue = $.connection.queueHub;

$.connection.hub.start().done(function () {
});
// Create a function that the hub can call to broadcast messages.
queue.client.broadcastMessage = function (name, result) {
    debugger;
    var $ctrl = "";
    var $bidRateDiv = $("#bidRateDiv");
    var $askRateDiv = $("#askRateDiv");
    switch (name) {
        case 'bid': $ctrl = $('#tblBid tbody'); break;
        case 'ask': $ctrl = $('#tblAsk tbody'); break;
        case 'completed': $ctrl = $('#tblTraded tbody'); break;
        case 'GetQueueData': $ctrl = $("#askRateDiv"); break;
    }
    //var $ctrl = name == 'bid' ? $('#tblBid tbody') : $('#tblAsk tbody');
    var data = JSON.parse(result);
    if (data.length != 0 && name == "ask") {
        $("#Stock").val(data[0].stock_code_id);
    }
    $ctrl.html('');
    if (name == 'bid' || name == 'ask') {
        for (item in data) {
            var $tr = $('<tr>').append(
               $('<td>').text(data[item].Rate),
               $('<td>').text(data[item].Points),
              $('<td>').text(data[item].Total)
              //$('<td>').text(Math.floor((Math.random() * 10) + 1))
           );
            $ctrl.append($tr);
        }
    }
    else if (name == "GetQueueData") {
        $bidRateDiv.html('');
        $askRateDiv.html('');
        $bidRateDiv.html(data.highestBidRate);
        $askRateDiv.html(data.LowestAskRate);
    }
    else {
        for (item in data) {
            var $tr = $('<tr>').append(
               $('<td>').text(data[item].created_date),
               $('<td>').text(data[item].queue_action),
               $('<td>').text(data[item].amount),
              $('<td>').text(data[item].bid_price),
              $('<td>').text(data[item].ask_price)
           );
            $ctrl.append($tr);
        }
    }
};

