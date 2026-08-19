import { useState, useEffect } from "react";

function App() {
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

  return (
    <div>
      <h1>MulliganDeck</h1>
      <p>Gestor de mazos de Magic: The Gathering</p>

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