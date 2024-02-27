// Inicialize o Firebase com suas chaves
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

function verificarCookie() {
    if (document.cookie.indexOf("deviceaproved=true") >= 0) {
      window.location.href = 'logout.html';
          console.log("essa sessão foi desconectada");
    } else {
        
    }
}

// Executa a função verificarCookie a cada 1 segundo
setInterval(verificarCookie, 1000);
