#! /bin/sh

docker-compose -f docker-compose.yml -f docker-compose.dev.override.yml up --remove-orphans -d