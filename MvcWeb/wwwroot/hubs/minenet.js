// todo:  extract to libs file
$(document).ready(function () {
  var connection = new signalR.HubConnectionBuilder()
    .withUrl("/ws") // Replace with your actual hub path
    .configureLogging(signalR.LogLevel.Information)
    .build();

  connection
    .start()
    .then(function () {
      console.log("Connected to hub");
    })
    .catch(function (err) {
      console.error(err.toString());
    });

  function dataCol(colValue) {
    var col = $("<div class='grid-column'>");
    col.append(colValue);
    col.append("</div>");

    return col;
  }

  // Function to update grid content
  function updateGrid(tagsData) {
    var gridBody = $("#tags-grid-body");
    gridBody.empty(); // Clear existing content

    // Iterate through data and append rows
    tagsData.locations.forEach((entry) => {
      console.log(entry);
      var row = $("<div class='grid-row'>");

      row.append(dataCol(entry.tagID));
      row.append(dataCol(entry.minerID));
      row.append(dataCol(entry.lastName));
      row.append(dataCol(entry.firstName));
      row.append(dataCol(entry.address));
      row.append(dataCol(entry.zoneNumber));
      row.append(dataCol(entry.zone));
      row.append(dataCol(entry.reported));
      row.append(dataCol(entry.signalStrength));
      row.append("</div>");

      gridBody.append(row);
    });
    
    // Make the grid sortable
    $("#sortable-grid").sortable({
      items: ".grid-row",
      cursor: "grabbing",
      update: function (event, ui) {
        // Handle sorting updates
        var sortedIds = $("#sortable-grid .grid-row").map(function () {
          return $(this).index();
        }).get();

        // Send sortedIds to the hub or perform any required action
        connection.invoke("UpdateSortOrder", sortedIds);
      }
    }).disableSelection();
  }

  // Call the updateGrid function initially and after any hub updates
  //updateGrid();

  // Hub method to receive updates from the server
  connection.on("LocationNotification", function (updatedData) {
    //console.log(updatedData);
    if (updatedData && updatedData.locations) {
      updateGrid(updatedData);
    }
  });


  // Hub method to receive updates from the server
  connection.on("AlertNotification", function (updatedData) {
    //data = updatedData;
    //console.log(data);
    //updateGrid(data);
  });

  // Hub method to receive updates from the server
  //connection.on("TagHistoryEtlUpdate", function (updatedData) {
  //  data = updatedData;
  //  console.log(data);
  //  updateGrid();
  //});

  // Hub method to receive updates from the server
  connection.on("LocationUpdate", function (updatedData) {
    if (updatedData && updatedData.locations) {
      updateGrid(updatedData);
    }
  });
});
