DOCKER_REPO?=alanrlima


build:
	@echo Building images
	@docker-compose build
.PHONY: build

push:
	@echo Tagging images
	docker tag learndocker-nginx:stretch-full ${DOCKER_REPO}/learndocker-nginx:stretch-full
	docker tag learndocker-nginx:stretch-light ${DOCKER_REPO}/learndocker-nginx:stretch-light
	docker tag learndocker-nginx:stretch-extras ${DOCKER_REPO}/learndocker-nginx:stretch-extras

	@echo Pushing to repo
	docker push ${DOCKER_REPO}/learndocker-nginx:stretch-full
	docker push ${DOCKER_REPO}/learndocker-nginx:stretch-light
	docker push ${DOCKER_REPO}/learndocker-nginx:stretch-extras
.PHONY: push

build-push: build push