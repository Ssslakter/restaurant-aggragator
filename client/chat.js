let connection;

$(document).ready(function () {
  $('#connectBtn').click(connect)
});

function connect() {
  connection = new signalR.HubConnectionBuilder()
    .withUrl($('#serviceUrl').val(), { accessTokenFactory: () => $('#jwt').val() })
    .configureLogging(signalR.LogLevel.Information)
    .build();

  connection.on("ReceiveMessage", (message) => {
    const li = document.createElement("li");
    li.textContent = `${message}`;
    document.getElementById("notificationList").appendChild(li);
  });

  connection.onclose(async () => {
    await start();
  });

  start();
}

async function start() {
  try {
    await connection.start();
    console.log("SignalR Connected.");
  } catch (err) {
    console.log(err);
    setTimeout(start, 5000);
  }
};

