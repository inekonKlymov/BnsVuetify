#!/bin/sh

set -e

nginx -g 'daemon off;' &
NGINX_PID=$!

while inotifywait -e modify /etc/nginx/nginx.conf; do
  echo "Reloading nginx..."
  nginx -s reload
  sleep 1
  if ! kill -0 $NGINX_PID 2>/dev/null; then
    echo "nginx process exited. Exiting entrypoint."
    exit 1
  fi
done
