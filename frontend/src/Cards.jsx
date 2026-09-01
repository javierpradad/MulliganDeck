import { useState, useEffect } from "react";

function Cards() {
  const [cards, setCards] = useState([]);
  const [search, setSearch] = useState("");

  useEffect(() => {
    const url = search
      ? `http://localhost:8080/api/cards?name=${search}`
      : `http://localhost:8080/api/cards`;
    fetch(url)
      .then((r) => r.json())
      .then((data) => setCards(data.items))
      .catch((e) => console.error(e));
  }, [search]);

  return (
    <div>
      <h2>Buscar cartas</h2>
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

export default Cards;