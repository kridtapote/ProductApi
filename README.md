# ProductApi (.NET 9) - JWT Authentication example


## Quick start


1. Build & run with Docker Compose:


```bash
docker-compose up --build
```


- API will be available at http://localhost:8080
- DB (Postgres) at port 5433 on the host


2. Example login request:


POST http://localhost:8080/api/auth/login
```json
{ "username": "admin", "password": "password" }
```


Response:
```json
{ "token": "<jwt-token-here>" }
```


3. Use the token in Authorization header for protected endpoints:


`Authorization: Bearer <jwt-token-here>`


### Notes
- This example seeds a demo user `admin` / `password` on first run.
- Change `JwtSettings:SecretKey` to a strong secret in production (>= 32 chars).
- For production, manage secrets via environment variables or secret stores.
Swagger UI
http://localhost:8080/swagger