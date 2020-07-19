FROM python:3.8-alpine3.12 as base

WORKDIR /app

COPY requirements.txt .

RUN pip install -r requirements.txt

FROM base

COPY *.py .

ENTRYPOINT ["python"]

CMD ["app.py"]