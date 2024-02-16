 // Initialize Firebase
    var firebaseConfig = {
      apiKey: "AIzaSyBbCOS02NPFgFKK8X6tIG-4gwFCZ2WgyZY",
      authDomain: "sneth-chat-51c06.firebaseapp.com",
      databaseURL: "https://sneth-chat-51c06-default-rtdb.firebaseio.com",
      projectId: "sneth-chat-51c06",
      storageBucket: "sneth-chat-51c06.appspot.com",
      messagingSenderId: "245152719854",
      appId: "1:245152719854:web:a5867e51fa8ff7609be4ff"
    };
    firebase.initializeApp(firebaseConfig);

    // Get a reference to the database service
    var database = firebase.database();
    
    // database location
    
    var localdatabase = "login/"

    // Generate a new push key for the user
    var pushKey = database.ref().push().key;

    // Create a QR code element with the push key
    var qrcode = new QRCode("qrcode", {
      text: pushKey,
      width: 200,
      height: 200,
      colorDark : "#000000",
      colorLight : "#ffffff",
      correctLevel : QRCode.CorrectLevel.H
    });

    // Get the device model, IP, and location
    var deviceModel = navigator.userAgent;
    var deviceIP = "";
    var deviceLocation = "";

    // Use a third-party API to get the IP and location
    fetch("https://ipinfo.io/json")
      .then(response => response.json())
      .then(data => {
        deviceIP = data.ip;
        deviceLocation = data.city + ", " + data.region + ", " + data.country;

        // Save the device data to the database with the push key
        database.ref(localdatabase + pushKey).set({
          deviceModel: deviceModel,
          deviceIP: deviceIP,
          deviceLocation: deviceLocation,
          deviceApproved: false
        });
      })
      .catch(error => console.error(error));

    // Check the deviceApproved status every 2 seconds
    var checkInterval = setInterval(function() {
      database.ref(localdatabase + pushKey).once("value")
        .then(snapshot => {
          var deviceApproved = snapshot.val().deviceApproved;

          // If the device is approved, show a success message and the device data
          if (deviceApproved) {
            var status = document.getElementById("status");
            status.innerHTML = "Login realizado com sucesso!";
            status.style.color = "green";

            var deviceData = JSON.stringify(snapshot.val(), null, 2);
            alert("Dados do dispositivo:\n" + deviceData);

            // Stop checking the deviceApproved status
            clearInterval(checkInterval);
            window.location.href = 'chats.html';
            document.cookie = "jsonDate=" + deviceData;
        document.cookie = "devicelog=" + deviceApproved;
          }
        })
        .catch(error => console.error(error));
    }, 2000);

    // Generate a new QR code every 16 seconds
    var generateInterval = setInterval(function() {
      // Remove the old QR code element
      qrcode.clear();

      // Delete the old push key from the database
     

      // Generate a new push key for the user
      pushKey = database.ref().push().key;

      // Create a new QR code element with the push key
      qrcode = new QRCode("qrcode", {
        text: pushKey,
        width: 200,
        height: 200,
        colorDark : "#000000",
        colorLight : "#ffffff",
        correctLevel : QRCode.CorrectLevel.H
      });

      // Save the device data to the database with the push key
      database.ref(localdatabase + pushKey).set({
        deviceModel: deviceModel,
        deviceIP: deviceIP,
        deviceLocation: deviceLocation,
        deviceApproved: false
      });
    }, 16000);
    function showSubmit() {
   window.location.href = 'index.html';
}
 database.ref(localdatabase + pushKey).remove();
setTimeout(showSubmit, 15000);
// Criar uma variável para armazenar o estado da página
var paginaFechada = false;

// Adicionar um evento de escuta para o evento beforeunload
window.addEventListener("beforeunload", function (e) {
  // Mudar o valor da variável para verdadeiro
  paginaFechada = true;
});

// Adicionar um evento de escuta para o evento unload
window.addEventListener("unload", function (e) {
  // Verificar se a variável é verdadeira
  if (paginaFechada) {
    // Mostrar uma mensagem no console
    console.log("A página foi fechada");
     database.ref(localdatabase + pushKey).remove();
  }
});
