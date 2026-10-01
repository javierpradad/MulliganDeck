import { useState } from "react";
import { BrowserRouter, Routes, Route, Link, Navigate, useLocation } from "react-router-dom";
import Login from "./Login";
import Register from "./Register";
import Cards from "./Cards";
import Decks from "./Decks";
import DeckDetail from "./DeckDetail";

function AppContent() {
  const [token, setToken] = useState(localStorage.getItem("token"));
  const location = useLocation();

  const handleLogin = (newToken) => {
    localStorage.setItem("token", newToken);
    setToken(newToken);
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    setToken(null);
  };

  const hideNav = location.pathname === "/login" || location.pathname === "/register";

  return (
    <div className="min-h-screen bg-neptune-950">
      {!hideNav && (
        <nav className="bg-neptune-900 border-b border-neptune-800 px-6 py-4 flex items-center gap-6">
          <Link to="/cartas" className="text-lg hover:opacity-80 font-display font-bold">
            <span className="text-neptune-100">Mulligan</span>
            <span className="text-neptune-400">Deck</span>
          </Link>
          <div className="flex items-center gap-4 ml-auto">
            <Link to="/cartas" className="text-neptune-300 hover:text-white transition">
              Cartas
            </Link>
            {token && (
              <Link to="/mazos" className="text-neptune-300 hover:text-white transition">
                Mazos
              </Link>
            )}
            {token ? (
              <button
                onClick={handleLogout}
                className="bg-neptune-700 text-white px-4 py-1.5 rounded-lg hover:bg-neptune-600 transition"
              >
                Cerrar sesión
              </button>
            ) : (
              <Link
                to="/login"
                className="bg-neptune-500 text-white px-4 py-1.5 rounded-lg hover:bg-neptune-400 transition"
              >
                Iniciar sesión
              </Link>
            )}
          </div>
        </nav>
      )}

      <Routes>
        <Route
          path="/login"
          element={token ? <Navigate to="/cartas" /> : <Login onLogin={handleLogin} />}
        />
        <Route
          path="/register"
          element={token ? <Navigate to="/cartas" /> : <Register />}
        />
        <Route path="/cartas" element={<Cards />} />
        <Route
          path="/mazos"
          element={token ? <Decks /> : <Navigate to="/login" />}
        />
        <Route
          path="/mazos/:id"
          element={token ? <DeckDetail /> : <Navigate to="/login" />}
        />
        <Route path="*" element={<Navigate to="/cartas" />} />
      </Routes>
    </div>
  );
}

function App() {
  return (
    <BrowserRouter>
      <AppContent />
    </BrowserRouter>
  );
}

export default App;