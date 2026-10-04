import { useState, useEffect } from "react";
import CardImage from "./CardImage";


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
    <div className="max-w-6xl mx-auto px-6 py-8">
      <h1 className="text-3xl font-bold text-neptune-100 font-display mb-2">
        Explorar cartas
      </h1>
      <p className="text-neptune-500 mb-6">
        Busca entre más de 38.000 cartas de Magic: The Gathering
      </p>

      <input
        type="text"
        placeholder="Buscar por nombre..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        className="w-full max-w-md px-4 py-3 mb-8 bg-neptune-900 border border-neptune-800 rounded-lg text-neptune-50 placeholder-neptune-600 focus:outline-none focus:border-neptune-400 transition"
      />

      <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4">
        {cards.map((card) => (
          <CardImage key={card.oracleId} card={card} />
        ))}
      </div>
    </div>
  );
}

export default Cards;