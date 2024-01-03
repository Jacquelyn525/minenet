$(document).ready(function () {
  var filterId = 0;

  // #region Hub


  var connection = new signalR.HubConnectionBuilder()
    .withUrl("/ws")
    .configureLogging(signalR.LogLevel.Information)
    .build();

  connection
    .start()
    .then(function () {
      console.log("Connected to minenet hub");
    })
    .catch(function (err) {
      console.error(err.toString());
    });

  connection.on("LocationNotification", function (updatedData) {
    if (updatedData && updatedData.locations) {
      updateLocationsGrid(updatedData);
    }
  });

  connection.on("AlertNotification", function (alertsData) {
    updateAlertsGrid(alertsData);
  });

  connection.on("LocationUpdate", function (updatedData) {
    if (updatedData && updatedData.locations) {
      updateLocationsGrid(updatedData);
    }
  });


  function dataCol(colValue) {
    var col = $("<td>");

    col.append(colValue);
    col.append("</td>");

    return col;
  }


  //#endregion

  //#region Grids

  function updateLocationsGrid(tagsData) {
    var gridBody = $("#tags-grid-body");

    gridBody.empty();

    filterGridByTagId(tagsData.locations, filterId).forEach((entry) => {
      var row = $("<tr>");

      row.append("<th scope='row' />");

      row.append(dataCol(entry.tagID));
      row.append(dataCol(entry.minerID));
      row.append(dataCol(entry.lastName));
      row.append(dataCol(entry.firstName));
      row.append(dataCol(entry.address));
      row.append(dataCol(entry.zone));
      row.append(dataCol(entry.reported));
      row.append(dataCol(entry.signalStrength));

      row.append("</tr>");

      gridBody.append(row);
    });
  }

  function updateAlertsGrid(alertsData) {
    var gridBody = $("#alerts-grid-body");

    gridBody.empty();

    alertsData.alerts.forEach((entry) => {
      var row = $("<tr>");

      row.append("<th scope='row' />");
      row.append(dataCol(entry.address));
      row.append(dataCol(entry.device));
      row.append(dataCol(entry.type));
      row.append(dataCol(entry.alarm));
      row.append(dataCol(entry.occured));
      row.append(dataCol(entry.location));
      row.append("</tr>");

      gridBody.append(row);
    });
  }

  //#endregion

  $('#myTabs a').on('click', function (e) {
    e.preventDefault();
    filterId = $(this).data('tab-id');
    setGridTitle(filterId);
    $(this).tab('show');
  });

  // Function to update numeric value based on tabId
  function updateNumericValue(tabId) {
    // You can perform logic here based on the tabId
    var numericValue = tabId * 100; // For example, multiply tabId by 100
    $('#numericValue' + tabId).text(numericValue);
  }
});
