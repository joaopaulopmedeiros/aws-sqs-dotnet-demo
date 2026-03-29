test:
	dotnet test

up:
	docker compose up --build -d

down:
	docker compose down

load:
	docker compose --profile load up k6 --abort-on-container-exit --exit-code-from k6
