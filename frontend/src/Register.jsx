import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";

function Register() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    try {
      const response = await fetch("http://localhost:8080/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ email, password }),
      });

      if (response.status === 409) {
        setError("Ya existe un usuario con ese email");
        return;
      }

      if (!response.ok) {
        setError("Error al registrar");
        return;
      }

      setSuccess(true);
      setTimeout(() => navigate("/login"), 1500);
    } catch {
      setError("Error de conexión");
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h2>Crear cuenta</h2>
      <input
        type="email"
        placeholder="Email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />
      <input
        type="password"
        placeholder="Contraseña"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <button type="submit">Registrarse</button>
      {error && <p style={{ color: "red" }}>{error}</p>}
      {success && <p style={{ color: "green" }}>Cuenta creada. Redirigiendo...</p>}
    </form>
  );
}

export default Register;