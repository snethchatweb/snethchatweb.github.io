document.addEventListener('DOMContentLoaded', function() {
  document.getElementById('avatar').addEventListener('click', function() {
    window.location.href = 'my-settings.html';
  });
});
// Inicializa o Firebase
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

// Função para obter o valor de um cookie pelo seu nome
function getCookie(name) {
  var cookieName = name + "=";
  var allCookies = document.cookie;
  var cookiesArray = allCookies.split(';');
  for (var i = 0; i < cookiesArray.length; i++) {
    var cookie = cookiesArray[i].trim();
    if (cookie.indexOf(cookieName) == 0) {
      return cookie.substring(cookieName.length, cookie.length);
    }
  }
  return "";
}

// Verifica se o usuário está logado e obtém as informações
document.addEventListener('DOMContentLoaded', function() {
  var userId = getCookie("user_id");

  if (userId) {
    console.log("ID do usuário logado: " + userId);

    // Obter informações do usuário do Firebase Database
    var userRef = firebase.database().ref('users/' + userId);
    userRef.once('value').then(function(snapshot) {
      var userData = snapshot.val();
      console.log("Informações do usuário:", userData);
    }).catch(function(error) {
      console.error("Erro ao obter informações do usuário:", error);
    });
  } else {
    console.log("Nenhum usuário está logado.");
  }

  document.getElementById('avatar').addEventListener('click', function() {
    window.location.href = 'my-settings.html';
  });
});