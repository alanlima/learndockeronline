FROM debian:buster-slim

ARG NGINX_VERSION
ENV NGINX_PKG nginx-${NGINX_VERSION:-full}

RUN apt-get -q update  \
    && apt-get -q install -y ${NGINX_PKG} \
    && rm /var/log/nginx/access.log \
    && rm /var/log/nginx/error.log \
    && ln -s /dev/stdout /var/log/nginx/access.log \
    && ln -s /dev/stderr /var/log/nginx/error.log \
    && rm -rf /var/lib/apt/lists/*

CMD ["/usr/sbin/nginx", "-g", "daemon off;"]

# Send signal to nginx to shutdown gracefully. Finish pending requests before terminate.
STOPSIGNAL SIGQUIT

LABEL maintainer="Alan Lima" app.version="${NGINX_PKG}"