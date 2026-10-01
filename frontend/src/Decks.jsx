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
    <div className="max-w-4xl mx-auto px-6 py-8">
      <h1 className="text-3xl font-bold text-neptune-100 font-display mb-6">
        Mis mazos
      </h1>

      <form
        onSubmit={handleCreate}
        className="bg-neptune-900 border border-neptune-800 rounded-xl p-5 mb-8 flex gap-3 items-end"
      >
        <div className="flex-1">
          <label className="block text-neptune-400 text-xs font-medium uppercase tracking-wider mb-2">
            Nombre del mazo
          </label>
          <input
            type="text"
            placeholder="Mi mazo..."
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="w-full px-4 py-2.5 bg-neptune-950 border border-neptune-800 rounded-lg text-neptune-50 placeholder-neptune-600 focus:outline-none focus:border-neptune-400 transition"
          />
        </div>
        <div>
          <label className="block text-neptune-400 text-xs font-medium uppercase tracking-wider mb-2">
            Formato
          </label>
          <select
            value={format}
            onChange={(e) => setFormat(e.target.value)}
            className="px-4 py-2.5 bg-neptune-950 border border-neptune-800 rounded-lg text-neptune-50 focus:outline-none focus:border-neptune-400 transition"
          >
            <option value="Standard">Standard</option>
            <option value="Commander">Commander</option>
          </select>
        </div>
        <button
          type="submit"
          className="bg-neptune-400 text-neptune-950 px-5 py-2.5 rounded-lg hover:bg-neptune-300 transition font-semibold"
        >
          Crear
        </button>
      </form>

      {decks.length === 0 ? (
        <p className="text-neptune-500 text-center py-12">
          No tienes mazos todavía. Crea el primero arriba.
        </p>
      ) : (
        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
          {decks.map((deck) => (
            <div
              key={deck.id}
              className="bg-neptune-900 border border-neptune-800 rounded-xl p-5 hover:border-neptune-600 transition flex justify-between items-center"
            >
              <Link to={`/mazos/${deck.id}`} className="flex-1">
                <h3 className="text-neptune-100 font-semibold text-lg">{deck.name}</h3>
                <span className="text-neptune-500 text-sm">{deck.format}</span>
              </Link>
              <button
                onClick={() => deleteDeck(deck.id)}
                className="text-neptune-500 hover:text-red-400 transition text-sm ml-4"
              >
                Borrar
              </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default Decks;