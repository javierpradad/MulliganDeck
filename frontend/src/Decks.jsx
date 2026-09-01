import { useState, useEffect } from "react";
import { Link } from "react-router-dom";

function Decks() {
  const [decks, setDecks] = useState([]);
  const [name, setName] = useState("");
  const [format, setFormat] = useState("Standard");

  const token = localStorage.getItem("token");

  const loadDecks = () => {
    fetch("http://localhost:8080/api/decks", {
      headers: { Authorization: `Bearer ${token}` },
    })
      .then((r) => r.json())
      .then((data) => setDecks(data))
      .catch((e) => console.error(e));
  };

  useEffect(() => {
    loadDecks();
  }, []);

  const handleCreate = async (e) => {
    e.preventDefault();

    await fetch("http://localhost:8080/api/decks", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({ name, format }),
    });

    setName("");
    loadDecks();
  };

  const deleteDeck = async (deckId) => {
    if (!confirm("¿Seguro que quieres borrar este mazo?")) return;

    await fetch(`http://localhost:8080/api/decks/${deckId}`, {
        method: "DELETE",
        headers: { Authorization: `Bearer ${token}` },
    });
    loadDecks();
  };

  return (
    <div>
      <h2>Mis mazos</h2>

      <form onSubmit={handleCreate}>
        <input
          type="text"
          placeholder="Nombre del mazo"
          value={name}
          onChange={(e) => setName(e.target.value)}
        />
        <select value={format} onChange={(e) => setFormat(e.target.value)}>
          <option value="Standard">Standard</option>
          <option value="Commander">Commander</option>
        </select>
        <button type="submit">Crear mazo</button>
      </form>

      <ul>
        {decks.map((deck) => (
            <li key={deck.id}>
            <Link to={`/mazos/${deck.id}`}>
                {deck.name} ({deck.format})
                </Link>{" "}
                <button onClick={() => deleteDeck(deck.id)}>Borrar</button>
            </li>
        ))}
        </ul>
    </div>
  );
}

export default Decks;