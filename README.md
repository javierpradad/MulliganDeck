# MulliganDeck

## Cómo ejecutar

1. Copia el fichero `.env.example` a un nuevo fichero llamado `.env` en la carpeta raíz.
2. Edita `.env` y rellena las variables POSTGRES_PASSWORD y ADMIN_PASSWORD con las contraseñas que prefieras.
3. Levanta todo:
   docker compose up --build
4. La API estará en http://localhost:8080/api/cards