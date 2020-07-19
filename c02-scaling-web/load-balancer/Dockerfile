FROM nginx:1.19

RUN apt-get update && apt-get install gettext-base

ENV PROXY_PROTOCOL=http PROXY_UPSTREAM=example.com

COPY docker-entrypoint.sh .

COPY nginx/default.conf /etc/nginx/conf.d/default.conf.template

ENTRYPOINT [ "/docker-entrypoint.sh" ]

CMD [ "nginx", "-g", "daemon off;" ]