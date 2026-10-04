import { useState, useEffect } from "react";
import { useParams } from "react-router-dom";
import CardImage from "./CardImage";

function DeckDetail() {
    const { id } = useParams();
    const [deck, setDeck] = useState(null);
    const [search, setSearch] = useState("");
    const [results, setResults] = useState([]);
    const [validation, setValidation] = useState(null);
    const [hoveredCard, setHoveredCard] = useState(null);
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
            // Preview inicial: la primera carta con imagen
            const firstWithImage = data.cards.find((c) => c.cardImageUri);
            if (firstWithImage) setHoveredCard(firstWithImage.cardImageUri);
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

    const groupCardsByType = (cards) => {
    const groups = {
        Comandante: [],
        Criaturas: [],
        Planeswalkers: [],
        Instantáneos: [],
        Conjuros: [],
        Encantamientos: [],
        Artefactos: [],
        Tierras: [],
        Otros: [],
    };

    for (const card of cards) {
        const type = card.cardTypeLine || "";
        if (deck.commanderId === card.cardId) groups.Comandante.push(card);
        else if (type.includes("Creature")) groups.Criaturas.push(card);
        else if (type.includes("Planeswalker")) groups.Planeswalkers.push(card);
        else if (type.includes("Instant")) groups.Instantáneos.push(card);
        else if (type.includes("Sorcery")) groups.Conjuros.push(card);
        else if (type.includes("Enchantment")) groups.Encantamientos.push(card);
        else if (type.includes("Artifact")) groups.Artefactos.push(card);
        else if (type.includes("Land")) groups.Tierras.push(card);
        else groups.Otros.push(card);
    }

    return groups;
    };

    if (!deck) return <p className="text-neptune-400 p-8">Cargando...</p>;

    const groups = groupCardsByType(deck.cards);

    return (
    <div className="max-w-7xl mx-auto px-6 py-8">
        {/* Banner de cabecera */}
        <div className="relative rounded-2xl overflow-hidden mb-6 bg-gradient-to-br from-neptune-800 via-neptune-900 to-neptune-950 border border-neptune-800 p-8">
        <h1 className="text-4xl font-bold text-neptune-50 font-display">{deck.name}</h1>
        <div className="flex items-center gap-3 mt-3">
            <span className="text-xs uppercase tracking-wider bg-neptune-700/60 text-neptune-200 px-3 py-1 rounded-full">
            {deck.format}
            </span>
            <span className="text-neptune-400 text-sm">{deck.cards.length} cartas</span>
            {deck.commanderName && (
            <span className="text-neptune-400 text-sm">· {deck.commanderName}</span>
            )}
        </div>

        <button
            onClick={validateDeck}
            className="absolute top-6 right-6 bg-amber-400 text-neptune-950 px-5 py-2.5 rounded-lg hover:bg-amber-300 transition font-semibold"
        >
            Validar mazo
        </button>
        </div>

        {/* Validación */}
        {validation && (
        <div className={`rounded-xl p-4 mb-6 border ${validation.isValid ? "bg-green-950/30 border-green-800 text-green-300" : "bg-red-950/30 border-red-900 text-red-300"}`}>
            {validation.isValid ? (
            <p className="font-medium">✓ El mazo es válido</p>
            ) : (
            <div>
                <p className="font-medium mb-2">✗ El mazo no es válido:</p>
                <ul className="list-disc list-inside space-y-1 text-sm">
                {validation.errors.map((e, i) => <li key={i}>{e}</li>)}
                </ul>
            </div>
            )}
        </div>
        )}

        {/* Buscador para añadir */}
        <input
        type="text"
        placeholder="Añadir una carta..."
        value={search}
        onChange={(e) => setSearch(e.target.value)}
        className="w-full px-4 py-3 mb-4 bg-neptune-900 border border-neptune-800 rounded-lg text-neptune-50 placeholder-neptune-600 focus:outline-none focus:border-neptune-400 transition"
        />
        {results.length > 0 && (
        <div className="bg-neptune-900 border border-neptune-800 rounded-lg mb-6 max-h-60 overflow-y-auto">
            {results.map((card) => (
            <div
                key={card.oracleId}
                className="px-4 py-2 flex justify-between items-center hover:bg-neptune-800 cursor-pointer"
                onMouseEnter={() => setHoveredCard(card.imageUri)}
            >
                <span className="text-neptune-200 text-sm">{card.name}</span>
                <button onClick={() => addCard(card.oracleId)} className="text-xs bg-neptune-700 text-neptune-100 px-3 py-1 rounded hover:bg-neptune-600 transition">
                Añadir
                </button>
            </div>
            ))}
        </div>
        )}

        {/* Dos columnas: preview + secciones */}
        <div className="flex gap-6">
        <div className="hidden lg:block w-64 shrink-0">
            <div className="sticky top-6">
            {hoveredCard ? (
                <img src={hoveredCard} alt="preview" className="w-full rounded-xl shadow-2xl" />
            ) : (
                <div className="w-full aspect-[5/7] bg-neptune-900 border border-neptune-800 rounded-xl flex items-center justify-center">
                <span className="text-neptune-600 text-sm">Pasa el cursor sobre una carta</span>
                </div>
            )}
            </div>
        </div>

        <div className="flex-1 space-y-6">
            {Object.entries(groups).map(([groupName, groupCards]) =>
            groupCards.length === 0 ? null : (
                <div key={groupName}>
                <h3 className="text-neptune-300 font-display font-semibold mb-3 uppercase text-sm tracking-wider">
                    {groupName} ({groupCards.length})
                </h3>
                <div className="grid grid-cols-3 sm:grid-cols-4 md:grid-cols-5 gap-3">
                    {groupCards.map((card) => (
                    <div key={card.cardId} className="relative group">
                        <CardImage
                        imageUri={card.cardImageUri}
                        faces={card.faces}
                        name={card.cardName}
                        onHover={setHoveredCard}
                        />
                        {card.quantity > 1 && (
                        <span className="absolute top-1 left-1 bg-neptune-950/90 text-neptune-100 text-xs font-bold px-2 py-0.5 rounded-full z-10">
                            {card.quantity}
                        </span>
                        )}
                        <div className="absolute inset-x-0 bottom-0 p-2 flex justify-center gap-2 opacity-0 group-hover:opacity-100 transition z-10">
                        {deck.format === "Commander" && (
                            <button onClick={() => setCommander(card.cardId)} className="text-xs bg-neptune-600 text-white px-2 py-1 rounded shadow-lg">Cmd</button>
                        )}
                        <button onClick={() => removeCard(card.cardId)} className="text-xs bg-red-900 text-white px-2 py-1 rounded shadow-lg">Quitar</button>
                        </div>
                    </div>
                    ))}
                </div>
                </div>
            )
            )}
        </div>
        </div>
    </div>
    );
}

export default DeckDetail;