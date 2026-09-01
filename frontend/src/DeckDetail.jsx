import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";

function DeckDetail() {
  const { id } = useParams();
  const [deck, setDeck] = useState(null);
  const [search, setSearch] = useState("");
  const [results, setResults] = useState([]);

  const token = localStorage.getItem("token");

  // Cargar el mazo
  const loadDeck = () => {
    fetch(`http://localhost:8080/api/decks/${id}`, {
      headers: { Authorization: `Bearer ${token}` },
    })
      .then((r) => r.json())
      .then((data) => setDeck(data))
      .catch((e) => console.error(e));
  };

  useEffect(() => {
    loadDeck();
  }, [id]);

  // Buscar cartas para añadir
  useEffect(() => {
    if (!search) {
      setResults([]);
      return;
    }
    fetch(`http://localhost:8080/api/cards?name=${search}`)
      .then((r) => r.json())
      .then((data) => setResults(data.items))
      .catch((e) => console.error(e));
  }, [search]);

  // Añadir una carta al mazo
  const addCard = async (cardId) => {
    await fetch(`http://localhost:8080/api/decks/${id}/cards`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({ cardId, quantity: 1 }),
    });
    loadDeck(); // recargar para ver la carta añadida
  };

  if (!deck) {
    return <p>Cargando...</p>;
  }

  return (
    <div>
      <h2>{deck.name}</h2>
      <p>Formato: {deck.format}</p>

      <h3>Cartas ({deck.cards.length})</h3>
      <ul>
        {deck.cards.map((card) => (
          <li key={card.cardId}>
            {card.quantity}x {card.cardName}
          </li>
        ))}
      </ul>

      <h3>Añadir cartas</h3>
      <input
        type="text"
        placeholder="Buscar carta..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
      />
      <ul>
        {results.map((card) => (
          <li key={card.oracleId}>
            {card.name}{" "}
            <button onClick={() => addCard(card.oracleId)}>Añadir</button>
          </li>
        ))}
      </ul>
    </div>
  );
}

export default DeckDetail;