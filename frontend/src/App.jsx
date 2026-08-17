import { useState, useEffect } from "react";

function App() {
  const [cards, setCards] = useState([]);

  useEffect(() => {
    fetch("http://localhost:8080/api/cards")
      .then((response) => response.json())
      .then((data) => setCards(data.items))
      .catch((error) => console.error("Error:", error));
  }, []);

  return (
    <div>
      <h1>MulliganDeck</h1>
      <p>Gestor de mazos de Magic: The Gathering</p>
      <ul>
        {cards.map((card) => (
          <li key={card.oracleId}>{card.name}</li>
        ))}
      </ul>
    </div>
  );
}

export default App;