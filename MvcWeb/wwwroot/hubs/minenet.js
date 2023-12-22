// todo:  extract to libs file
$(document).ready(function () {
  var connection = new signalR.HubConnectionBuilder()
    .withUrl("/ws") // Replace with your actual hub path
    .configureLogging(signalR.LogLevel.Information)
    .build();

  connection.start().then(function () {
    console.log("Connected to hub");
  }).catch(function (err) {
    console.error(err.toString());
  });

  // Dummy data (replace with your actual data)
  var data = [
    { Column1: "Data 1", Column2: "Data 2", Column3: "Data 3" },
    { Column1: "Data 4", Column2: "Data 5", Column3: "Data 6" },
    // Add more data as needed
  ];

  // Function to update grid content
  function updateGrid() {
    var gridBody = $("#tags-grid-body");
    gridBody.empty(); // Clear existing content

    // Iterate through data and append rows
    //data.forEach(function (item) {
    //  var row = $("<div class='grid-row'>");
    //  row.append("<div class='grid-column'>" + item.Column1 + "</div>");
    //  row.append("<div class='grid-column'>" + item.Column2 + "</div>");
    //  row.append("<div class='grid-column'>" + item.Column3 + "</div>");
    //  gridBody.append(row);
    //});
    
    var row = $("<div class='grid-row'>");

    row.append("<div class='grid-column'>" + data.timeStamp + "</div>");
    row.append("<div class='grid-column'>" + data.message + "</div>");
    row.append("<div class='grid-column'>" + JSON.stringify(data.locations) + "</div>");
    row.append("</div>");

    gridBody.append(row);

    
    //gridBody.append("<div class='grid-row'><div class='grid-column'>" + data.timeStamp
    //  + "</div><div class='grid-column'>" + data.message +
    //"</div ><div /></div> ");

    // Make the grid sortable
    //$("#sortable-grid").sortable({
    //  items: ".grid-row",
    //  cursor: "grabbing",
    //  update: function (event, ui) {
    //    // Handle sorting updates
    //    var sortedIds = $("#sortable-grid .grid-row").map(function () {
    //      return $(this).index();
    //    }).get();

    //    // Send sortedIds to the hub or perform any required action
    //    connection.invoke("UpdateSortOrder", sortedIds);
    //  }
    //}).disableSelection();
  }

  // Call the updateGrid function initially and after any hub updates
  updateGrid();

  // Hub method to receive updates from the server
  connection.on("AlertNotification", function (updatedData) {
    data = updatedData;
    console.log(data);
    updateGrid();
  });

  // Hub method to receive updates from the server
  connection.on('LocationNotification', function (updatedData) {
    data = updatedData;
    console.log(data);
    updateGrid();
  });

  // Hub method to receive updates from the server
  connection.on("TagHistoryEtlUpdate", function (updatedData) {
    data = updatedData;
    console.log(data);
    updateGrid();
  });



  // Hub method to receive updates from the server
  connection.on('LocationUpdate', function (updatedData) {
    data = updatedData;
    console.log(data);
    updateGrid();
  });

});
