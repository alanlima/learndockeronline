#!/bin/sh

envsubst '$PROXY_PROTOCOL,$PROXY_UPSTREAM' < /etc/nginx/conf.d/default.conf.template > /etc/nginx/conf.d/default.conf

exec "$@"