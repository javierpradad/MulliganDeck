import { useState } from "react";
import { BrowserRouter, Routes, Route, Link, Navigate } from "react-router-dom";
import Login from "./Login";
import Cards from "./Cards";

function App() {
  const [token, setToken] = useState(localStorage.getItem("token"));

  const handleLogin = (newToken) => {
    localStorage.setItem("token", newToken);
    setToken(newToken);
  };

  const handleLogout = () => {
    localStorage.removeItem("token");
    setToken(null);
  };

  return (
    <BrowserRouter>
      <nav>
        <Link to="/cartas">Cartas</Link>
        {token && <button onClick={handleLogout}>Cerrar sesión</button>}
      </nav>

      <Routes>
        <Route
          path="/login"
          element={token ? <Navigate to="/cartas" /> : <Login onLogin={handleLogin} />}
        />
        <Route
          path="/cartas"
          element={token ? <Cards /> : <Navigate to="/login" />}
        />
        <Route path="*" element={<Navigate to="/cartas" />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;