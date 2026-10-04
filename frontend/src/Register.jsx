import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { API_URL } from "./api";

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
      const response = await fetch(`${API_URL}/api/auth/register`, {
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
    <div className="min-h-screen flex bg-neptune-950">
        <div
        className="hidden md:flex md:w-1/2 flex-col justify-between p-12 relative bg-neptune-900 bg-cover bg-center"
        style={{
            backgroundImage:
            "linear-gradient(rgba(28,43,48,0.85), rgba(28,43,48,0.95)), url('https://images.unsplash.com/photo-1732944710507-feecdfb4f2c1?q=80&w=687&auto=format&fit=crop&ixlib=rb-4.1.0&ixid=M3wxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8fA%3D%3D')",
        }}
        >
        <h1 className="text-3xl font-extrabold text-neptune-50 font-display tracking-tight">
            Mulligan<span className="text-neptune-400">Deck</span>
        </h1>

        <div>
            <h2 className="text-4xl font-bold text-neptune-50 font-display leading-tight mb-4">
            Únete y empieza<br />a construir.
            </h2>
            <p className="text-neptune-300 text-lg max-w-md">
            Crea tu cuenta para guardar tus mazos, gestionar tu colección y
            validar tus creaciones.
            </p>
        </div>

        <p className="text-neptune-500 text-sm">
            © 2026 MulliganDeck · Magic: The Gathering
        </p>
        </div>

        {/* Panel derecho: formulario */}
        <div className="flex-1 flex items-center justify-center px-8">
        <form onSubmit={handleSubmit} className="w-full max-w-sm">
            <h2 className="text-2xl font-bold text-neptune-100 font-display mb-1">
            Crear cuenta
            </h2>
            <p className="text-neptune-500 text-sm mb-8">
            Regístrate para empezar
            </p>

            <label className="block text-neptune-400 text-xs font-medium uppercase tracking-wider mb-2">
            Email
            </label>
            <input
            type="email"
            placeholder="tu@email.com"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            className="w-full px-4 py-3 mb-5 bg-neptune-900 border border-neptune-800 rounded-lg text-neptune-50 placeholder-neptune-600 focus:outline-none focus:border-neptune-400 transition"
            />

            <label className="block text-neptune-400 text-xs font-medium uppercase tracking-wider mb-2">
            Contraseña
            </label>
            <input
            type="password"
            placeholder="••••••••"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            className="w-full px-4 py-3 mb-6 bg-neptune-900 border border-neptune-800 rounded-lg text-neptune-50 placeholder-neptune-600 focus:outline-none focus:border-neptune-400 transition"
            />

            <button
            type="submit"
            className="w-full bg-neptune-400 text-neptune-950 py-3 rounded-lg hover:bg-neptune-300 transition font-semibold"
            >
            Registrarse
            </button>

            {error && <p className="text-red-400 text-sm mt-4">{error}</p>}
            {success && (
            <p className="text-green-400 text-sm mt-4">
                Cuenta creada. Redirigiendo...
            </p>
            )}

            <p className="text-neptune-500 text-sm mt-8">
            ¿Ya tienes cuenta?{" "}
            <Link to="/login" className="text-neptune-300 hover:text-neptune-200 font-medium">
                Inicia sesión
            </Link>
            </p>
        </form>
        </div>
    </div>
    );
}

export default Register;

