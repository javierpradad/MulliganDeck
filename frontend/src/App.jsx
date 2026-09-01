import { useState, useEffect } from "react";
import Login from "./Login";

function App() {
  const [token, setToken] = useState(localStorage.getItem("token"));
  const [cards, setCards] = useState([]);
  const [search, setSearch] = useState("");

  useEffect(() => {
    const url = search
      ? `http://localhost:8080/api/cards?name=${search}`
      : `http://localhost:8080/api/cards`;
    fetch(url)
      .then((response) => response.json())
      .then((data) => setCards(data.items))
      .catch((error) => console.error("Error:", error));
  }, [search]);

  const handleLogin = (newToken) => {
    localStorage.setItem("token", newToken);
    setToken(newToken);
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    setToken(null);
  };

  if (!token) {
    return <Login onLogin={handleLogin} />;
  }

  return (
    <div>
      <h1>MulliganDeck</h1>
      <p>Sesión iniciada ✓</p>
      <button onClick={handleLogout}>Cerrar sesión</button>
      <br />
      <input
        type="text"
        placeholder="Buscar cartas..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />
      <ul>
        {cards.map((card) => (
          <li key={card.oracleId}>
            {card.name} — {card.manaCost} ({card.typeLine})
          </li>
        ))}
      </ul>
    </div>
  );
}

export default App;