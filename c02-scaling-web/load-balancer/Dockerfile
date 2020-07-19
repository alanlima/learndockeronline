FROM jfahrer/nginx:latest

RUN apt-get update && apt-get install gettext-base

ENV PROXY_PROTOCOL=http PROXY_UPSTREAM=example.com

COPY nginx/proxy.conf /etc/nginx/sites-available/default.template

COPY nginx/start.sh /

CMD [ "/start.sh" ]

