#!/usr/bin/env bash

HOST=$1
PORT=$2
shift 2

TIMEOUT=30
COMMAND="$@"

for i in $(seq 1 $TIMEOUT); do
  nc -z $HOST $PORT && break
  echo "Waiting for $HOST:$PORT..."
  sleep 1
done

if [ "$i" == "$TIMEOUT" ]; then
  echo "Error: Timeout while waiting for $HOST:$PORT to be ready."
  exit 1
fi

exec $COMMAND
