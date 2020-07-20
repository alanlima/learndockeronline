# Output commands

## Create frontend service

```bash
docker service create \
    --name demo-frontend \
    --network service-net \
    jfahrer/swarm-demo-frontend:v4
```

## Create load balancer service

```bash
docker service create \
    --name demo-lb \
    --network service-net \
    --publish 80:80 \
    --env PROXY_UPSTREAM=demo-frontend:8080 \
    jfahrer/swarm-demo-lb:v1
```

## Scale demo-frontend

```bash
docker service scale demo-frontend=5
```

## Update with rollback

```bash
docker service update \
    --update-failure-action rollback \
    --update-parallelism 3 \
    --update-monitor 10s \
    --image jfahrer/swarm-demo-frontend:v3 \
    --rollback-parallelism 3 demo-frontend
```
