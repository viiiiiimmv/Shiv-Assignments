#!/bin/zsh

set -u
unsetopt BG_NICE

EXISTING_PID=$(lsof -ti tcp:5111 -sTCP:LISTEN 2>/dev/null || true)
if [ -n "$EXISTING_PID" ]; then
    kill $EXISTING_PID 2>/dev/null || true
    sleep 1
fi

TEST_IMAGE=/tmp/ems-swagger-test.png
printf %s "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO5sZ8kAAAAASUVORK5CYII=" | base64 -d > "$TEST_IMAGE"

dotnet ./bin/Debug/net8.0/EMPLOYEE-MANAGEMENT-SYS.dll --urls http://127.0.0.1:5111 >/tmp/ems-api.log 2>&1 &
APP_PID=$!

cleanup() {
    kill $APP_PID 2>/dev/null || true
    wait $APP_PID 2>/dev/null || true
}

trap cleanup EXIT

READY=0
for _ in {1..30}; do
    if curl -sS --max-time 5 http://127.0.0.1:5111/health >/tmp/ems-health.json 2>/dev/null; then
        READY=1
        break
    fi
    sleep 1
done

if [ "$READY" -ne 1 ]; then
    echo "APP_FAILED"
    echo "--- JOBS ---"
    jobs -l || true
    echo "--- PORTS ---"
    lsof -nP -iTCP -a -p $APP_PID || true
    echo "--- LOG ---"
    sed -n "1,200p" /tmp/ems-api.log
    exit 1
fi

echo "--- HEALTH ---"
cat /tmp/ems-health.json
echo

echo "--- ROOT ---"
curl -sS --max-time 5 http://127.0.0.1:5111/ | rg -n "swagger-ui|Swagger UI|Employee Management API" || true

echo "--- OPENAPI ---"
curl -sS --max-time 5 http://127.0.0.1:5111/swagger/v1/swagger.json | rg -n "\"/api/Employee\"|\"/health\"" || true

EMAIL=swagger.$RANDOM@example.com
CREATE_RESPONSE=$(curl -sS --max-time 10 -X POST http://127.0.0.1:5111/api/Employee \
  -F "FirstName=Swagger" \
  -F "LastName=Tester" \
  -F "Email=$EMAIL" \
  -F "Age=24" \
  -F "Image=@$TEST_IMAGE;type=image/png")

echo "--- CREATE ---"
echo "$CREATE_RESPONSE"

EMPLOYEE_ID=$(echo "$CREATE_RESPONSE" | rg -o "\"id\":\\s*[0-9]+" | head -n1 | rg -o "[0-9]+")
IMAGE_PATH=$(echo "$CREATE_RESPONSE" | sed -n "s/.*\"imagePath\":\"\\([^\"]*\\)\".*/\\1/p")

echo "EMPLOYEE_ID=$EMPLOYEE_ID"
echo "IMAGE_PATH=$IMAGE_PATH"

echo "--- GET ONE ---"
curl -sS --max-time 5 http://127.0.0.1:5111/api/Employee/$EMPLOYEE_ID
echo

echo "--- GET PAGED ---"
curl -sS --max-time 5 "http://127.0.0.1:5111/api/Employee?pageNumber=1&pageSize=1"
echo

echo "--- IMAGE ---"
curl -i -sS --max-time 5 http://127.0.0.1:5111$IMAGE_PATH | sed -n "1,8p"

echo "--- UPDATE ---"
curl -sS --max-time 10 -X PUT http://127.0.0.1:5111/api/Employee/$EMPLOYEE_ID \
  -F "FirstName=Swagger" \
  -F "LastName=Updated" \
  -F "Email=$EMAIL" \
  -F "Age=25"
echo

echo "--- DELETE STATUS ---"
curl -sS -o /tmp/ems-delete.out -w "%{http_code}" --max-time 5 -X DELETE http://127.0.0.1:5111/api/Employee/$EMPLOYEE_ID
echo

echo "--- GET AFTER DELETE STATUS ---"
curl -sS -o /tmp/ems-after-delete.out -w "%{http_code}" --max-time 5 http://127.0.0.1:5111/api/Employee/$EMPLOYEE_ID
echo
