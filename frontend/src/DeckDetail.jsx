import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";

function DeckDetail() {
  const { id } = useParams();
  const [deck, setDeck] = useState(null);
  const [search, setSearch] = useState("");
  const [results, setResults] = useState([]);
  const [validation, setValidation] = useState(null);
  const [editName, setEditName] = useState("");
  const [editFormat, setEditFormat] = useState("");

  const token = localStorage.getItem("token");

  useEffect(() => {
    loadDeck();
  }, [id]);

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

  const addCard = async (cardId) => {
    await fetch(`http://localhost:8080/api/decks/${id}/cards`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({ cardId, quantity: 1 }),
    });
    loadDeck();
  };

  const removeCard = async (cardId) => {
    await fetch(`http://localhost:8080/api/decks/${id}/cards/${cardId}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` },
    });
    loadDeck();
    };

  const setCommander = async (cardId) => {
    const response = await fetch(`http://localhost:8080/api/decks/${id}/commander`, {
        method: "PUT",
        headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ cardId }),
    });

    if (!response.ok) {
        const data = await response.json();
        alert(data.message);
        return;
    }

    loadDeck();
    };

  const loadDeck = () => {
    fetch(`http://localhost:8080/api/decks/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
    })
        .then((r) => r.json())
        .then((data) => {
        setDeck(data);
        setEditName(data.name);
        setEditFormat(data.format);
        })
        .catch((e) => console.error(e));
    };

  const validateDeck = async () => {
    const response = await fetch(`http://localhost:8080/api/decks/${id}/validate`, {
        method: "POST",
        headers: { Authorization: `Bearer ${token}` },
    });
    const data = await response.json();
    setValidation(data);
    };

  if (!deck) {
    return <p>Cargando...</p>;
  }

  const updateDeck = async (e) => {
    e.preventDefault();
    await fetch(`http://localhost:8080/api/decks/${id}`, {
        method: "PUT",
        headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({ name: editName, format: editFormat }),
    });
    loadDeck();
  };

  return (
    <div>
      <h2>{deck.name}</h2>
      <p>Formato: {deck.format}</p>
      {deck.commanderName && (
        <p><strong>Comandante:</strong> {deck.commanderName}</p>
      )}

      <form onSubmit={updateDeck}>
        <input
            type="text"
            value={editName}
            onChange={(e) => setEditName(e.target.value)}
        />
        <select value={editFormat} onChange={(e) => setEditFormat(e.target.value)}>
            <option value="Standard">Standard</option>
            <option value="Commander">Commander</option>
        </select>
        <button type="submit">Guardar cambios</button>
        </form>

      <button onClick={validateDeck}>Validar mazo</button>

      {validation && (
        <div>
            {validation.isValid ? (
            <p style={{ color: "green" }}>✓ El mazo es válido</p>
            ) : (
            <div style={{ color: "red" }}>
                <p>✗ El mazo no es válido:</p>
                <ul>
                {validation.errors.map((error, index) => (
                    <li key={index}>{error}</li>
                ))}
                </ul>
            </div>
            )}
        </div>
        )}

      <h3>Cartas ({deck.cards.length})</h3>
      <ul>
        {deck.cards.map((card) => (
          <li key={card.cardId}>
            {card.quantity}x {card.cardName}{" "}
            <button onClick={() => removeCard(card.cardId)}>Quitar</button>{" "}
            <button onClick={() => setCommander(card.cardId)}>Comandante</button>
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