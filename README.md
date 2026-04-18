# Stage 1 API — ASP.NET Core 8 Minimal API

A blazing-fast REST API built with ASP.NET Core 8 **Minimal APIs**.  
No controllers, no middleware overhead — just three endpoints served directly from `Program.cs`.

## Why ASP.NET Core?

- Cold starts under 50 ms in production
- ~1 µs per request latency on Kestrel
- Single self-contained binary after publish (no runtime install on server)
- Built-in Kestrel HTTP server — no extra process layer needed

---

## Endpoints

| Method | Path | Response |
|--------|------|----------|
| GET | `/` | `{ "message": "API is running" }` |
| GET | `/health` | `{ "message": "healthy" }` |
| GET | `/me` | `{ "name": "...", "email": "...", "github": "..." }` |

All endpoints return `Content-Type: application/json` and HTTP `200`.

---

## Run Locally

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

```bash
git clone https://github.com/yourusername/stage1-api.git
cd stage1-api
dotnet run
```

The API listens on **http://localhost:5000** by default.

Test it:
```bash
curl http://localhost:5000/
curl http://localhost:5000/health
curl http://localhost:5000/me
```

---

## Deploy to a VPS (Ubuntu 22.04 / 24.04)

### 1 — Build a self-contained binary on your dev machine

```bash
dotnet publish -c Release -r linux-x64 --self-contained true -o ./publish
```

This produces a **single trimmed binary** (`publish/StageOneApi`) — no .NET runtime required on the server.

### 2 — Copy the binary to the server

```bash
scp -r ./publish/* user@your-server-ip:/var/www/stage1-api/
ssh user@your-server-ip "chmod +x /var/www/stage1-api/StageOneApi"
```

### 3 — Create a systemd service

```bash
sudo cp stage1-api.service /etc/systemd/system/
sudo systemctl daemon-reload
sudo systemctl enable stage1-api
sudo systemctl start stage1-api
sudo systemctl status stage1-api
```

The service runs as `www-data`, auto-restarts on crash, and survives reboots.

### 4 — Configure Nginx reverse proxy

```bash
sudo cp nginx-stage1-api.conf /etc/nginx/sites-available/stage1-api
sudo ln -s /etc/nginx/sites-available/stage1-api /etc/nginx/sites-enabled/
# Edit the server_name line to your domain or IP
sudo nano /etc/nginx/sites-available/stage1-api
sudo nginx -t && sudo systemctl reload nginx
```

### 5 — (Optional) Add TLS with Certbot

```bash
sudo apt install certbot python3-certbot-nginx -y
sudo certbot --nginx -d your-domain.com
```

---

## Live URL

**http://54.196.87.169**

---

## Project Structure

```
stage1-api/
├── Program.cs                 # All three endpoints — ~20 lines total
├── StageOneApi.csproj         # SDK-style project; self-contained publish
├── appsettings.json           # Kestrel bound to localhost:5000
├── stage1-api.service         # systemd unit — keeps the service alive
├── nginx-stage1-api.conf      # Nginx reverse-proxy block
└── README.md
```
