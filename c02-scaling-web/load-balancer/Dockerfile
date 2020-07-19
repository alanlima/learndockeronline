FROM nginx:1.19-alpine

RUN apk --update --no-cache add \
    gettext

ENV PROXY_PROTOCOL=http PROXY_UPSTREAM=example.com

COPY docker-entrypoint.sh .

COPY nginx/default.conf /etc/nginx/conf.d/default.conf.template

ENTRYPOINT [ "/docker-entrypoint.sh" ]

CMD [ "nginx", "-g", "daemon off;" ]